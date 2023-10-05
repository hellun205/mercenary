using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Player;
using UnityEngine;
using Util;
using Util.Text;

namespace Weapon
{
  [Serializable]
  public struct WeaponStatus
  {
    [Header("Base")]
    public WeaponType type;

    public int price;

    [Header("Attack")]
    public float attackDamage;

    public float attackSpeed;
    public float fireRange;
    public float multipleCritical;
    public float knockback;
    public float bleedingDamage;
    public float criticalPercent;

    // Ranged
    [Header("Ranged")]
    public int penetrate;

    public float errorRange;
    public int bulletCount;
    public float bulletSpeed;

    // Explosion
    [Header("Explosion")]
    public float explosionRange;

    public static WeaponStatus operator +(WeaponStatus a, PlayerStatus b)
      => new()
      {
        attackSpeed = a.attackSpeed * (b.attackSpeed + 1),
        attackDamage = a.attackDamage + a.type switch
        {
          WeaponType.Melee => b.meleeDamage,
          WeaponType.Ranged => b.rangedDamage,
          _ => 0
        },
        fireRange = a.fireRange + a.type switch
        {
          WeaponType.Melee => b.range / 2,
          WeaponType.Ranged => b.range,
          _ => 0,
        },
        criticalPercent = a.multipleCritical * b.criticalPercent,
        multipleCritical = a.multipleCritical,
        knockback = a.knockback + b.knockback,
        bleedingDamage = a.bleedingDamage > 0 ? a.bleedingDamage + b.bleedingDamage : 0,
        type = a.type
      };

    public static WeaponStatus operator +(WeaponStatus a, IncreaseStatus b)
    {
      var res = a;

      res.knockback = b.ToValue("knockback", res.knockback, (o, v) => o + v);
      res.attackSpeed = b.ToValue("attackSpeed", res.attackSpeed, (o, v) => o + v);
      res.bleedingDamage = b.ToValue("bleedingDamage", res.bleedingDamage, (o, v) => o + v);
      res.multipleCritical = b.ToValue("multipleCritical", res.multipleCritical, (o, v) => o + v);
      res.attackDamage = b.ToValue
      (
        res.type == WeaponType.Melee ? "meleeDamage" : "rangedDamage",
        res.attackDamage,
        (o, v) => o + v
      );
      res.bulletSpeed = b.ToValue("bulletSpeed", res.bulletSpeed, (o, v) => o + v);
      res.criticalPercent = b.ToValue("criticalPercent", res.criticalPercent, (o, v) => o + v);
      res.errorRange = b.ToValue("errorRange", res.errorRange, (o, v) => o + v);
      res.explosionRange = b.ToValue("explosionRange", res.explosionRange, (o, v) => o + v);
      res.fireRange = b.ToValue("range", res.fireRange, (o, v) => o + v);
      res.penetrate = (int)b.ToValue("penetrateCount", res.penetrate, (o, v) => o + v);
      res.bulletCount = (int)b.ToValue("penetrateCount", res.penetrate, (o, v) => o + v);

      return res;
    }

    private static readonly
      Dictionary<string, (string displayName, Func<float, float> displayValue, string p, bool reverse)> setting
        = new()
        {
          { "attackDamage", ("데미지", v => v, "", false) },
          { "criticalPercent", ("치명타 확률", v => v * 100, "", false) },
          { "bleedingDamage", ("출혈 피해량", v => v, "", false) },
          { "attackSpeed", ("공격 속도", v => v, "", false) },
          { "fireRange", ("범위", v => v, "", false) },
          { "knockback", ("넉백", v => v, "", false) },
          { "explosionRange", ("폭발 범위", v => v, "", false) },
          { "penetrateCount", ("관통", v => v, "", false) },
          { "errorRange", ("오차 범위", v => v, "", false) },
          { "multipleCritical", ("치명타 데미지", v => v, "x", false) },
        };

    public string GetDescription(IncreaseStatus? additional = null)
    {
      var fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
      var sb = new StringBuilder();

      foreach (var field in fields)
      {
        var value = field.GetValue(this);
        var v = Convert.ToSingle(value);
        if (!setting.TryGetValue(field.Name, out var set)) continue;
        if (v != 0)
        {
          sb.Append(PlayerStatus.GetTextViaValue
            (
              $"{set.displayName}: ",
              set.displayValue.Invoke(v),
              plus: "",
              prefix: set.p,
              isReverse: set.reverse
            )
          );
          if (additional.HasValue)
          {
            var addV = additional.Value.ToValue(field.Name, v, (o, val) => o + val);
            if (!addV.Approximately(v, 0.001f))
              sb.Append($" ({addV})".AddColor(Color.yellow));
          }

          sb.Append("\n");
        }
      }

      return sb.ToString();
    }
  }
}