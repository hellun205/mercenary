using System;

namespace Weapon
{
  [Serializable]
  public struct WeaponStatus
  {
    public float attackDamage;
    public float hp;
    // public float attackSpeed;
    public float moveSpeed;
    public float range;
    public float attackDelay;
  }
}