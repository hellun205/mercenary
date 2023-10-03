using System;
using UnityEngine;

namespace Enemy
{
  [Serializable]
  public struct EnemyStatus
  {
    [Header("Base")]
    public float maxHp;
    
    public float damage;
    
    public float moveSpeed;
    
    public int drop;

    [Header("Detail")]
    public float fireRange;

    public float bulletSpeed;

    public float attackSpeed;

    public float increaseMoveSpeedPerSecond;

    public float maxMoveSpeed;

  }
}
