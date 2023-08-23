using Manager;
using UnityEngine;

namespace Weapon
{
  public class TargetableObject : MonoBehaviour
  {
    public bool canTarget;

    public float hp = 50f;

    public void Hit(float damage)
    {
      hp -= damage;

      if (hp <= 0)
      {
        canTarget = false;
        GameManager.Spawn.Spawn(transform.position, "coin");
        Destroy(gameObject);
      }
    }
  }
}
