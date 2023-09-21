using System.Collections;
using System.Collections.Generic;
using Enemy;
using Interact;
using Manager;
using Pool;
using Pool.Extensions;
using UnityEngine;
using Util;

namespace Weapon
{
  public class TargetableObject : InteractiveObject, IUsePool
  {
    public PoolObject poolObject { get; set; }

    public bool canTarget => !isDead;

    public float hp;

    public int index => poolObject.index;

    private SpriteRenderer sr;

    private bool isDead;

    private Coroutiner deadCrt;

    public bool playerAttacked;

    private Queue<float> bleedingQueue = new();

    public const float bleedingDelay = 0.7f;

    private MovableObject movableObject;

    [Header("Targetable Object")]
    public TargetableStatus status;

    [SerializeField]
    private Timer bleedingTimer = new();

    [SerializeField]
    private Timer knockBackTimer = new();

    private Vector2 knockBackStartPosition;
    private Vector2 knockBackEndPosition;

    private void Awake()
    {
      sr = GetComponent<SpriteRenderer>();
      movableObject = GetComponent<MovableObject>();
      deadCrt = new Coroutiner(DeadRoutine);
      bleedingTimer.duration = bleedingDelay;
      bleedingTimer.onEnd += OnBleedingTimerEnd;
      knockBackTimer.onTick += OnKnockBackTimerTick;
      knockBackTimer.onEnd += OnKnockBackTimerEnd;
      knockBackTimer.onBeforeStart += OnKnockBackTimerBeforeStart;
    }

    private void OnKnockBackTimerBeforeStart(Timer sender)
    {
      if (movableObject != null)
        movableObject.canMove = false;
    }

    private void OnKnockBackTimerEnd(Timer sender)
    {
      if (movableObject != null)
        movableObject.canMove = true;
    }

    private void OnKnockBackTimerTick(Timer sender)
    {
      transform.position = Vector2.Lerp(knockBackStartPosition, knockBackEndPosition, sender.value);
    }

    private void OnBleedingTimerEnd(Timer sender)
    {
      if (bleedingQueue.TryDequeue(out var amount))
        Damage(amount);
      CoroutineUtility.WaitUntil(() => bleedingQueue.Count > 0, () => sender.Start());
    }

    private IEnumerator DeadRoutine()
    {
      var time = 0f;
      while (!sr.color.a.ApproximatelyEqual(0f, 0.1f))
      {
        sr.color = sr.color.Setter(a: Mathf.Lerp(sr.color.a, 0f, time));
        time += Time.deltaTime;
        yield return new WaitForEndOfFrame();
      }

      poolObject.Release();
    }

    public void OnSummon()
    {
      hp = status.maxHp;
      detectCaster = InteractCaster.Player;
      isDead = false;
      playerAttacked = false;
      sr.color = Color.white;
      bleedingTimer.Start();
    }

    public void OnKilled()
    {
      deadCrt.Stop();
      bleedingTimer.Stop();
      bleedingQueue.Clear();
    }

    private void Update()
    {
      sr.color = Color.Lerp(sr.color, Color.white, Time.deltaTime * 5f);
    }

    public void Kill(bool throwCoin = false)
    {
      isDead = true;
      detectCaster = InteractCaster.Nothing;
      deadCrt.Start();
    }

    protected override void OnInteract(Interacter caster)
    {
      if (!caster.TryGetComponent<AttackableObject>(out var ao)) return;

      GameManager.Player.SuccessfulAttack();
      Damage(ao.isCritical ? ao.damage * ao.multipleDamage : ao.damage, ao.knockBack, ao.transform);
      if (ao.isCritical)
      {
        
        for (var i = 0; i < AttackableObject.bleedingCount; i++)
          bleedingQueue.Enqueue(ao.bleeding / AttackableObject.bleedingCount);
      }
    }

    private void Damage(float amount, float knockBack = 0f, Transform attacker = null)
    {
      hp -= amount;
      sr.color = Color.red;
      GameManager.Pool.Summon<Damage>("ui/damage", transform.GetAroundRandom(0.4f),
        obj => obj.value = Mathf.RoundToInt(amount));

      if (knockBack > 0 && attacker != null)
      {
        knockBackStartPosition = transform.position;
        knockBackEndPosition = 
          transform.position + (transform.position - attacker.position).normalized * (knockBack / 10);
        knockBackTimer.Start();
      }

      if (hp <= 0)
      {
        playerAttacked = true;
        Kill(true);
      }
    }
  }
}