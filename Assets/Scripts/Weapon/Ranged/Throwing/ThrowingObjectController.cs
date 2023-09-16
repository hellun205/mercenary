using Manager;
using Pool;
using Store.Status;
using UnityEngine;

namespace Weapon.Ranged.Throwing
{
  [RequireComponent(typeof(PoolObject))]
  public class ThrowingObjectController : MonoBehaviour
  {
    public ThrowingWeapon weaponData;

    private PoolObject po;
    private bool isEnabled;
    public Transform target;
    private Vector2 targetPos;
    private float time;
    private ThrowingWeaponController mainCtrler;

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

    public void SetTarget(Transform target, ThrowingWeaponController mainCtrl)
    {
      this.target = target;
      mainCtrler = mainCtrl;
      targetPos = target.position;
      isEnabled = true;
    }

    protected virtual void Fire()
    {
      isEnabled = false;
      GameManager.Pool.Summon<ExplosionEffectController>(mainCtrler.GetPObj(weaponData.effectObj), transform.position,
        obj => obj.SetRange(weaponData.damageRange));

      var enemies = Physics2D.OverlapCircleAll(transform.position, weaponData.damageRange, LayerMask.GetMask("Enemy"));
      foreach (var enemy in enemies)
      {
        enemy.GetComponent<TargetableObject>().Hit(mainCtrler.status.rangedDamage);
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