using Interact;
using Manager;
using Pool;
using UnityEngine;
using Util;

namespace Weapon.Ranged.Throwing
{
  [RequireComponent(typeof(PoolObject), typeof(CircleCollider2D))]
  public class ThrowingObjectController : AttackableObject
  {
    private PoolObject po;
    private bool isEnabled;
    private Vector2 targetPos;
    private float time;
    private ThrowingWeaponController mainCtrler;
    private CircleCollider2D col;
    private SpriteRenderer sr;

    [SerializeField]
    private InteractiveObject detect;

    protected virtual void Awake()
    {
      po = GetComponent<PoolObject>();
      col = GetComponent<CircleCollider2D>();
      sr = GetComponent<SpriteRenderer>();

      po.onGet += PoolOnGet;
      po.onReleased += PoolOnRelease;
      detect.onInteract += OnDetect;
    }

    private void OnDetect(Interacter obj)
    {
      Debug.Log("Detect");
      if (mainCtrler.weaponData.fireOnContact)
        Fire();
    }

    private void PoolOnRelease()
    {
      isEnabled = false;
      // currentCondition = InteractCondition.Normal;
    }

    private void PoolOnGet()
    {
      currentCondition = InteractCondition.Normal;
      sr.color = sr.color.Setter(a: 1f);
      time = 0f;
    }

    private void Update()
    {
      if (!isEnabled) return;

      transform.position = Vector2.Lerp(transform.position, targetPos, Time.deltaTime * 3f);

      time += Time.deltaTime;

      if (time >= mainCtrler.weaponData.damageDelay)
        Fire();
    }

    public void SetTarget(Transform target, ThrowingWeaponController mainCtrl)
    {
      mainCtrler = mainCtrl;
      targetPos = target.position;
      col.radius = mainCtrler.weaponData.damageRange;
      isEnabled = true;
    }

    protected virtual void Fire()
    {
      currentCondition = InteractCondition.Attack;
      isEnabled = false;
      GameManager.Pool.Summon<ExplosionEffectController>
      (
        mainCtrler.GetPObj(mainCtrler.weaponData.effectObj),
        transform.position,
        obj => obj.SetRange(mainCtrler.weaponData.damageRange)
      );

      sr.color = sr.color.Setter(a: 0f);
      CoroutineUtility.Wait(0.1f, () => po.Release());
    }
  }
}
