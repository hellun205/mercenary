using System;
using UnityEngine;

namespace Player
{
  [Serializable]
  public struct PlayerStatus
  {
    [Header("Hp")]
    [Tooltip("최대 체력")]
    public float maxHp;
    
    [Tooltip("현재 체력")]
    public float hp;
    
    [Tooltip("초당 체력 재생")]
    public float regeneration;
    
    [Tooltip("피해 흡혈 (회복할 확률)")]
    public float drainHp;
    
    [Header("Attack")]
    [Tooltip("근거리 피해량")]
    public float meleeDamage;
    
    [Tooltip("원거리 피해량")]
    public float rangedDamage;
    
    [Tooltip("출혈 피해량")]
    public float bleedingDamage;
    
    [Tooltip("공격 속도")]
    public float attackSpeed;

    [Tooltip("공격 범위(추가)")]
    public float range;
    
    [Header("Other")]
    [Tooltip("방어")]
    public float armor;

    [Tooltip("이동 속도")]
    public float moveSpeed;

    [Tooltip("행운")]
    public float luck;
  }
}