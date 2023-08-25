using System;
using UnityEngine;

namespace Player
{
  [Serializable]
  public struct PlayerStatus
  {
    [Header("Hp")]
    public float maxHp;
    public float hp;
    public float regeneration;
    public float drainHp;
    
    [Header("Damage")]
    public float meleeDamage;
    public float rangedDamage;
    public float bleedingDamage;
    public float attackSpeed;
    
    
    public float moveSpeed;
    public float luck;
  }
}