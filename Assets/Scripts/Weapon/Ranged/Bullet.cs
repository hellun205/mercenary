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

    [SerializeField]
    private SpriteRenderer sr;

    private bool isDead;

    protected virtual void Awake()
    {
      despawnTimer.onEnd += t => Kill();
      detectEnemy.onInteract += OnDetect;
    }

    protected virtual void OnDetect(Interacter obj)
    {
      if (curPenetrateCount >= maxPenetrateCount)
        Kill();
      else curPenetrateCount++;
    }

    public virtual void OnSummon()
    {
      isEnabled = true;
      detectEnemy.enabled = true;
      despawnTimer.Start();
      canAttack = true;
      sr.color = sr.color.Setter(a: 1f);
      currentCondition = InteractCondition.Attack;
    }

    public virtual void OnKilled()
    {
      isDead = false;
      isEnabled = false;
      canAttack = false;
      curPenetrateCount = 0;
      despawnTimer.Stop();
      currentCondition = InteractCondition.Normal;
    }

    protected void Kill()
    {
      if (isDead) return;
      isDead = true;
      isEnabled = false;
      sr.color = sr.color.Setter(a: 0f);
      CoroutineUtility.Wait(0.05f, () => poolObject.Release());
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

    public void SetTarget(float z, float errorRange)
    {
      var rotation = Random.Range(z - errorRange, z + errorRange);
      transform.rotation = Quaternion.Euler(0, 0, rotation);
      isEnabled = true;
    }
  }
}