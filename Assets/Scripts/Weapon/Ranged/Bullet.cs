using System;
using Interact;
using Pool;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace Weapon.Ranged
{
  [RequireComponent(typeof(PoolObject))]
  public class Bullet : AttackableObject, IUsePool
  {
    public float speed = 10f;

    [NonSerialized]
    public bool isEnabled = false;

    // [NonSerialized]
    public int maxPenetrateCount = 0;

    [SerializeField]
    private int curPenetrateCount;

    public Timer despawnTimer = new();

    public PoolObject poolObject { get; set; }
    
    [SerializeField]
    protected InteractiveObject detectEnemy;

    protected virtual void Awake()
    {
      despawnTimer.onEnd += t => poolObject.Release();
      detectEnemy.onInteract += OnDetect;
    }

    protected virtual void OnDetect(Interacter obj)
    {
      if (curPenetrateCount >= maxPenetrateCount)
        poolObject.Release();
      else curPenetrateCount++;
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

    public void SetTarget(TargetableObject targetableObject, float errorRange)
    {
      var z = transform.GetRotationOfLookAtObject(targetableObject.transform);
      var rotation = Random.Range(z.eulerAngles.z - errorRange, z.eulerAngles.z + errorRange);
      transform.rotation = Quaternion.Euler(0, 0, rotation);
      isEnabled = true;
    }
  }
}