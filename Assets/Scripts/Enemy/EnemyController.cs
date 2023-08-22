using UnityEngine;
using Weapon;

namespace Enemy
{
  [RequireComponent(typeof(TargetableObject))]
  public class EnemyController : MonoBehaviour
  {
    public bool canTarget { get; set; }

    public Vector3 position => transform.position;

    public GameObject go => gameObject;
  }
}
