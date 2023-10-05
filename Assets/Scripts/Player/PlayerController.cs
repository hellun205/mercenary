using System;
using System.Collections.Generic;
using System.Linq;
using Coin;
using Data;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Enemy;
using Interact;
using Manager;
using Player.Partner;
using Pool.Extensions;
using Store.Equipment;
using Store.Status;
using UI;
using UnityEngine;
using Util;
using Attribute = Weapon.Attribute;
using ItemData = Item.ItemData;

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

    // public List<WeaponInventory> partnerWeaponInventories;
    public PartnerController[] partners;

    [SerializeField]
    private SpriteRenderer sr;

    private TweenerCore<Color, Color, ColorOptions> colorTweener;

    private InteractiveObject io;

    [SerializeField]
    private CoinExplorer coinExplorer;

    public PlayerStatus currentStatus { get; private set; }

    private void Awake()
    {
      if (GameManager.Player == null) GameManager.Player = this;

      inventory = GetComponent<PlayerInventory>();
      weaponInventory = GetComponent<WeaponInventory>();
      io = GetComponent<InteractiveObject>();
      hpBar = GameManager.UI.Find<ProgressBar>("$hp");
      anim = GetComponent<Animator>();
      partners = GetComponentsInChildren<PartnerController>();
      io.onInteract += OnDamage;
      weaponInventory.onChanged += RefreshStatus;
      GameManager.Wave.onWaveStart += RefreshStatus;

      RefreshHpBar();
    }

    private void RefreshStatus()
    {
      currentStatus = GetStatus();
    }

    private void OnDamage(Interacter obj)
    {
      if (obj.TryGetComponent(typeof(IAttacker), out var component))
      {
        Hit(((IAttacker) component).damage * (1 - currentStatus.armor));
      }
    }

    private void Update()
    {
      if (time >= 1f)
      {
        time -= 1f;

        curRegenerationHp += currentStatus.regeneration / 10;
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
      hpBar.maxValue = currentStatus.maxHp;
      hpBar.value = status.hp;
    }

    private void Hit(float damage)
    {
      if (currentStatus.evasionRate.ApplyProbability())
      {
        GameManager.Pool.Summon("ui/miss", transform.GetAroundRandom(0.4f));
        return;
      }
      var dmg = damage * (1 - currentStatus.armor);
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
      status.hp = Mathf.Min(currentStatus.maxHp, status.hp + amount);

      var healed = status.hp - tmp;
      if (healed >= 1)
        GameManager.Pool.Summon<Heal>("ui/heal", transform.GetAroundRandom(0.4f),
          obj => obj.value = Mathf.RoundToInt(healed));
    }

    public void SuccessfulAttack()
    {
      if (currentStatus.drainHp.ApplyProbability())
      {
        Heal(1);
      }
    }

    public PlayerStatus GetStatus()
    {
      var res = status;
      var items = inventory.items;

      foreach (var (item, count) in items.Where(x => GameManager.GetIPossessible(x.Key.name) is ItemData))
        res += ((ItemData) GameManager.GetItem(item.name)).status * count;

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

      foreach (var weapon in weaponInventory.weapons.Where(x => x != null)
                .Select(x => GameManager.WeaponData.Get(x.Value.name)))
        foreach (var flag in weapon.attribute.GetFlags().Where(x => x != 0))
          Add(flag);

      foreach (var partnerWeaponInventory in partners.Select(x => x.weaponInventory))
        foreach (var weapon in partnerWeaponInventory.weapons.Where(x => x != null)
                  .Select(x => GameManager.WeaponData.Get(x.Value.name)))
          foreach (var flag in weapon.attribute.GetFlags().Where(x => x != 0))
            Add(flag);

      foreach (var (att, count) in dict)
        res += GameManager.Data.data.GetAttributeChemistryIncrease(att, count);

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
      foreach (var partner in partners)
        partner.weaponInventory.onChanged += RefreshStatus;
      status = GameManager.Data.data.GetPlayerStatus();
      coinExplorer.GetComponent<CircleCollider2D>().radius =
        GameManager.Data.data.GetPlayerStatusData(PlayerStatusItem.CoinDetectRange) / 10;
      FindObjectOfType<WeaponInventoryUI>().SetWeapon(0, 0, (GameManager.Manager.startWeaponName, 0));
      FindObjectOfType<Status>().Refresh();
    }
  }
}
