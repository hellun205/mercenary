using UnityEngine;
using UnityEngine.Serialization;

namespace Weapon.Ranged.Bomb
{
  [CreateAssetMenu(fileName = "Ranged Bomb Weapon", menuName = "Weapon/Ranged/Bomb", order = 0)]
  public class RangedBombWeaponData : WeaponData
  {
    [Header("Ranged - Bomb")]
    public string bullet;

    public string effectObj;
    
    public float explosionDelay;

    public float explosionRange;

    public bool fireOnContact;
    
    // public override float GetAttackDamage() => status.attackDamage + GameManager.Player.status.rangedDamage;
  }
}