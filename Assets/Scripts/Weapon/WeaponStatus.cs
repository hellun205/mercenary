using System;
using Player;

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
        multipleCritical = a.multipleCritical * b.criticalPercent,
        knockback = a.knockback + b.knockback,
        bleedingDamage = a.bleedingDamage + b.bleedingDamage,
        type = a.type
      };
  }
}
