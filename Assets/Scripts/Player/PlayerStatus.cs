using System;
using System.Text;
using UnityEngine;
using Util;

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
    [Range(0f, 1f)]
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

    [Tooltip("공격 속도")]
    public float attackSpeed;

    [Tooltip("공격 범위(추가)")]
    public float range;

    [Header("Other")]
    [Tooltip("방어 %")]
    [Range(0f, 1f)]
    public float armor;

    [Tooltip("이동 속도")]
    public float moveSpeed;

    [Tooltip("행운")]
    [Range(0f, 1f)]
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
      if (invincibilityTime != 0)
        sb.Append(GetTextViaValue("무적 시간", invincibilityTime)).Append("\n");
      if (meleeDamage != 0)
        sb.Append(GetTextViaValue("근거리 피해량", meleeDamage)).Append("\n");
      if (rangedDamage != 0)
        sb.Append(GetTextViaValue("원거리 피해량", rangedDamage)).Append("\n");
      if (bleedingDamage != 0)
        sb.Append(GetTextViaValue("출혈 피해량", bleedingDamage)).Append("\n");
      if (attackSpeed != 0)
        sb.Append(GetTextViaValue("공격 속도", attackSpeed)).Append("\n");
      if (range != 0)
        sb.Append(GetTextViaValue("범위", range)).Append("\n");
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
  }
}
