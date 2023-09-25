using System;
using System.Text;
using Player;
using UnityEngine;
using Util.Text;

namespace Weapon
{
  [Serializable]
  public class WeaponStatus
  {
    public WeaponType type;
    public float attackDamage;
    public float attackSpeed;
    public float fireRange;
    public float multipleCritical = 1;
    public float knockback;
    public float bleedingDamage;

    [NonSerialized]
    public float criticalPercent;

    public static WeaponStatus operator +(WeaponStatus a, PlayerStatus b)
      => new()
      {
        attackSpeed = a.attackSpeed * (b.attackSpeed + 1),
        attackDamage = a.attackDamage + a.type switch
        {
          WeaponType.Melee  => b.meleeDamage,
          WeaponType.Ranged => b.rangedDamage,
          _                 => 0
        },
        fireRange = a.fireRange + a.type switch
        {
          WeaponType.Melee  => b.range / 2,
          WeaponType.Ranged => b.range,
          _                 => 0,
        },
        criticalPercent = a.multipleCritical * b.criticalPercent,
        multipleCritical = a.multipleCritical,
        knockback = a.knockback + b.knockback,
        bleedingDamage = a.bleedingDamage > 0 ? a.bleedingDamage + b.bleedingDamage : 0,
        type = a.type
      };

    public string GetDescription()
    {
      var sb = new StringBuilder();

      if (attackDamage != 0)
        sb.Append(PlayerStatus.GetTextViaValue("공격력: ", attackDamage, plus: "")).Append("\n");
      if (multipleCritical != 0)
        sb.Append(PlayerStatus.GetTextViaValue("치명타 대미지: ", multipleCritical, plus: "x")).Append("\n");
      if (bleedingDamage != 0)
        sb.Append(PlayerStatus.GetTextViaValue("출혈 대미지: ", bleedingDamage, plus: "")).Append("\n");
      if (attackSpeed != 0)
        sb.Append(PlayerStatus.GetTextViaValue("공격 속도: ", attackSpeed, plus: "")).Append("\n");
      if (knockback != 0)
        sb.Append(PlayerStatus.GetTextViaValue("넉백: ", knockback, plus: "")).Append("\n");
      if (fireRange != 0)
        sb.Append(PlayerStatus.GetTextViaValue("범위: ", fireRange, plus: "")).Append("\n");

      return sb.ToString();
    }
  }
}
