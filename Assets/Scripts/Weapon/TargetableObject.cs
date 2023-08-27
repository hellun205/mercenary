using System;
using Manager;
using Pool;
using Pool.Extensions;
using TMPro;
using UnityEngine;
using Util;

namespace Weapon
{
  public class TargetableObject : MonoBehaviour
  {
    public TargetableStatus status;

    public bool canTarget;

    public float hp;

    public int index => po.index;

    [NonSerialized]
    public PoolObject po;

    private SpriteRenderer sr;

    private void Awake()
    {
      po = GetComponent<PoolObject>();
      sr = GetComponent<SpriteRenderer>();
      po.onGet += () =>
      {
        hp = status.maxHp;
        canTarget = true;
        sr.color = Color.white;
      };
      // po.onReleased += () => canTarget = false;
    }

    private void Update()
    {
      sr.color = Color.Lerp(sr.color, Color.white, Time.deltaTime * 5f);
    }

    public void Hit(float damage)
    {
      GameManager.Player.SuccessfulAttack();
      hp -= damage;
      sr.color = Color.red;
      GameManager.Pool.Summon<Text>("ui/text", transform.GetAroundRandom(0.4f), obj =>
      {
        obj.value = $"{Mathf.RoundToInt(damage)}";
        obj.color = Color.red;
        Utils.Wait(1.5f, obj.po.Release);
      });

      if (hp <= 0)
      {
        canTarget = false;
        po.Release();
      }
    }
  }
}
