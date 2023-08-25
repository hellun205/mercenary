using System;
using UnityEngine;
using UnityEngine.Serialization;
using Util;

namespace Weapon
{
  [RequireComponent(typeof(CircleCollider2D))]
  public class WeaponController<T> : MonoBehaviour where T : Weapon
  {
    public T weaponData;

    public TargetableObject target;
    public TargetableObject AttackTarget;

    public bool hasTarget = false;

    [FormerlySerializedAs("collider")]
    [SerializeField]
    private CircleCollider2D col;

    private float time;

    [SerializeField]
    protected SpriteRenderer sr;

    protected virtual void Reset()
    {
      col = GetComponent<CircleCollider2D>();
      col.isTrigger = true;
    }

    protected virtual void Awake()
    {
      RefreshRange();
    }

    [ContextMenu("Refresh Range")]
    private void RefreshRange()
    {
      col.radius = weaponData.status.fireRange;
    }

    private void Update()
    {
      RefreshRange();

      if (hasTarget && target && target.canTarget)
      {
        if (weaponData.rotate)
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

    protected void SetTarget(TargetableObject targetObj)
    {
      AttackTarget = targetObj;
    }

    protected void Attack()
    {
      try
      {
        if (AttackTarget.canTarget && AttackTarget.gameObject.activeSelf)
          AttackTarget.Hit(weaponData.status.attackDamage);
      }
      catch (Exception ex)
      {
        Debug.Log("Error Attack target:" + ex.Message);
      }
    }
  }
}