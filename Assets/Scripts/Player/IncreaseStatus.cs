using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
  public enum StatusValueType
  {
    Normal, Fixed, PercentPlus,
    All
  }

  public struct IncreaseStatus
  {
    public string maxHp;

    public string regeneration;
    public string drainHp;

    public string meleeDamage;

    public string rangedDamage;
    public string bleedingDamage;
    public string criticalPercent;
    public string attackSpeed;
    public string fireRange;
    public string knockback;

    public string explosionRange;

    public string armor;

    public string moveSpeed;
    public string luck;

    public string evasionRate;

    public string penetrateCount;
    public string errorRange;
    public string multipleCritical;
    public string attackDamage;

    public string resurrection;
    public string killEnemy;

    public readonly static Dictionary<string, (float min, float max)> limit = new()
    {
      { "evasionRate", (0f, 0.7f) },
      { "drainHp", (0f, 1f) },
      { "criticalPercent", (0f, 1f) },
      { "armor", (0f, 0.6f) }
    };

    public static (float min, float max) GetLimit(string field)
      => limit.TryGetValue(field, out var value) ? value : (float.MinValue, float.MaxValue);

    public float GetValue(string fieldName)
      => GetValue(fieldName, out _, out _);

    public float GetValue(string fieldName, out StatusValueType type)
      => GetValue(fieldName, out type, out var _);

    public float GetValue(string fieldName, out StatusValueType type, out FieldInfo fieldInfo)
    {
      fieldInfo = GetType().GetField
      (
        fieldName,
        BindingFlags.Public |
        BindingFlags.Instance |
        BindingFlags.DeclaredOnly
      );

      var value = (string) (fieldInfo?.GetValue(this) ?? "0");

      if (value.Contains(GetTypeChar(StatusValueType.PercentPlus)))
      {
        type = StatusValueType.PercentPlus;
        value = value.Replace(GetTypeChar(StatusValueType.PercentPlus), "");
      }
      else if (value.Contains(GetTypeChar(StatusValueType.Fixed)))
      {
        type = StatusValueType.Fixed;
        value = value.Replace(GetTypeChar(StatusValueType.Fixed), "");
      }
      else if (value.Contains(GetTypeChar(StatusValueType.All)))
      {
        type = StatusValueType.All;
        value = float.MaxValue.ToString(CultureInfo.InvariantCulture);
      }
      else type = StatusValueType.Normal;

      return Convert.ToSingle(value);
    }

    private string GetTypeChar(StatusValueType type)
      => type switch
      {
        StatusValueType.Normal      => "",
        StatusValueType.Fixed       => "=",
        StatusValueType.PercentPlus => "%+",
        StatusValueType.All         => "all",
        _                           => throw new ArgumentOutOfRangeException(nameof(type), type, null)
      };

    public void SetValue(string fieldName, float value)
      => SetValue(fieldName, _ => value);

    public void SetValue(string fieldName, Func<float, float> setter)
    {
      var field = GetValue(fieldName, out var type, out var fieldInfo);

      var res = type switch
      {
        StatusValueType.Normal      => setter.Invoke(field),
        StatusValueType.Fixed       => field,
        StatusValueType.PercentPlus => setter.Invoke(field),
        _                           => throw new ArgumentOutOfRangeException()
      };

      fieldInfo.SetValueDirect(__makeref(this), $"{GetTypeChar(type)}{res}");
    }

    public void SetValues(Func<string, float, float> setter)
    {
      var fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

      foreach (var field in fields.Select(x => x.Name))
        SetValue(field, v => setter.Invoke(field, v));
    }

    public float ToValue(string fieldName, float originalValue, Func<float, float, float> setValueGetter)
    {
      var fieldValue = GetValue(fieldName, out var type);
      var lim = GetLimit(fieldName);
      return Mathf.Clamp(type switch
      {
        StatusValueType.Normal      => setValueGetter.Invoke(originalValue, fieldValue),
        StatusValueType.Fixed       => fieldValue,
        StatusValueType.PercentPlus => originalValue * (1 + fieldValue),
        _                           => throw new ArgumentOutOfRangeException()
      }, lim.min, lim.max);
    }

    public static IncreaseStatus operator *(IncreaseStatus a, int b)
    {
      var res = a;
      res.SetValues((n, v) => v * b);
      return res;
    }

    public static IncreaseStatus operator +(IncreaseStatus a, IncreaseStatus b)
    {
      var res = a;
      res.SetValues((n, v) => v + b.GetValue(n));
      return res;
    }

    private static readonly
      Dictionary<string, (string displayName, Func<float, float> displayValue, string p, bool reverse)> setting
        = new()
        {
          { "maxHp", ("최대 체력", v => v, "", false) },
          { "regeneration", ("체력 재생", v => v / 10, "", false) },
          { "drainHp", ("흡혈", v => v * 100, "", false) },
          { "meleeDamage", ("근거리 피해량", v => v, "", false) },
          { "rangedDamage", ("원거리 피해량", v => v, "", false) },
          { "attackDamage", ("데미지", v => v, "", false) },
          { "criticalPercent", ("치명타 확률", v => v * 100, "", false) },
          { "bleedingDamage", ("출혈 피해량", v => v, "", false) },
          { "attackSpeed", ("공격 속도", v => v * 100, "", true) },
          { "fireRange", ("범위", v => v, "", false) },
          { "knockback", ("넉백", v => v, "", false) },
          { "armor", ("방어력", v => v * 100, "", false) },
          { "moveSpeed", ("이동 속도", v => v * 100, "", false) },
          { "luck", ("행운", v => v * 100, "", false) },
          { "explosionRange", ("폭발 범위", v => v, "", false) },
          { "evasionRate", ("회피율", v => v * 100, "", false) },
          { "penetrateCount", ("관통", v => v * 100, "", false) },
          { "multipleCritical", ("치명타 데미지", v => v * 100, "x", false) },
        };

    public string GetDescription(bool useReverse = true)
    {
      var sb = new StringBuilder();
      var fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

      foreach (var field in fields.Select(x => x.Name))
      {
        var v = GetValue(field, out var type);
        if (!setting.TryGetValue(field, out var set))
          continue;

        if (v != 0)
        {
          switch (type)
          {
            case StatusValueType.PercentPlus:
              sb.Append
              (
                PlayerStatus.GetTextViaValue
                (
                  $"{set.displayName}: ",
                  v * 100,
                  plus: "",
                  subfix: "%",
                  isReverse: useReverse && set.reverse
                )
              );
              break;

            default:
              sb.Append
              (
                PlayerStatus.GetTextViaValue
                (
                  $"{set.displayName}: ",
                  set.displayValue.Invoke(v),
                  plus: "",
                  prefix: set.p,
                  isReverse: useReverse && set.reverse
                )
              );
              break;
          }

          sb.Append("\n");
        }
      }

      return sb.ToString();
    }
  }
}
