using System;
using Manager;
using Player;
using UnityEngine;
using Util;

namespace Weapon
{
  [RequireComponent(typeof(CircleCollider2D))]
  public class WeaponController : MonoBehaviour, ITier
  {
    public WeaponData weaponData;

    [NonSerialized]
    public WeaponStatus status;

    [Header("Target")]
    public TargetableObject target;

    public bool hasTarget = false;

    [NonSerialized]
    public bool isAttacking;

    [Header("Weapon Control")]
    [SerializeField]
    protected SpriteRenderer sr;

    [SerializeField]
    private CircleCollider2D col;

    private float time;
    private float readyTime;

    [NonSerialized]
    public bool isReady;

    public string GetPObj(string objName) => $"weapon/{name}/{objName}";

    public int tier { get; set; }
    public string dataName => $"{name}.{tier}";

    protected virtual void Reset()
    {
      col = GetComponent<CircleCollider2D>();
      col.isTrigger = true;
    }

    protected virtual void Awake()
    {
      GameManager.Wave.onWaveStart += RefreshStatus;
    }

    private void Start()
    {
      RefreshStatus();
    }

    private void RefreshStatus()
    {
      while (weaponData == null)
      {
        try
        {
          weaponData = GameManager.WeaponData[name];
        }
        catch (Exception ex)
        {
          Debug.Log(ex);
        }
      }

      status = weaponData.status[tier] + GameManager.Player.GetStatus();
    }

    [ContextMenu("Refresh Range")]
    private void RefreshRange()
    {
      RefreshStatus();
      col.radius = status.fireRange / 10;
    }

    protected virtual void Update()
    {
      if (!GameManager.Wave.state) return;
      if (!isReady)
      {
        readyTime += Time.deltaTime;
        if (readyTime >= 2f) isReady = true;
        return;
      }

      RefreshRange();
      if (hasTarget && target && target.canTarget)
      {
        var r = transform.GetRotationOfLookAtObject(target.transform);
        if (weaponData.rotate && !isAttacking)
        {
          transform.localRotation = Quaternion.Lerp(transform.rotation, r, Time.deltaTime * 20f);
        }

        if (time >= status.attackSpeed && (
              !weaponData.rotate ||
              transform.rotation.eulerAngles.z.ApproximatelyEqual(r.eulerAngles.z, 10f)))
        {
          time = 0;
          OnFire();
        }

        time += Time.deltaTime;
      }
      else
      {
        hasTarget = false;
        // time = 0;
      }

      if (weaponData.needFlipY)
        transform.localScale =
          transform.localScale.Setter(y: (transform.rotation.eulerAngles.z is < 90 and > -90 or >= 270) ? -1 : 1);
      if (weaponData.needFlipX)
        sr.flipX = (transform.rotation.eulerAngles.z is < 90 and > -90 or >= 270);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
      if (!hasTarget && other.TryGetComponent(typeof(TargetableObject), out var component))
      {
        var targetable = component as TargetableObject;
        target = targetable;
        hasTarget = true;
      }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
      if (hasTarget && other.TryGetComponent<TargetableObject>(out var to) && to.index == target.index)
      {
        target = null;
        hasTarget = false;
      }
    }

    protected virtual void OnFire()
    {
    }

    protected void ApplyDamage(AttackableObject ao)
    {
      ao.damage = status.attackDamage;
      ao.multipleDamage = status.multipleCritical;
      ao.isCritical = status.criticalPercent.ApplyPercentage();
      ao.bleeding = status.bleedingDamage;
      ao.knockBack = status.knockback;
    }
  }
}
