using UnityEngine;
using Weapon;

namespace Enemy
{
  [CreateAssetMenu(fileName = "Enemy Status", menuName = "Status/Enemy", order = 0)]
  public class EnemyStatus : TargetableStatus
  {
    public float moveSpeed;
  }
}