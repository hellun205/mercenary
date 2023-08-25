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

    private PoolObject po;

    private void Awake()
    {
      po = GetComponent<PoolObject>();
      po.onGet += () =>
      {
        hp = status.maxHp;
        canTarget = true;
      };
      po.onReleased += () => canTarget = false;
    }

    private void Update()
    {
      GetComponent<SpriteRenderer>().color =
        Color.Lerp(GetComponent<SpriteRenderer>().color, Color.white, Time.deltaTime * 5f);
    }

    public void Hit(float damage)
    {
      hp -= damage;
      GetComponent<SpriteRenderer>().color = Color.red;
      GameManager.Pool.Summon<Text>("text", transform.position, obj =>
      {
        obj.value = $"{damage}";
        obj.color = Color.red;
        Utils.Wait(1.5f, obj.po.Release);
      });

      if (hp <= 0)
      {
        canTarget = false;
        GameManager.Spawn.Spawn(transform.position, "coin");
        po.Release();
      }
    }
  }
}