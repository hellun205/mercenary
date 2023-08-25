using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Util;

namespace Weapon
{
  [RequireComponent(typeof(CircleCollider2D))]
  public abstract class WeaponController<T> : MonoBehaviour where T : Weapon
  {
    public T weaponData;

    public TargetableObject target;

    public bool hasTarget = false;


    [FormerlySerializedAs("collider")]
    [SerializeField]
    private CircleCollider2D col;

    private float time;
    
    public bool isAttacking;

    [SerializeField]
    protected SpriteRenderer sr;
    
    [SerializeField]
    private List<int> attackedTargets = new List<int>();

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

    protected virtual void Update()
    {
      RefreshRange();

      if (hasTarget && target && target.canTarget)
      {
        if (weaponData.rotate && !isAttacking)
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
          ((TargetableObject)component).index == target.index)
      {
        target = null;
        hasTarget = false;
      }
    }

    protected virtual void OnFire()
    {
      attackedTargets.Clear();
    }
    
    public void Attack(TargetableObject targetObj)
    {
      if (!isAttacking && attackedTargets.Contains(targetObj.index)) return;
      attackedTargets.Add(targetObj.index);
      try
      {
        if (targetObj.canTarget && targetObj.gameObject.activeSelf)
          targetObj.Hit(weaponData.status.attackDamage);
      }
      catch (Exception ex)
      {
        Debug.Log("Error Attack target:" + ex.Message);
      }
    }
  }
}