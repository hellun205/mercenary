using System;
using JetBrains.Annotations;
using Manager;
using UnityEngine;

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
      if (hasTarget && target.canTarget)
      {
        var offset = target.transform.position - transform.position;
        var rotation = Quaternion.Euler(0, 0,
          Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg);
        transform.localRotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 5f);
        Debug.Log(Mathf.Atan2(target.transform.position.y, target.transform.position.x) * Mathf.Rad2Deg);
      }
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
