using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Enemy;
using Interact;
using Item;
using Manager;
using Pool.Extensions;
using UI;
using UnityEngine;
using Util;
using Attribute = Weapon.Attribute;

namespace Player
{
  public class PlayerController : InteractiveObject
  {
    [NonSerialized]
    public PlayerInventory inventory;

    private Animator anim;

    public PlayerStatus status;

    private ProgressBar hpBar;

    [NonSerialized]
    public WeaponInventory weaponInventory;

    private float curRegenerationHp;

    private float time;

    public List<WeaponInventory> partnerWeaponInventories;

    [SerializeField]
    private SpriteRenderer sr;

    private TweenerCore<Color, Color, ColorOptions> colorTweener;

    private InteractiveObject io;

    public PlayerStatus currentStatus { get; private set; }

    private void Awake()
    {
      if (GameManager.Player == null) GameManager.Player = this;

      inventory = GetComponent<PlayerInventory>();
      weaponInventory = GetComponent<WeaponInventory>();
      io = GetComponent<InteractiveObject>();
      hpBar = GameManager.UI.Find<ProgressBar>("$hp");
      anim = GetComponent<Animator>();
      io.onInteract += OnDamage;
      weaponInventory.onChanged += RefreshStatus;
      GameManager.Wave.onWaveStart += RefreshStatus;
      foreach (var partnerWeaponInventory in partnerWeaponInventories)
      {
        partnerWeaponInventory.onChanged += RefreshStatus;
      }
      RefreshHpBar();
    }

    private void RefreshStatus()
    {
      currentStatus = GetStatus();
    }

    private void OnDamage(Interacter obj)
    {
      if (obj.TryGetComponent<EnemyController>(out var ec))
      {
        Hit(ec.status.damage * (1 - currentStatus.armor));
      }
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
    }

    private void RefreshHpBar()
    {
      hpBar.maxValue = status.maxHp;
      hpBar.value = status.hp;
    }

    private void Hit(float damage)
    {
      var dmg = damage * (1 - status.armor);
      status.hp = Mathf.Max(0, status.hp - dmg);
      colorTweener.Kill();
      sr.color = Color.red;
      colorTweener = sr.DOColor(Color.white, 0.5f);

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

    public PlayerStatus GetStatus()
    {
      var res = status;
      var items = inventory.items;

      foreach (var (item, count) in items.Where(x => x.Key is ItemData))
        res += (item as ItemData).increaseStatus * count;

      res += GetChemistryStatus(out var _);
      
      return res;
    }

    public IncreaseStatus GetChemistryStatus(out Dictionary<Attribute, int> counts)
    {
      var res = new IncreaseStatus();
      var dict = new Dictionary<Attribute, int>();

      void Add(Attribute att)
      {
        if (!dict.TryAdd(att, 1))
          dict[att]++;
      }

      foreach (var weapon in weaponInventory.weapons.Where(x => x != null))
        foreach (var flag in weapon.attribute.GetFlags().Where(x => x != 0))
          Add(flag);

      foreach (var partnerWeaponInventory in partnerWeaponInventories)
        foreach (var weapon in partnerWeaponInventory.weapons.Where(x => x != null))
          foreach (var flag in weapon.attribute.GetFlags().Where(x => x != 0))
            Add(flag);
      
      foreach (var (att, count) in dict)
        res += GameManager.Manager.attributeChemistry.GetIncrease(att, count);

      counts = dict;
      return res;
    }

    // protected override void OnInteract(Interacter caster)
    // {
    //   base.OnInteract(caster);
    //   if (caster.TryGetComponent<EnemyController>(out var ec))
    //   {
    //     Hit(ec.status.damage);
    //   }
    // }

    private void Start()
    {
    }
  }
}
