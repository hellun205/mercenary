using System;
using UnityEngine.Serialization;

namespace Weapon
{
  [Serializable]
  public class WeaponStatus
  {
    public float attackDamage;
    public float hp;
    public float attackSpeed;
    public float moveSpeed;
    public float fireRange;
  }
}