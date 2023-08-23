using System;
using Enemy;
using UnityEngine;

namespace Weapon
{
  public class MeleeWeaponCollider : MonoBehaviour
  {
    public bool isAttacking = false;
    public float damage;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
      if (isAttacking)
      {
        other.GetComponent<TargetableObject>().Hit(damage);
      }
    }
  }
}