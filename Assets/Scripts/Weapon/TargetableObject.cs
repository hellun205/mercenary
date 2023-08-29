using System.Collections;
using Manager;
using Pool.Extensions;
using UnityEngine;
using Util;

namespace Weapon
{
  public class TargetableObject : UsePool
  {
    public TargetableStatus status;

    public bool canTarget => !isDead;

    public float hp;

    public int index => po.index;

    private SpriteRenderer sr;

    private bool isDead;

    private Coroutiner deadCrt;

    public bool playerAttacked;

    protected override void Awake()
    {
      sr = GetComponent<SpriteRenderer>();
      deadCrt = new Coroutiner(DeadRoutine);

      base.Awake();
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

      po.Release();
    }

    protected override void OnSummon()
    {
      hp = status.maxHp;
      isDead = false;
      playerAttacked = false;
      sr.color = Color.white;
    }

    protected override void OnKilled()
    {
      deadCrt.Stop();
    }

    private void Update()
    {
      sr.color = Color.Lerp(sr.color, Color.white, Time.deltaTime * 5f);
    }

    public void Hit(float damage)
    {
      GameManager.Player.SuccessfulAttack();
      playerAttacked = true;
      hp -= damage;
      sr.color = Color.red;
      GameManager.Pool.Summon<Damage>("ui/damage", transform.GetAroundRandom(0.4f),
        obj => obj.value = Mathf.RoundToInt(damage));

      if (hp <= 0)
      {
        Kill(true);
      }
    }

    public void Kill(bool throwCoin = false)
    {
      isDead = true;
      deadCrt.Start();
    }
  }
}
