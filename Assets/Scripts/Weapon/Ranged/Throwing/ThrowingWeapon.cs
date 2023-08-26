using UnityEngine;

namespace Weapon.Ranged.Throwing
{
  [CreateAssetMenu(fileName = "Throwing Weapon", menuName = "Weapon/Ranged/Throwing", order = 0)]
  public class ThrowingWeapon : Weapon
  {
    [Header("Ranged - Throwing - Explosion")]
    public string throwingObj;

    public string effectObj;
    
    public float damageDelay;

    public float damageRange;

    public bool fireOnContact;
  }
}