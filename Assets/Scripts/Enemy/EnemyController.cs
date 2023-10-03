using Data;
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
  public class EnemyController : Interacter, IUsePool, IAttacker
  {
    public PoolObject poolObject { get; set; }

    public EnemyStatus status { get; private set; }

    // private bool isEnabled = true;

    private Transform target;

    private TargetableObject to;
    private MovableObject movableObject;
    private RangedAttacker rangedAttacker;

    public float damage => status.damage;

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
      rangedAttacker = GetComponent<RangedAttacker>();
      to.onDead += OnDead;
      attackCooldownTimer.onStart += _ => currentCondition = InteractCondition.Normal;
      attackCooldownTimer.onEnd += _ =>
      {
        currentCondition = InteractCondition.Attack;
        RemoveDetection();
      };

      if (movableObject != null)
      {
        movableObject.moveSpeedGetter = () => status.moveSpeed;
        movableObject.increaseAmountGetter = () => status.increaseMoveSpeedPerSecond;
        movableObject.maxMoveSpeedGetter = () => status.maxMoveSpeed;
      }

      if (rangedAttacker != null)
      {
        rangedAttacker.bulletSpeedGetter = () => status.bulletSpeed;
        rangedAttacker.fireDurationGetter = () => status.attackSpeed;
        rangedAttacker.damageGetter = () => status.damage;
        rangedAttacker.detectRangeGetter = () => status.fireRange;
      }

      currentCondition = rangedAttacker == null ? InteractCondition.Attack : InteractCondition.Normal;
    }

    private void OnDead()
    {
      if (to.playerAttacked)
        ThrowCoin();
    }

    public void OnSummon()
    {
      status = GameManager.Data.data.GetEnemyStatus(poolObject.originalName, GameManager.Wave.currentWave);
      movableObject.canMove = true;

      if (rangedAttacker != null)
      {
        rangedAttacker.Ready();
      }
    }

    public void OnKilled()
    {
      movableObject.canMove = false;
    }

    private void ThrowCoin()
    {
      var count = status.drop;
      if (GameManager.Player.status.luck.ApplyPercentage())
        count *= 2;

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
      if (rangedAttacker != null) return;
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
