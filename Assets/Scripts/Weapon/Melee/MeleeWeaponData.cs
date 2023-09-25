using Manager;
using UnityEngine;

namespace Weapon.Melee
{
  [CreateAssetMenu(fileName = "MeleeWeapon", menuName = "Weapon/Melee/Normal", order = 0)]
  public class MeleeWeaponData : WeaponData
  {
    // [Header("Melee - Normal")]
    
    // public override float GetAttackDamage() => status.attackDamage + GameManager.Player.status.meleeDamage;
  }
}