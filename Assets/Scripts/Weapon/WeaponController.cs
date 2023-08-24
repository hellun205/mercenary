using System;
using JetBrains.Annotations;
using Manager;
using Pool;
using UnityEngine;
using Util;

namespace Weapon
{
  [RequireComponent(typeof(CircleCollider2D))]
  public class WeaponController : MonoBehaviour
  {
    public Weapon weaponData;

    public TargetableObject target;

    public bool hasTarget = false;

    [SerializeField]
    private CircleCollider2D collider;

    private float time;

    [SerializeField]
    private SpriteRenderer sr;

    private void Reset()
    {
      collider = GetComponent<CircleCollider2D>();
      collider.isTrigger = true;
    }

    private void Awake()
    {
      RefreshRange();
    }

    [ContextMenu("Refresh Range")]
    private void RefreshRange()
    {
      collider.radius = weaponData.status.range;
    }

    private void Update()
    {
      RefreshRange();

      if (hasTarget && target && target.canTarget)
      {
        transform.localRotation = Quaternion.Lerp
        (
          transform.rotation,
          transform.GetRotationOfLookAtObject(target.transform),
          Time.deltaTime * 10f
        );

        if (time >= 1 / weaponData.status.attackSpeed)
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

      if (weaponData.needFlip)
        sr.flipY = (transform.rotation.eulerAngles.z is < 90 and > -90 or >= 270);
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
          ((TargetableObject)component).index == target.index)
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