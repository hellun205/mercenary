using System.Collections;
using System.Collections.Generic;
using Interact;
using Manager;
using Pool;
using Pool.Extensions;
using UnityEngine;
using Util;

namespace Weapon
{
  public class TargetableObject : InteractiveObject, IUsePool
  {
    public PoolObject poolObject { get; set; }
    
    public bool canTarget => !isDead;

    public float hp;

    public int index => poolObject.index;

    private SpriteRenderer sr;

    private bool isDead;

    private Coroutiner deadCrt;

    public bool playerAttacked;

    private Queue<float> bleedingQueue = new();

    public const float bleedingDelay = 0.7f;
    
    [Header("Targetable Object")]
    public TargetableStatus status;
    
    [SerializeField]
    private Timer bleedingTimer = new ();

    private void Awake()
    {
      sr = GetComponent<SpriteRenderer>();
      deadCrt = new Coroutiner(DeadRoutine);
      bleedingTimer.duration = bleedingDelay;
      bleedingTimer.onEnd += OnTimerEnd;
    }

    private void OnTimerEnd(Timer sender)
    {
      if (bleedingQueue.TryDequeue(out var amount))
        Damage(amount);
      CoroutineUtility.WaitUntil(() => bleedingQueue.Count > 0, () => sender.Start());
    }

    private IEnumerator DeadRoutine()
    {
      var time = 0f;
      while (!sr.color.a.ApproximatelyEqual(0f, 0.1f))
      {
        sr.color = sr.color.Setter(a: Mathf.Lerp(sr.color.a, 0f, time));
        time += Time.deltaTime;
        yield return new WaitForEndOfFrame();
      }

      poolObject.Release();
    }

    public void OnSummon()
    {
      hp = status.maxHp;
      isDead = false;
      playerAttacked = false;
      sr.color = Color.white;
      bleedingTimer.Start();
    }

    public void OnKilled()
    {
      deadCrt.Stop();
      bleedingTimer.Stop();
      bleedingQueue.Clear();
    }

    private void Update()
    {
      sr.color = Color.Lerp(sr.color, Color.white, Time.deltaTime * 5f);
    }

    public void Kill(bool throwCoin = false)
    {
      isDead = true;
      deadCrt.Start();
    }

    protected override void OnInteract(Interacter caster)
    {
      if (!caster.TryGetComponent<AttackableObject>(out var ao)) return;
      
      GameManager.Player.SuccessfulAttack();
      Damage(ao.damage);


      if (ao.isCritical)
      {
        for (var i = 0; i < AttackableObject.bleedingCount; i++)
          bleedingQueue.Enqueue(ao.bleeding / AttackableObject.bleedingCount);
      }
    }

    private void Damage(float amount)
    {
      hp -= amount;
      sr.color = Color.red;
      GameManager.Pool.Summon<Damage>("ui/damage", transform.GetAroundRandom(0.4f),
        obj => obj.value = Mathf.RoundToInt(amount));

      if (hp <= 0)
      {
        playerAttacked = true;
        Kill(true);
      }
    }
  }
}
