using UnityEngine;

namespace Weapon
{
  public abstract class Weapon : ScriptableObject
  {
    [Header("Weapon Desc")]
    public string _name;
    
    public Sprite icon;

    public string prefab;

    [Multiline]
    public string descriptions;

    public WeaponStatus status;
    
    [Header("Sprite Setting")]
    public bool needFlip;

    public bool rotate = true;
  }
}