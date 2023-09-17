using Interact;
using Manager;
using Pool;
using UnityEngine;
using Util;
using Weapon;

namespace Enemy
{
  [RequireComponent(typeof(TargetableObject))]
  public class EnemyController : Interacter, IUsePool
  {
    public PoolObject poolObject { get; set; }

    [Header("Base Status")]
    public EnemyStatus status;

    private bool isEnabled = true;

    private Transform target;

    private TargetableObject to;

    private void Reset()
    {
      caster = InteractCaster.Others;
    }

    private void Awake()
    {
      to = GetComponent<TargetableObject>();
    }

    public void OnSummon()
    {
      isEnabled = true;
    }

    public void OnKilled()
    {
      if (to.playerAttacked)
        ThrowCoin();

      isEnabled = false;
    }

    private void ThrowCoin()
    {
      var count = 1;
      if (GameManager.Player.status.luck.ApplyPercentage())
        count++;

      for (var i = 0; i < count; i++)
        GameManager.Spawn.Spawn(transform.position, "object/coin");
    }

    private void Start()
    {
      target = GameManager.Player.transform;
    }

    private void Update()
    {
      if (!isEnabled || !to.canTarget) return;

      transform.rotation = transform.GetRotationOfLookAtObject(target.transform);
      transform.Translate(Vector3.right * (Time.deltaTime * status.moveSpeed));
    }

    private void OnTriggerStay2D(Collider2D other)
    {
      if (!other.CompareTag("Player") || !to.canTarget) return;

      GameManager.Player.Hit(status.damage, poolObject.index);
    }
  }
}
