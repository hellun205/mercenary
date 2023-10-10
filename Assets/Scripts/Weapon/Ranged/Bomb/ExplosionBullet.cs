using System;
using Interact;
using Manager;
using UnityEngine;

namespace Weapon.Ranged.Bomb
{
  public class ExplosionBullet : Bullet
  {
    [Header("Bomb")]
    // [SerializeField]
    // private SpriteRenderer sr;
    [NonSerialized]
    public Vector2 targetPosition;

    public float explosionRange { get; set; }

    private CircleCollider2D col;

    [NonSerialized]
    public RangedBombWeapon mainCtrler;

    private bool isFired;

    protected override void Awake()
    {
      base.Awake();
      col = GetComponent<CircleCollider2D>();
      detectEnemy.onInteract += OnDetect;
    }

    protected override void OnDetect(Interacter obj)
    {
      if (!mainCtrler.weaponData.fireOnContact) return;
      OnDetect();
      Fire();
    }

    public override void OnSummon()
    {
      base.OnSummon();
      isFired = false;
      currentCondition = InteractCondition.Normal;
      // sr.color = sr.color.Setter(a: 1f);
      OnStart();
    }

    protected virtual void OnDetect()
    {
    }

    public virtual void OnStart()
    {
    }


    public void SetRange(float range)
    {
      explosionRange = range / 10;
      col.radius = explosionRange;
    }

    protected void Fire()
    {
      if (isFired) return;
      isFired = true;
      isEnabled = false;
      currentCondition = InteractCondition.Attack;

      GameManager.Pool.Summon<ExplosionEffectController>
      (
        mainCtrler.GetPObj(mainCtrler.weaponData.effectObj),
        transform.position,
        obj => obj.SetRange(explosionRange)
      );
      Kill();
    }
  }
}