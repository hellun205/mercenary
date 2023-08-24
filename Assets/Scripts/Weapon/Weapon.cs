using UnityEngine;

namespace Weapon
{
  public abstract class Weapon : ScriptableObject
  {
    public Sprite icon;

    public string prefab;

    [Multiline]
    public string descriptions;

    public WeaponStatus status;

    public bool needFlip;
  }
}