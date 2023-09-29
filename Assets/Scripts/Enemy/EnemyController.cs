using Interact;
using Manager;
using Player;
using Pool;
using Spawn;
using UnityEngine;
using Util;
using Weapon;

namespace Enemy
{
  [RequireComponent(typeof(TargetableObject))]
  public class EnemyController : Interacter, IUsePool
  {
    public PoolObject poolObject { get; set; }

    public SpawnData.Enemy.Status status { get; private set; }

    // private bool isEnabled = true;

    private Transform target;

    private TargetableObject to;
    private MovableObject movableObject;

    [Header("Enemy Controller")]
    [SerializeField]
    private Timer attackCooldownTimer = new();

    private void Reset()
    {
      caster = InteractCaster.Others;
    }

    private void Awake()
    {
      to = GetComponent<TargetableObject>();
      movableObject = GetComponent<MovableObject>();
      attackCooldownTimer.onStart += _ => currentCondition = InteractCondition.Normal;
      attackCooldownTimer.onEnd += _ =>
      {
        currentCondition = InteractCondition.Attack;
        RemoveDetection();
      };
      movableObject.moveSpeed = () => status.moveSpeed;
      to.onDead += OnDead;
    }

    private void OnDead()
    {
      if (to.playerAttacked)
        ThrowCoin();
    }

    public void OnSummon()
    {
      status = GameManager.Manager.spawn.GetEnemyStatus(name, GameManager.Wave.currentWave);
      movableObject.canMove = true;
    }

    public void OnKilled()
    {
      movableObject.canMove = false;
    }

    private void ThrowCoin()
    {
      var count = 1;
      if (GameManager.Player.status.luck.ApplyPercentage())
        count++;

      for (var i = 0; i < count; i++)
        GameManager.Pool.Summon("object/coin", transform.position);
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
