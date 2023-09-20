using System;
using Interact;
using Manager;
using UnityEngine;
using Util;
using Weapon.Ranged.Throwing;

namespace Weapon.Ranged.Bomb
{
  public class BombBullet : Bullet
  {
    [Header("Bomb")]
    [SerializeField]
    private SpriteRenderer sr;

    public Vector2 targetPosition;

    public float explosionRange;

    public Timer explosionTimer = new();

    private CircleCollider2D col;

    protected override bool isKillOnInteract => false;

    [NonSerialized]
    public RangedBombWeapon mainCtrler;

    [SerializeField]
    private Timer moveTimer = new();

    private Vector2 startPosition;

    [SerializeField]
    private InteractiveObject detectEnemy;

    protected override void Awake()
    {
      base.Awake();
      col = GetComponent<CircleCollider2D>();
      explosionTimer.onEnd += OnExplosionTimerEnd;
      moveTimer.onBeforeStart += OnMoveTimerStart;
      moveTimer.onTick += OnMoveTimerTick;
      detectEnemy.onInteract += OnDetect;
    }

    private void OnDetect(Interacter obj)
    {
      if (!mainCtrler.weaponData.fireOnContact) return;
      
      moveTimer.Stop();
      explosionTimer.Stop();
      Fire();
    }

    private void OnMoveTimerStart(Timer sender)
    {
      startPosition = transform.position;
    }

    private void OnMoveTimerTick(Timer sender)
    {
      transform.position = Vector2.Lerp(startPosition, targetPosition, sender.value);
    }

    private void OnExplosionTimerEnd(Timer sender)
    {
      Fire();
    }

    protected override void Move()
    {
    }

    public override void OnSummon()
    {
      base.OnSummon();
      sr.color = sr.color.Setter(a: 1f);
    }

    public override void OnKilled()
    {
      base.OnKilled();
      currentCondition = InteractCondition.Normal;
    }

    public void SetRange(float range)
    {
      explosionRange = range / 10;
      col.radius = explosionRange;
      explosionTimer.Start();
      moveTimer.Start();
    }

    private void Fire()
    {
      currentCondition = InteractCondition.Attack;
      sr.color = sr.color.Setter(a: 0f);

      GameManager.Pool.Summon<ExplosionEffectController>
      (
        mainCtrler.GetPObj(mainCtrler.weaponData.effectObj),
        transform.position,
        obj => obj.SetRange(explosionRange)
      );
      CoroutineUtility.Wait(0.2f, () => poolObject.Release());
    }
  }
}