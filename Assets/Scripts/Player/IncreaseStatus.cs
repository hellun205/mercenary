using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Player
{
  public enum StatusValueType
  {
    Normal, Fixed, PercentPlus
  }

  [Serializable]
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
    public string range;
    public string knockback;

    public string explosionRange;

    public string armor;

    public string moveSpeed;
    public string luck;

    public string evasionRate;

    public string penetrateCount;
    public string errorRange;
    public string multipleCritical;

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
      else type = StatusValueType.Normal;

      return Convert.ToSingle(value);
    }

    private string GetTypeChar(StatusValueType type)
      => type switch
      {
        StatusValueType.Normal      => "",
        StatusValueType.Fixed       => "=",
        StatusValueType.PercentPlus => "%+",
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
      return type switch
      {
        StatusValueType.Normal      => setValueGetter.Invoke(originalValue, fieldValue),
        StatusValueType.Fixed       => fieldValue,
        StatusValueType.PercentPlus => originalValue * (1 + fieldValue),
        _                           => throw new ArgumentOutOfRangeException()
      };
    }

    public static IncreaseStatus operator *(IncreaseStatus a, int b)
    {
      var res = a;

      res.SetValues((n, v) => v * b);
      // res.SetValue("armor", v => v * b);
      // res.SetValue("knockback", v => v * b);
      // res.SetValue("criticalPercent", v => v * b);
      // res.SetValue("bleedingDamage", v => v * b);
      // res.SetValue("regeneration", v => v * b);
      // res.SetValue("luck", v => v * b);
      // res.SetValue("range", v => v * b);
      // res.SetValue("drainHp", v => v * b);
      // res.SetValue("meleeDamage", v => v * b);
      // res.SetValue("moveSpeed", v => v * b);
      // res.SetValue("rangedDamage", v => v * b);
      // res.SetValue("explosionRange", v => v * b);
      // res.SetValue("rangedDamage", v => v * b);
      // res.SetValue("evasionRate", v => v * b);

      return res;
    }

    public static IncreaseStatus operator +(IncreaseStatus a, IncreaseStatus b)
    {
      var res = a;

      res.SetValues((n, v) => v + b.GetValue(n));
      // res.SetValue("armor", v => v + b.GetValue("armor"));
      // res.SetValue("knockback", v => v + b.GetValue("knockback"));
      // res.SetValue("criticalPercent", v => v + b.GetValue("criticalPercent"));
      // res.SetValue("bleedingDamage", v => v + b.GetValue("bleedingDamage"));
      // res.SetValue("regeneration", v => v + b.GetValue("regeneration"));
      // res.SetValue("luck", v => v + b.GetValue("luck"));
      // res.SetValue("range", v => v + b.GetValue("range"));
      // res.SetValue("drainHp", v => v + b.GetValue("drainHp"));
      // res.SetValue("meleeDamage", v => v + b.GetValue("meleeDamage"));
      // res.SetValue("moveSpeed", v => v + b.GetValue("moveSpeed"));
      // res.SetValue("rangedDamage", v => v + b.GetValue("rangedDamage"));
      // res.SetValue("explosionRange", v => v + b.GetValue("explosionRange"));
      // res.SetValue("rangedDamage", v => v + b.GetValue("rangedDamage"));
      // res.SetValue("evasionRate", v => v + b.GetValue("evasionRate"));

      return res;
    }

    private static readonly Dictionary<string, (string displayName, Func<float, float> displayValue, string p)> setting
      = new()
      {
        { "maxHp", ("최대 체력", v => v, "") },
        { "regeneration", ("체력 재생", v => v / 10, "") },
        { "drainHp", ("흡혈", v => v * 100, "") },
        { "meleeDamage", ("근거리 피해량", v => v, "") },
        { "rangedDamage", ("원거리 피해량", v => v, "") },
        { "criticalPercent", ("치명타 확률", v => v * 100, "") },
        { "bleedingDamage", ("출혈 피해량", v => v, "") },
        { "attackSpeed", ("공격 속도", v => v * 100, "") },
        { "range", ("범위", v => v, "") },
        { "knockback", ("넉백", v => v, "") },
        { "armor", ("방어력", v => v * 100, "") },
        { "moveSpeed", ("이동 속도", v => v * 100, "") },
        { "luck", ("행운", v => v * 100, "") },
        { "explosionRange", ("폭발 범위", v => v, "") },
        { "evasionRate", ("회피율", v => v * 100, "") },
        { "penetrateCount", ("관통", v => v * 100, "") },
        { "errorRange", ("오차 범위", v => v * 100, "") },
        { "multipleCritical", ("치명타 데미지", v => v * 100, "x") },
      };

    public string GetDescription()
    {
      var sb = new StringBuilder();
      var fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

      foreach (var field in fields.Select(x => x.Name))
      {
        var v = GetValue(field, out var type);
        var set = setting[field];
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
                  subfix: "%"
                )
              ).Append("\n");
              break;

            default:
              sb.Append
              (
                PlayerStatus.GetTextViaValue
                (
                  $"{set.displayName}: ",
                  set.displayValue.Invoke(v),
                  plus: "",
                  prefix: set.p
                )
              ).Append("\n");
              break;
          }
        }
      }

      return sb.ToString();
    }
  }
}
