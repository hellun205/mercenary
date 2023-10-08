using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coin;
using Consumable;
using Data;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Enemy;
using Interact;
using Manager;
using Player.Partner;
using Pool.Extensions;
using Scene;
using Store.Consumable;
using Store.Equipment;
using Store.Status;
using Transition;
using UI;
using UnityEngine;
using Util;
using Weapon;
using Weapon.Ranged.Bomb;
using Attribute = Weapon.Attribute;
using ItemData = Item.ItemData;
using WeaponData = Weapon.WeaponData;

namespace Player
{
  public class PlayerController : InteractiveObject
  {
    [NonSerialized]
    public PlayerInventory inventory;

    private Animator anim;

    public PlayerStatus status;

    private ProgressBar hpBar;
    public PlayerMovement movement { get; private set; }

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

    public PlayerStatus currentStatus;

    public Dictionary<BuffData, BuffInformationItem> buffs { get; } = new();

    public Dictionary<string, Action<float>> activeConsumable;
    public Dictionary<string, Action<float>> endActiveConsumable;

    private Timer invincibilityTimer = new Timer();
    private bool isInvincibility;
    public float moveSpeedPercent { get; private set; }

    public bool isResurrection { get; private set; }

    private void Awake()
    {
      if (GameManager.Player == null) GameManager.Player = this;

      movement = GetComponent<PlayerMovement>();
      inventory = GetComponent<PlayerInventory>();
      weaponInventory = GetComponent<WeaponInventory>();
      io = GetComponent<InteractiveObject>();
      hpBar = GameManager.UI.Find<ProgressBar>("$hp");
      anim = GetComponent<Animator>();
      partners = GetComponentsInChildren<PartnerController>();
      io.onInteract += OnDamage;
      weaponInventory.onChanged += () => RefreshStatus();
      GameManager.Wave.onWaveStart += () => RefreshStatus();
      GameManager.Wave.onWaveEnd += WaveOnonWaveEnd;
      activeConsumable = new()
      {
        { "resurrection", OnUseResurrection },
        { "killEnemy", OnUseKillEnemy }
      };
      endActiveConsumable = new()
      {
        { "resurrection", OnEndResurrection }
      };
      invincibilityTimer.onEnd += _ => isInvincibility = false;
      invincibilityTimer.onStart += _ => isInvincibility = true;

      partners.Length.For(i =>
      {
        partners[i].wrapper = FindObjectsOfType<WeaponSlotWrapper>()
         .First
          (
            x => x.type == EquipmentType.Partner && x.partnerIndex == i
          );
      });

      RefreshHpBar();
    }

    public PlayerStatus RefreshStatus()
    {
      currentStatus = GetStatus();
      return currentStatus;
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

      if (GameManager.Wave.state)
        GameManager.Key.KeyMap(GetKeyType.Down,
          (Keys.UseItem1, () => UseItem(0)),
          (Keys.UseItem2, () => UseItem(1)),
          (Keys.UseItem3, () => UseItem(2))
        );
    }

    private void UseItem(int index)
    {
      var wrapper = GameManager.UI.Find<ConsumableSlotWrapper>("$consumable_wrapper");
      var slot = wrapper.slots[index];

      if (string.IsNullOrEmpty(slot.itemData)) return;

      UseConsumableItem(slot.itemData);
      slot.SetItem(null);
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
      if (isInvincibility) return;

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

      if (status.hp <= 0)
        if (isResurrection)
          Resurrection();
        else
          Dead();
    }

    private void Resurrection()
    {
      Debug.Log("resurrection!");
      status.hp = currentStatus.maxHp;
      var buff = buffs
       .Where(x => x.Key.status.GetValue("resurrection") > 0)
       .OrderByDescending(x => x.Key.timer.elapsedTime)
       .First()
       .Key;

      invincibilityTimer.duration = buff.status.GetValue("resurrection");
      invincibilityTimer.Start();

      OnBuffEnd(buff);
    }

    public void Dead()
    {
      Debug.Log("Dead");
      GameManager.Manager.GameOver();
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

    private PlayerStatus GetStatus()
    {
      var res = status;
      var increases = new IncreaseStatus()
      {
        moveSpeed = "%+0"
      };
      var items = inventory.items;

      foreach (var (item, count) in items.Where(x => GameManager.GetIPossessible(x.Key.name) is ItemData))
        increases += ((ItemData) GameManager.GetItem(item.name)).status * count;

      foreach (var (data, ui) in buffs)
        increases += data.status;

      foreach (var item in weaponInventory.weapons.Where(x => x.HasValue).Select
                 (x => ((WeaponData) GameManager.GetIPossessible(x.Value.name)).increaseStatus[x.Value.tier]))
        increases += item;

      foreach (var partnerWI in partners.Select(x => x.weaponInventory.weapons))
        foreach (var item in partnerWI.Where(x => x.HasValue).Select
                   (x => ((WeaponData) GameManager.GetIPossessible(x.Value.name)).increaseStatus[x.Value.tier]))
          increases += item;

      increases += GetChemistryStatus(out var _);

      res += increases;
      moveSpeedPercent = increases.GetValue("moveSpeed");
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

    private void WaveOnonWaveEnd()
    {
      foreach (var (data, ui) in buffs)
      {
        OnBuffEnd(data);
      }
    }

    public void UseConsumableItem(string consumableItemName)
    {
      var consumable = GameManager.Consumables.Get(consumableItemName) as ConsumableItem;
      var buffWrapper = GameManager.UI.Find<BuffInformation>("$buff_wrapper");
      var stat = consumable.GetStatus();

      if (consumable.GetDuration() > 0)
      {
        var buff = new BuffData(consumable)
        {
          onEnd = OnBuffEnd,
          onTick = OnBuffTick
        };

        buffs.Add(buff, buffWrapper.Add(buff.name, buff.icon, stat.GetDescription()));
        buff.StartTimer();
      }

      foreach (var (fieldName, action) in activeConsumable)
      {
        var value = stat.GetValue(fieldName);
        if (value > 0)
          action.Invoke(value);
      }

      RefreshStatus();
    }

    private void OnBuffTick(BuffData targetBuffData)
    {
      buffs[targetBuffData].timerImage.fillAmount = targetBuffData.timer.value;
    }

    private void OnBuffEnd(BuffData targetBuffData)
    {
      var buffWrapper = GameManager.UI.Find<BuffInformation>("$buff_wrapper");
      targetBuffData.timer.Stop();
      buffWrapper.Remove(buffs[targetBuffData]);
      buffs.Remove(targetBuffData);

      foreach (var (fieldName, action) in endActiveConsumable)
      {
        var value = targetBuffData.status.GetValue(fieldName);
        if (value > 0)
          action.Invoke(value);
      }

      RefreshStatus();
    }

    private void OnUseResurrection(float value)
    {
      isResurrection = true;
    }

    private void OnEndResurrection(float obj)
    {
      isResurrection = false;
    }

    private void OnUseKillEnemy(float value)
    {
      var enemies = FindObjectsOfType<TargetableObject>();
      var count = value > 9999 ? 9999 : Mathf.FloorToInt(value);
      Debug.Log(count);
      var i = 0;

      foreach (var targetableObject in enemies)
      {
        if (i >= count) break;
        GameManager.Pool.Summon<ExplosionEffectController>("effect/explosion", targetableObject.transform.position,
          obj => { obj.SetRange(2f); });
        targetableObject.Kill(true);
        i++;
      }
    }

    private void Start()
    {
      foreach (var partner in partners)
        partner.weaponInventory.onChanged += () => RefreshStatus();
      status = GameManager.Data.data.GetPlayerStatus();
      coinExplorer.GetComponent<CircleCollider2D>().radius =
        GameManager.Data.data.GetPlayerStatusData(PlayerStatusItem.CoinDetectRange) / 10;
      FindObjectOfType<WeaponInventoryUI>().SetWeapon(0, 0, (GameManager.Manager.startWeaponName, 0));
      FindObjectOfType<Status>().Refresh();
    }
  }
}
