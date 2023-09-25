using System;
using UnityEngine;

namespace Weapon
{
  [CreateAssetMenu(fileName = "Weapon Bundle", menuName = "Weapon Bundle", order = 0)]
  public class WeaponBundle : ScriptableObject
  {
    public WeaponData[] weapons = Array.Empty<WeaponData>();

    public WeaponData this[int tier] => weapons[tier];
  }
}
