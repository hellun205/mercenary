using UnityEngine;
using Weapon;

namespace Enemy
{
  [CreateAssetMenu(fileName = "Enemy Status", menuName = "Status/Attackable/Enemy", order = 0)]
  public class EnemyStatus : TargetableStatus
  {
    [Tooltip("공격력")]
    public float damage;
    
    [Tooltip("이동 속도")]
    public float moveSpeed;
  }
}