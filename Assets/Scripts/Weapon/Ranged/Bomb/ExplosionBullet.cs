using System;
using Interact;
using Manager;
using UnityEngine;
using Util;
using Weapon.Ranged.Throwing;

namespace Weapon.Ranged.Bomb
{
  public class ExplosionBullet : Bullet
  {
    [Header("Bomb")]
    [SerializeField]
    private SpriteRenderer sr;

    [NonSerialized]
    public Vector2 targetPosition;

    [NonSerialized]
    public float explosionRange;

    private CircleCollider2D col;

    protected override bool isKillOnInteract => false;

    [NonSerialized]
    public RangedBombWeapon mainCtrler;

    [SerializeField]
    protected InteractiveObject detectEnemy;

    protected override void Awake()
    {
      base.Awake();
      col = GetComponent<CircleCollider2D>();
      detectEnemy.onInteract += OnDetect;
    }

    private void OnDetect(Interacter obj)
    {
      if (!mainCtrler.weaponData.fireOnContact) return;
      OnDetect();
      Fire();
    }

    public override void OnSummon()
    {
      base.OnSummon();
      sr.color = sr.color.Setter(a: 1f);
    }

    protected virtual void OnDetect()
    {
    }

    public virtual void OnStart()
    {
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
    }

    protected void Fire()
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