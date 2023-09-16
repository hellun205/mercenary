using System;

namespace Weapon
{
  [Serializable]
  public class WeaponStatus
  {
    public WeaponType type;
    public float attackDamage;
    public float hp;
    public float attackSpeed;
    public float moveSpeed;
    public float fireRange;
  }
}