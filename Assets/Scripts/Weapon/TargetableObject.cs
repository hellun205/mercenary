using Manager;
using Pool;
using UnityEngine;

namespace Weapon
{
  public class TargetableObject : MonoBehaviour
  {
    public TargetableStatus status;
    
    public bool canTarget;

    public float hp;

    public int index => po.index;

    private PoolObject po;

    private void Awake()
    {
      po = GetComponent<PoolObject>();
      po.onGet += () =>
      {
        hp = status.maxHp;
        canTarget = true;
      };
      po.onReleased += () => canTarget = false;
    }

    public void Hit(float damage)
    {
      hp -= damage;

      if (hp <= 0)
      {
        canTarget = false;
        GameManager.Spawn.Spawn(transform.position, "coin");
        po.Release();
      }
    }
  }
}