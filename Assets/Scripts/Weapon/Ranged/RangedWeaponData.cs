using Manager;
using UnityEngine;

namespace Weapon.Ranged
{
  [CreateAssetMenu(fileName = "Ranged Weapon", menuName = "Weapon/Ranged/Normal", order = 0)]
  public class RangedWeaponData : WeaponData
  {
    [Header("Ranged - Normal")]
    public int penetrate;

    public string bullet;

    public float errorRange;

    public int bulletCount;

  }
}