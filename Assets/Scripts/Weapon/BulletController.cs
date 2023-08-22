using System;
using UnityEngine;
using Util;

namespace Weapon
{
  public class BulletController : MonoBehaviour
  {
    public Transform target;
    
    public float damage;

    public float speed = 10f;

    public bool isEnabled = false;

    public float despawnTime = 10f;
    
    private float time;

    private void Update()
    {
      if (!isEnabled) return;
      
      transform.Translate(Vector3.right * (Time.deltaTime * speed));

      time += Time.deltaTime;
      if (time >= despawnTime)
        Destroy(gameObject);
    }

    public void SetTarget(TargetableObject targetableObject, float damage)
    {
      target = targetableObject.transform;
      this.damage = damage;
      transform.rotation = transform.rotation.GetAngleOfLookAtObject(transform, target);
      isEnabled = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
      if (col.TryGetComponent(typeof(TargetableObject), out var component))
      {
        var targetable = component as TargetableObject;
        
        targetable.Hit(damage);
        Destroy(gameObject);
      }
    }
  }
}
