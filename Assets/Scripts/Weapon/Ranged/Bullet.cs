using System;
using Interact;
using Pool;
using UnityEngine;
using Util;

namespace Weapon.Ranged
{
  [RequireComponent(typeof(PoolObject))]
  public class Bullet : AttackableObject, IUsePool
  {
    [NonSerialized]
    public Transform target;

    [NonSerialized]
    public float speed = 10f;

    [NonSerialized]
    public bool isEnabled = false;

    [NonSerialized]
    public int maxPenetrateCount = 0;

    private int curPenetrateCount;

    public Timer despawnTimer = new();

    public PoolObject poolObject { get; set; }

    protected virtual bool isKillOnInteract => true;

    protected virtual void Awake()
    {
      despawnTimer.onEnd += t => poolObject.Release();
    }

    public virtual void OnSummon()
    {
      isEnabled = true;
      despawnTimer.Start();
    }

    public virtual void OnKilled()
    {
      isEnabled = false;
      curPenetrateCount = 0;
      despawnTimer.Stop();
    }

    private void Update()
    {
      if (!isEnabled) return;
      Move();
    }

    protected virtual void Move()
    {
      transform.Translate(Vector3.right * (Time.deltaTime * speed));
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
      if (!isKillOnInteract) return;
      if (curPenetrateCount >= maxPenetrateCount)
        poolObject.Release();
      else curPenetrateCount++;
    }
  }
}