using System;
using Manager;
using Pool;
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
      GameManager.Pool.Summon("text", transform.position, obj =>
      {
        var t = obj.GetComponent<TextMeshProUGUI>();
        t.text = $"{damage}";
        t.color = Color.red;
        Utils.Wait(1.5f, obj.Release);
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