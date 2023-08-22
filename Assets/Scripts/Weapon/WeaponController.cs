using System;
using JetBrains.Annotations;
using Manager;
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

    public Action onFire;

    private float time;

#if UNITY_EDITOR
    private void Reset()
    {
      collider = GetComponent<CircleCollider2D>();
      collider.radius = weaponData.status.range;
    }
    private void OnValidate()
    {
      collider.radius = weaponData.status.range;
    }
#endif

    private void Awake()
    {
    }

    private void Update()
    {
      if (hasTarget && target && target.canTarget)
      {
        transform.localRotation = Quaternion.Lerp
        (
          transform.rotation,
          transform.rotation.GetAngleOfLookAtObject(transform, target.transform),
          Time.deltaTime * 10f
        );

        if (time >= weaponData.status.attackDelay)
        {
          time = 0;
          onFire?.Invoke();
        }
      }

      time += Time.deltaTime;
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
      if (hasTarget && other.gameObject == target.gameObject)
      {
        hasTarget = false;
      }
    }
  }
}
