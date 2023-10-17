using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Enemy;
using Interact;
using Manager;
using Pool;
using Pool.Extensions;
using Sound;
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

    [SerializeField]
    private SpriteRenderer sr;

    private bool isDead;

    public bool playerAttacked;

    private Queue<float> bleedingQueue = new();

    public const float bleedingDelay = 0.7f;

    private MovableObject movableObject;

    public virtual float maxHp =>
      GameManager.Data.data.GetEnemyStatus(poolObject.originalName, GameManager.Wave.currentWave).maxHp;

    [Header("Targetable Object"), SerializeField]
    private Timer bleedingTimer = new();

    [SerializeField]
    private Timer knockBackTimer = new();

    private Vector2 knockBackStartPosition;
    private Vector2 knockBackEndPosition;

    public event Action onDead;

    private TweenerCore<Color, Color, ColorOptions> deadTweener;

    private void Awake()
    {
      movableObject = GetComponent<MovableObject>();
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

    public void OnSummon()
    {
      deadTweener.Kill();
      hp = maxHp;
      detectCaster = InteractCaster.Player;
      isDead = false;
      playerAttacked = false;
      sr.color = Color.white;
      bleedingTimer.Start();
    }

    public void OnKilled()
    {
      bleedingTimer.Stop();
      bleedingQueue.Clear();
      knockBackTimer.Stop();
      OnSummon();
    }

    private void Start()
    {
      OnSummon();
    }

    private void Update()
    {
      if (isDead) return;
      sr.color = Color.Lerp(sr.color, Color.white, Time.deltaTime * 5f);
    }
    
    public void Kill(bool? throwCoin = null)
    {
      if (throwCoin.HasValue)
        playerAttacked = throwCoin.Value;
      
      isDead = true;
      onDead?.Invoke();
      detectCaster = InteractCaster.Nothing;
      deadTweener = sr.DOFade(0f, 0.3f).OnComplete(poolObject.Release);
    }
    
    protected override void OnInteract(Interacter caster)
    {
      if (!caster.TryGetComponent<AttackableObject>(out var ao) || !ao.canAttack) return;
      
      GameManager.Player.SuccessfulAttack();
      Damage(ao.isCritical ? ao.damage * ao.multipleDamage : ao.damage, ao.knockBack, ao.transform);
      if (ao.isCritical)
      {
        for (var i = 0; i < AttackableObject.bleedingCount; i++)
          bleedingQueue.Enqueue(ao.bleeding / AttackableObject.bleedingCount);
      }
    }

    public void Damage(float amount, float knockBack = 0f, Transform attacker = null)
    {
      hp -= amount;
      sr.color = Color.red;
      GameManager.Pool.Summon("effect/blood", transform.position);
      GameManager.Pool.Summon<Damage>("ui/damage", transform.GetAroundRandom(0.4f),
        obj => obj.value = Mathf.RoundToInt(amount));
      GameManager.Sound.Play(SoundType.SFX_Normal, "sfx/normal/enemyhit");

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
