using System;
using System.Collections.Generic;
using Manager;
using Pool.Extensions;
using UI;
using UnityEngine;
using Util;

namespace Player
{
  public class PlayerController : MonoBehaviour
  {
    [NonSerialized]
    public PlayerInventory inventory;
    
    private Animator anim;

    public PlayerStatus status;

    private ProgressBar hpBar;

    public WeaponInventory weaponInventory;

    private float curRegenerationHp;

    private float time;

    private List<int> damagedEnemies = new();

    private void Awake()
    {
      inventory = GetComponent<PlayerInventory>();
      weaponInventory = GetComponent<WeaponInventory>();
      hpBar = GameManager.UI.Find<ProgressBar>("$hp");
      anim = GetComponent<Animator>();
      RefreshHpBar();
    }

    private void Update()
    {
      if (time >= 1f)
      {
        time -= 1f;

        curRegenerationHp += status.regeneration;
        if (curRegenerationHp >= 1f)
        {
          var value = Mathf.FloorToInt(curRegenerationHp);
          curRegenerationHp -= value;
          Heal(value);
        }
      }

      time += Time.deltaTime;
    }

    private void LateUpdate()
    {
      RefreshHpBar();
      anim.SetFloat("invincibility", 1 / status.invincibilityTime);
    }

    private void RefreshHpBar()
    {
      hpBar.maxValue = status.maxHp;
      hpBar.value = status.hp;
    }

    private void StartInvincibility()
    {
      status.isInvincibility = true;
    }

    private void EndInvincibility()
    {
      damagedEnemies.Clear();
      status.isInvincibility = false;
    }

    public void Hit(float damage, int index)
    {
      if (status.isInvincibility || damagedEnemies.Contains(index)) return;
      damagedEnemies.Add(index);
      
      var dmg = damage * (1 - status.armor);
      status.hp = Mathf.Max(0, status.hp - dmg);
      anim.SetTrigger("hurt");

      GameManager.Pool.Summon<Damage>("ui/damage", transform.GetAroundRandom(0.4f),
        obj => obj.value = Mathf.RoundToInt(damage));
    }

    public void Heal(int amount)
    {
      var tmp = status.hp;
      status.hp = Mathf.Min(status.maxHp, status.hp + amount);

      var healed = status.hp - tmp;
      if (healed >= 1)
        GameManager.Pool.Summon<Heal>("ui/heal", transform.GetAroundRandom(0.4f),
          obj => obj.value = Mathf.RoundToInt(healed));
    }

    public void SuccessfulAttack()
    {
      if (status.drainHp.ApplyPercentage())
      {
        Heal(1);
      }
    }

    private void Start()
    {
      weaponInventory.Test();
    }
  }
}
