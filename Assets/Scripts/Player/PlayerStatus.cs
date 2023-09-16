using System;
using System.Text;
using UnityEngine;
using Util;
using Weapon;

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

    [Header("Invincibility")]
    [Tooltip("무적")]
    public bool isInvincibility;

    [Tooltip("무적 시간")]
    public float invincibilityTime;

    [Header("Attack")]
    [Tooltip("근거리 피해량")]
    public float meleeDamage;

    [Tooltip("원거리 피해량")]
    public float rangedDamage;

    [Tooltip("출혈 피해량")]
    public float bleedingDamage;

    [Tooltip("치명타율")]
    public float criticalPercentage;

    [Tooltip("공격 속도")]
    public float attackSpeed;

    [Tooltip("공격 범위(추가)")]
    public float range;

    [Tooltip("넉백")]
    public float knockback;

    [Header("Other")]
    [Tooltip("방어 %")]
    [Range(-1f, 1f)]
    public float armor;

    [Tooltip("이동 속도")]
    public float moveSpeed;

    [Tooltip("행운")]
    [Range(-1f, 1f)]
    public float luck;

    public string GetDescription()
    {
      var sb = new StringBuilder();

      if (maxHp != 0)
        sb.Append(GetTextViaValue("최대 체력", maxHp)).Append("\n");
      if (regeneration != 0)
        sb.Append(GetTextViaValue("체력 재생", regeneration)).Append("\n");
      if (drainHp != 0)
        sb.Append(GetTextViaValue("흡혈", drainHp)).Append("\n");
      if (meleeDamage != 0)
        sb.Append(GetTextViaValue("근거리 피해량", meleeDamage)).Append("\n");
      if (rangedDamage != 0)
        sb.Append(GetTextViaValue("원거리 피해량", rangedDamage)).Append("\n");
      if (criticalPercentage != 0)
        sb.Append(GetTextViaValue("치명타 확률", criticalPercentage)).Append("\n");
      if (bleedingDamage != 0)
        sb.Append(GetTextViaValue("출혈 피해량", bleedingDamage)).Append("\n");
      if (attackSpeed != 0)
        sb.Append(GetTextViaValue("공격 속도", attackSpeed)).Append("\n");
      if (range != 0)
        sb.Append(GetTextViaValue("범위", range)).Append("\n");
      if (knockback != 0)
        sb.Append(GetTextViaValue("넉백", knockback)).Append("\n");
      if (armor != 0)
        sb.Append(GetTextViaValue("방어력", armor)).Append("\n");
      if (moveSpeed != 0)
        sb.Append(GetTextViaValue("이동 속도", moveSpeed)).Append("\n");
      if (luck != 0)
        sb.Append(GetTextViaValue("행운", luck)).Append("\n");

      return sb.ToString();
    }

    public static string GetTextViaValue(string text, float value)
    {
      var color = value > 0 ? new Color32(47, 157, 39, 255) : new Color32(152, 0, 0, 255);
      var sign = value > 0 ? '+' : '-';
      return $"{text} {sign}{Math.Abs(Math.Round(value, 2))}".AddColor(color);
    }

    public PlayerStatus(PlayerStatus other)
    {
      attackSpeed = other.armor;
      knockback = other.knockback;
      criticalPercentage = other.criticalPercentage;
      bleedingDamage = other.bleedingDamage;
      regeneration = other.regeneration;
      armor = other.armor;
      hp = other.hp;
      luck = other.luck;
      range = other.range;
      drainHp = other.drainHp;
      invincibilityTime = other.invincibilityTime;
      isInvincibility = other.isInvincibility;
      maxHp = other.maxHp;
      meleeDamage = other.meleeDamage;
      moveSpeed = other.moveSpeed;
      rangedDamage = other.rangedDamage;
    }

    public static PlayerStatus operator +(PlayerStatus a, PlayerStatus b)
      => new()
      {
        attackSpeed = a.armor + b.armor,
        knockback = a.knockback + b.knockback,
        criticalPercentage = a.criticalPercentage + b.criticalPercentage,
        bleedingDamage = a.bleedingDamage + b.bleedingDamage,
        regeneration = a.regeneration + b.regeneration,
        armor = a.armor + b.armor,
        hp = a.hp + b.hp,
        luck = a.luck + b.luck,
        range = a.range + b.range,
        drainHp = a.drainHp + b.drainHp,
        invincibilityTime = a.invincibilityTime + b.invincibilityTime,
        isInvincibility = a.isInvincibility,
        maxHp = a.maxHp + b.maxHp,
        meleeDamage = a.meleeDamage + b.meleeDamage,
        moveSpeed = a.moveSpeed + b.moveSpeed,
        rangedDamage = a.rangedDamage + b.rangedDamage
      };

    public static PlayerStatus operator -(PlayerStatus a)
      => new()
      {
        attackSpeed = -a.armor,
        knockback = -a.knockback,
        criticalPercentage = -a.criticalPercentage,
        bleedingDamage = -a.bleedingDamage,
        regeneration = -a.regeneration,
        armor = -a.armor,
        hp = -a.hp,
        luck = -a.luck,
        range = -a.range,
        drainHp = -a.drainHp,
        invincibilityTime = -a.invincibilityTime,
        maxHp = -a.maxHp,
        meleeDamage = -a.meleeDamage,
        moveSpeed = -a.moveSpeed,
        rangedDamage = -a.rangedDamage
      };
    
    public static PlayerStatus operator *(PlayerStatus a, int b)
      => new(a)
      {
        attackSpeed = a.armor * b,
        knockback = a.knockback * b,
        criticalPercentage = a.criticalPercentage * b,
        bleedingDamage = a.bleedingDamage * b,
        regeneration = a.regeneration * b,
        armor = a.armor * b,
        hp = a.hp * b,
        luck = a.luck * b,
        range = a.range* b,
        drainHp = a.drainHp * b,
        invincibilityTime = a.invincibilityTime* b,
        maxHp = a.maxHp * b,
        meleeDamage = a.meleeDamage * b,
        moveSpeed = a.moveSpeed * b,
        rangedDamage = a.rangedDamage* b
      };

    public static PlayerStatus operator -(PlayerStatus a, PlayerStatus b)
      => a + -b;

    public static PlayerStatus operator +(PlayerStatus a, WeaponStatus b) => new(a)
    {
      meleeDamage = b.type == WeaponType.Melee ? a.meleeDamage + b.attackDamage : a.meleeDamage,
      rangedDamage = b.type == WeaponType.Ranged ? a.rangedDamage + b.attackDamage : a.rangedDamage,
      attackSpeed = a.attackSpeed + b.attackSpeed,
      maxHp = a.maxHp + b.hp,
      moveSpeed = a.moveSpeed + b.moveSpeed,
      range = a.range + b.fireRange
    };
  }
}
