using Manager;
using UnityEngine;
using UnityEngine.Serialization;

namespace Weapon
{
  public abstract class Weapon : ScriptableObject
  {
    [Header("Weapon Desc")]
    public string _name;
    
    public Sprite icon;

    [Multiline]
    public string descriptions;

    public WeaponStatus status;
    
    [FormerlySerializedAs("needFlip")]
    [Header("Sprite Setting")]
    public bool needFlipY;

    public bool needFlipX;

    public bool rotate = true;
    
    public virtual float GetAttackDamage() => status.attackDamage;
    public float GetAttackSpeed() => status.attackSpeed + GameManager.Player.status.attackSpeed;

    public float GetRange() => status.fireRange + GameManager.Player.status.range;
  }
}