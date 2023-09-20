using Manager;
using UnityEngine;

namespace Weapon.Ranged.Throwing
{
  [CreateAssetMenu(fileName = "Throwing Weapon", menuName = "Weapon/Ranged/Throwing", order = 0)]
  public class ThrowingWeaponData : WeaponData
  {
    [Header("Ranged - Throwing - Explosion")]
    public string throwingObj;

    public string effectObj;
    
    public float damageDelay;

    public float damageRange;

    public bool fireOnContact;
    
    // public override float GetAttackDamage() => status.attackDamage + GameManager.Player.status.rangedDamage;
  }
}