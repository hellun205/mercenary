using System;
using Interact;
using Manager;
using Player;
using Pool;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace Enemy
{
  [RequireComponent(typeof(PoolObject))]
  public class EnemyBullet : Interacter, IUsePool, IAttacker
  {
    public PoolObject poolObject { get; set; }
    
    public float damage { get; set; }
    public float bulletSpeed { get; set; }
    
    public bool isDead { get; private set; }

    [SerializeField]
    private SpriteRenderer sr;

    private Timer despawnTimer = new Timer();

    private Animator anim;

    private void Awake()
    {
      despawnTimer.duration = 10;
      despawnTimer.onEnd += t => Kill();
      anim = GetComponent<Animator>();
      anim.keepAnimatorStateOnDisable = true;
    }

    private void Reset()
    {
      caster = InteractCaster.Others;
    }

    private void Update()
    {
      transform.Translate(Vector3.right * (Time.deltaTime * bulletSpeed));
    }

    protected override void OnInteract(InteractiveObject target)
    {
      base.OnInteract(target);
      if (target.TryGetComponent<PlayerController>(out var _))
      {
        Kill();
        currentCondition = InteractCondition.Normal;
      }
    }

    public void Kill()
    {
      if (isDead) return;
      isDead = true;
      despawnTimer.Stop();
      sr.color = sr.color.Setter(a: 0f);
      CoroutineUtility.Wait(0.2f, () => poolObject.Release());
    }

    public void OnSummon()
    {
      currentCondition = InteractCondition.Attack;
      isDead = false;
      despawnTimer.Start();
    }
    
    public void SetTarget(float errorRange = 0f)
    {
      var z = transform.GetRotationOfLookAtObject(GameManager.Player.transform);
      var rotation = Random.Range(z.eulerAngles.z - errorRange, z.eulerAngles.z + errorRange);
      transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
  }
}
