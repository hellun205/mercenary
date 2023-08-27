using System;
using Manager;
using UnityEngine;
using Util;

namespace Weapon
{
  [RequireComponent(typeof(CircleCollider2D))]
  public abstract class WeaponController<T> : MonoBehaviour where T : Weapon
  {
    public T weaponData => (T)GameManager.WeaponData[name];

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
    
    protected virtual void Reset()
    {
      col = GetComponent<CircleCollider2D>();
      col.isTrigger = true;
    }

    protected virtual void Awake()
    {
      // RefreshRange();
    }

    [ContextMenu("Refresh Range")]
    private void RefreshRange()
    {
      col.radius = weaponData.GetRange();
    }

    protected virtual void Update()
    {
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

        if (time >= 1 / weaponData.GetAttackSpeed() && (
              !weaponData.rotate ||
              transform.rotation.eulerAngles.z.ApproximatelyEqual(r.eulerAngles.z, 10f) ))
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
        sr.flipY = (transform.rotation.eulerAngles.z is < 90 and > -90 or >= 270);
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
      if (hasTarget &&
          other.TryGetComponent(typeof(TargetableObject), out var component) &&
          ((TargetableObject) component).index == target.index)
      {
        target = null;
        hasTarget = false;
      }
    }

    protected virtual void OnFire()
    {
    }

  }
}
