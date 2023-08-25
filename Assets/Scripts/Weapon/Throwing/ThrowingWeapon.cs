using UnityEngine;

namespace Weapon.Throwing
{
  [CreateAssetMenu(fileName = "Throwing Weapon", menuName = "Weapon/Throwing", order = 0)]
  public class ThrowingWeapon : Weapon
  {
    [Header("Throwing")]
    public string throwingObj;
    
    public float damageDelay;

    public float damageRange;

    public bool fireOnContact;
  }
}