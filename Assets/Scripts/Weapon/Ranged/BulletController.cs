using System;
using Interact;
using Pool;
using UnityEngine;
using Util;

namespace Weapon.Ranged
{
  [RequireComponent(typeof(PoolObject))]
  public class BulletController : AttackableObject
  {
    public Transform target;

    public float speed = 10f;

    public bool isEnabled = false;

    public float despawnTime = 5f;

    public int maxPenetrateCount = 0;

    private float time;
    private int curPenetrateCount;
    
    [NonSerialized]
    public PoolObject po;

    private void Awake()
    {
      po = GetComponent<PoolObject>();
      po.onGet += () => { isEnabled = true; };
      po.onReleased += () =>
      {
        isEnabled = false;
        curPenetrateCount = 0;
      };
    }

    private void Update()
    {
      if (!isEnabled) return;

      transform.Translate(Vector3.right * (Time.deltaTime * speed));

      time += Time.deltaTime;
      if (time >= despawnTime)
        Destroy(gameObject);
    }

    public void SetTarget(TargetableObject targetableObject)
    {
      target = targetableObject.transform;
      transform.rotation = transform.GetRotationOfLookAtObject(target);
      isEnabled = true;
    }

    protected override void OnInteract(InteractiveObject target)
    {
      base.OnInteract(target);
      if (curPenetrateCount >= maxPenetrateCount)
        po.Release();
      else curPenetrateCount++;
    }
  }
}
