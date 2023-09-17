using System.Collections;
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
    
    public TargetableStatus status;

    public bool canTarget => !isDead;

    public float hp;

    public int index => poolObject.index;

    private SpriteRenderer sr;

    private bool isDead;

    private Coroutiner deadCrt;

    public bool playerAttacked;

    private void Awake()
    {
      sr = GetComponent<SpriteRenderer>();
      deadCrt = new Coroutiner(DeadRoutine);
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
    }

    public void OnKilled()
    {
      deadCrt.Stop();
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
      playerAttacked = true;
      hp -= ao.damage;
      sr.color = Color.red;
      GameManager.Pool.Summon<Damage>("ui/damage", transform.GetAroundRandom(0.4f),
        obj => obj.value = Mathf.RoundToInt(ao.damage));

      if (hp <= 0)
      {
        Kill(true);
      }
    }
  }
}
