using System;
using UnityEngine;
using UnityEngine.Serialization;
using Util.Text;

namespace Player
{
  [Serializable]
  public struct PlayerStatus
  {
    [Header("Hp")]
    [Tooltip("최대 체력")]
    public float maxHp;

    [Tooltip("현재 체력")]
    public float hp;

    [Tooltip("초당 체력 재생")]
    public float regeneration;

    [Tooltip("피해 흡혈 (회복할 확률)")]
    [Range(-1f, 1f)]
    public float drainHp;

    [Header("Attack")]
    [Tooltip("근거리 피해량")]
    public float meleeDamage;

    [Tooltip("원거리 피해량")]
    public float rangedDamage;

    [Tooltip("출혈 피해량")]
    public float bleedingDamage;

    [FormerlySerializedAs("criticalPercentage")]
    [Tooltip("치명타율")]
    public float criticalPercent;

    [Tooltip("공격 속도")]
    public float attackSpeed;

    [Tooltip("공격 범위(추가)")]
    public float range;

    [Tooltip("넉백")]
    public float knockback;

    [Tooltip("폭발 범위")]
    public float explosionRange;

    [Header("Other")]
    [Tooltip("방어 %")]
    [Range(-1f, 1f)]
    public float armor;

    [Tooltip("이동 속도")]
    public float moveSpeed;

    [Tooltip("행운")]
    [Range(-1f, 1f)]
    public float luck;

    public float evasionRate;

    // public string GetDescription()
    // {
    //   var sb = new StringBuilder();
    //
    //   if (maxHp != 0)
    //     sb.Append(GetTextViaValue("최대 체력: ", maxHp)).Append("\n");
    //   if (regeneration != 0)
    //     sb.Append(GetTextViaValue("체력 재생: ", regeneration)).Append("\n");
    //   if (drainHp != 0)
    //     sb.Append(GetTextViaValue("흡혈: ", drainHp)).Append("\n");
    //   if (meleeDamage != 0)
    //     sb.Append(GetTextViaValue("근거리 피해량: ", meleeDamage)).Append("\n");
    //   if (rangedDamage != 0)
    //     sb.Append(GetTextViaValue("원거리 피해량: ", rangedDamage)).Append("\n");
    //   if (criticalPercent != 0)
    //     sb.Append(GetTextViaValue("치명타 확률: ", criticalPercent)).Append("\n");
    //   if (bleedingDamage != 0)
    //     sb.Append(GetTextViaValue("출혈 피해량: ", bleedingDamage)).Append("\n");
    //   if (attackSpeed != 0)
    //     sb.Append(GetTextViaValue("공격 속도: ", attackSpeed)).Append("\n");
    //   if (range != 0)
    //     sb.Append(GetTextViaValue("범위: ", range)).Append("\n");
    //   if (knockback != 0)
    //     sb.Append(GetTextViaValue("넉백: ", knockback)).Append("\n");
    //   if (armor != 0)
    //     sb.Append(GetTextViaValue("방어력: ", armor)).Append("\n");
    //   if (moveSpeed != 0)
    //     sb.Append(GetTextViaValue("이동 속도: ", moveSpeed)).Append("\n");
    //   if (luck != 0)
    //     sb.Append(GetTextViaValue("행운: ", luck)).Append("\n");
    //
    //   return sb.ToString();
    // }

    public static string GetTextViaValue
    (
      string text,
      float value,
      float multiple = 1f,
      string prefix = "",
      string subfix = "",
      string plus = "+",
      string minus = "-",
      bool isReverse = false
    )
    {
      var color = (isReverse ? value < 0 : value > 0) ? new Color32(47, 157, 39, 255) : new Color32(152, 0, 0, 255);
      var sign = value > 0 ? plus : minus;
      return $"{text} {$"{sign}{prefix}{Math.Abs(Math.Round(value, 2)) * multiple}{subfix}".AddColor(color)}";
    }

    public static PlayerStatus operator +(PlayerStatus a, IncreaseStatus b)
    {
      var res = a;

      res.maxHp = b.ToValue("maxHp", res.maxHp, (o, v) => o + v);
      res.regeneration = b.ToValue("regeneration", res.maxHp, (o, v) => o + v);
      res.meleeDamage = b.ToValue("meleeDamage", res.meleeDamage, (o, v) => o + v);
      res.rangedDamage = b.ToValue("rangedDamage", res.rangedDamage, (o, v) => o + v);
      res.criticalPercent = b.ToValue("criticalPercent", res.criticalPercent, (o, v) => o + v);
      res.bleedingDamage = b.ToValue("bleedingDamage", res.bleedingDamage, (o, v) => o + v);
      res.attackSpeed = b.ToValue("attackSpeed", res.attackSpeed, (o, v) => o + v);
      res.range = b.ToValue("range", res.range, (o, v) => o + v);
      res.armor = b.ToValue("armor", res.armor, (o, v) => o + v);
      res.knockback = b.ToValue("knockback", res.knockback, (o, v) => o + v);
      res.moveSpeed = b.ToValue("moveSpeed", res.moveSpeed, (o, v) => o + v);
      res.luck = b.ToValue("luck", res.luck, (o, v) => o + v);
      res.explosionRange = b.ToValue("explosionRange", res.explosionRange, (o, v) => o + v);
      res.evasionRate = b.ToValue("evasionRate", res.evasionRate, (o, v) => o + v);

      return res;
    }
  }
}
