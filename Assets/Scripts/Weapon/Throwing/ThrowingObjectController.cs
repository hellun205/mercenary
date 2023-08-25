using System;
using Manager;
using Pool;
using UnityEngine;

namespace Weapon.Throwing
{
  [RequireComponent(typeof(PoolObject))]
  public class ThrowingObjectController : MonoBehaviour
  {
    [SerializeField]
    private ThrowingWeapon weaponData;

    [SerializeField]
    private string explosionEffectObj;

    private PoolObject po;
    private bool isEnabled;
    public Transform target;
    private Vector2 targetPos;
    private float time;

    protected virtual void Awake()
    {
      po = GetComponent<PoolObject>();

      po.onGet += PoolOnGet;
      po.onReleased += PoolOnRelease;
    }

    private void PoolOnRelease()
    {
      isEnabled = false;
    }

    private void PoolOnGet()
    {
      time = 0f;
    }

    private void Update()
    {
      if (!isEnabled) return;

      transform.position = Vector2.Lerp(transform.position, targetPos, Time.deltaTime * 3f);

      time += Time.deltaTime;

      if (time >= weaponData.damageDelay)
        Fire();
    }

    public void SetTarget(Transform target)
    {
      this.target = target;
      targetPos = target.position;
      isEnabled = true;
    }

    protected virtual void Fire()
    {
      isEnabled = false;
      GameManager.Pool.Summon<ExplosionEffectController>(explosionEffectObj, transform.position,
        obj => obj.SetRange(weaponData.damageRange));

      var enemies = Physics2D.OverlapCircleAll(transform.position, weaponData.damageRange, LayerMask.GetMask("Enemy"));
      foreach (var enemy in enemies)
      {
        enemy.GetComponent<TargetableObject>().Hit(weaponData.status.attackDamage);
      }

      po.Release();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (weaponData.fireOnContact)
        Fire();
    }
  }
}