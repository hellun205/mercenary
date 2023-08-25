using UnityEngine;

namespace Weapon.Ranged
{
  [CreateAssetMenu(fileName = "Ranged Weapon", menuName = "Weapon/Ranged/Normal", order = 0)]
  public class RangedWeapon : Weapon
  {
    [Header("Ranged - Normal")]
    public int penetrate;
  }
}