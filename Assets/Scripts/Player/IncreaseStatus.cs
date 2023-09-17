using System;
using System.Text;
using UnityEngine;

namespace Player
{
  [Serializable]
  public struct IncreaseStatus
  {
    [Header("Hp")]
    public float maxHp;
    public float regeneration;
    public float drainHp;

    [Header("Attack")]
    public float meleeDamage;

    public float rangedDamage;
    public float bleedingDamage;
    public float criticalPercent;
    public float attackSpeed;
    public float range;
    public float knockback;

    [Header("Other")]
    public float armor;

    public float moveSpeed;
    public float luck;

    public static IncreaseStatus operator *(IncreaseStatus a, int b)
      => new()
      {
        attackSpeed = a.armor * b,
        knockback = a.knockback * b,
        criticalPercent = a.criticalPercent * b,
        bleedingDamage = a.bleedingDamage * b,
        regeneration = a.regeneration * b,
        armor = a.armor * b,
        luck = a.luck * b,
        range = a.range * b,
        drainHp = a.drainHp * b,
        maxHp = a.maxHp * b,
        meleeDamage = a.meleeDamage * b,
        moveSpeed = a.moveSpeed * b,
        rangedDamage = a.rangedDamage * b
      };

    public string GetDescription()
    {
      var sb = new StringBuilder();

      if (maxHp != 0)
        sb.Append(PlayerStatus.GetTextViaValue("최대 체력", maxHp)).Append("\n");
      if (regeneration != 0)
        sb.Append(PlayerStatus.GetTextViaValue("체력 재생", regeneration)).Append("\n");
      if (drainHp != 0)
        sb.Append(PlayerStatus.GetTextViaValue("흡혈", drainHp)).Append("\n");
      if (meleeDamage != 0)
        sb.Append(PlayerStatus.GetTextViaValue("근거리 피해량", meleeDamage)).Append("\n");
      if (rangedDamage != 0)
        sb.Append(PlayerStatus.GetTextViaValue("원거리 피해량", rangedDamage)).Append("\n");
      if (criticalPercent != 0)
        sb.Append(PlayerStatus.GetTextViaValue("치명타 확률", criticalPercent)).Append("\n");
      if (bleedingDamage != 0)
        sb.Append(PlayerStatus.GetTextViaValue("출혈 피해량", bleedingDamage)).Append("\n");
      if (attackSpeed != 0)
        sb.Append(PlayerStatus.GetTextViaValue("공격 속도", attackSpeed)).Append("\n");
      if (range != 0)
        sb.Append(PlayerStatus.GetTextViaValue("범위", range)).Append("\n");
      if (knockback != 0)
        sb.Append(PlayerStatus.GetTextViaValue("넉백", knockback)).Append("\n");
      if (armor != 0)
        sb.Append(PlayerStatus.GetTextViaValue("방어력", armor)).Append("\n");
      if (moveSpeed != 0)
        sb.Append(PlayerStatus.GetTextViaValue("이동 속도", moveSpeed)).Append("\n");
      if (luck != 0)
        sb.Append(PlayerStatus.GetTextViaValue("행운", luck)).Append("\n");

      return sb.ToString();
    }
  }
}
