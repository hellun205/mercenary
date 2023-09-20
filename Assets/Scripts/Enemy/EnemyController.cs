using Interact;
using Manager;
using Player;
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

    [Header("Enemy Controller")]
    public EnemyStatus status;

    // private bool isEnabled = true;

    private Transform target;

    private TargetableObject to;
    private MovableObject movableObject;

    [SerializeField]
    private Timer attackCooldownTimer = new ();

    private void Reset()
    {
      caster = InteractCaster.Others;
    }

    private void Awake()
    {
      to = GetComponent<TargetableObject>();
      movableObject = GetComponent<MovableObject>();
      attackCooldownTimer.onStart += _ => currentCondition = InteractCondition.Normal;
      attackCooldownTimer.onEnd += _ => currentCondition = InteractCondition.Attack;
      movableObject.moveSpeed = () => status.moveSpeed;
    }

    public void OnSummon()
    {
      movableObject.canMove = true;
    }

    public void OnKilled()
    {
      if (to.playerAttacked)
        ThrowCoin();

      movableObject.canMove = false;
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


    protected override void OnInteract(InteractiveObject target)
    {
      base.OnInteract(target);
      if (target.TryGetComponent<PlayerController>(out var pc))
      {
        attackCooldownTimer.Start();
      }
    }

    // private void OnTriggerStay2D(Collider2D other)
    // {
    //   if (!other.CompareTag("Player") || !to.canTarget) return;
    //
    //   GameManager.Player.Hit(status.damage, poolObject.index);
    // }
  }
}
