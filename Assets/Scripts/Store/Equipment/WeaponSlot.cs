using System;
using System.Collections.Generic;
using System.Text;
using Data;
using DG.Tweening;
using Manager;
using Player;
using Sound;
using UI.DragNDrop;
using UI.Popup;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Util.Text;
using Util.UI;
using Weapon;
using Attribute = Weapon.Attribute;
using WeaponData = Weapon.WeaponData;

namespace Store.Equipment
{
  public class WeaponSlot : UsePopup<ListPopup>, IUseContextMenu
  {
    [SerializeField]
    private Image targetImg;

    [SerializeField]
    private Image panelImg;

    public Outline outline { get; private set; }

    public (string name, int tier)? weapon;

    public int slotIndex;

    private ItemDrag useDrag;
    private ItemDrop useDrop;

    [NonSerialized]
    public WeaponSlotWrapper wrapper;

    private WeaponInventoryUI parentUI;

    private Color defColor;

    public static event Action onSale;

    public Attribute? highlightAttribute { get; set; } = null;
    public Func<IncreaseStatus> highlightStatus { get; set; }

    public override string popupName => "$popup_weapon";

    public bool isIncrease => weapon.HasValue &&
                              highlightAttribute.HasValue &&
                              GameManager.WeaponData.Get(weapon.Value.name).attribute.HasFlag(highlightAttribute.Value);

    protected override void Awake()
    {
      base.Awake();
      outline = GetComponent<Outline>();
      parentUI = FindObjectOfType<WeaponInventoryUI>();
      useDrag = GetComponent<ItemDrag>();
      useDrop = GetComponent<ItemDrop>();

      useDrag.draggingObject = GameManager.UI.Find<DraggingObject>("$dragging_item");
      useDrag.condition = () => weapon != null;
      useDrag.getter = () => new ItemRequest()
      {
        beginDragType = DragType.WeaponSlot,
        item = weapon!.Value.name,
        tier = weapon!.Value.tier,
        weaponSlotData = (GetWrapperIndex(wrapper), slotIndex),
        draggingImage = GameManager.WeaponData.Get(weapon?.name).icon,
        weaponInventoryUI = parentUI,
      };

      useDrop.onGetRequest += OnDrop;
      defColor = panelImg.color;
    }

    private void OnDrop(ItemRequest data)
    {
      var item = GameManager.GetIPossessible(data.item);
      if (item is not WeaponData) return;
      var weapon = item as WeaponData;

      switch (data.beginDragType)
      {
        case DragType.Inventory:
          if (this.weapon != null)
            GameManager.Player.inventory.GainItem(this.weapon.Value.name, this.weapon.Value.tier);
          GameManager.Player.inventory.LoseItem(weapon.name, data.tier);
          Set((weapon.name, data.tier));
          break;

        case DragType.WeaponSlot:
          if (data.weaponSlotData.wrapperId == GetWrapperIndex(wrapper) && data.weaponSlotData.slotId == slotIndex)
            return;
          wrapper.list.Move
          (
            (data.weaponSlotData.wrapperId, data.weaponSlotData.slotId),
            (GetWrapperIndex(wrapper), slotIndex)
          );
          break;

        default:
          throw new ArgumentOutOfRangeException();
      }

      OnEntered();
    }

    public void Set((string name, int tier)? weapon, bool setWeaponInventory = true)
    {
      this.weapon = weapon;
      targetImg.sprite = weapon == null ? null : GameManager.WeaponData.Get(weapon.Value.name).icon;
      targetImg.color = weapon == null ? Color.clear : Color.white;
      panelImg.DOColor(weapon.HasValue ? GameManager.GetTierColor(weapon.Value.tier) : defColor, 0.2f).SetUpdate(true);

      RefreshAdditional();

      if (setWeaponInventory)
        wrapper.SetWeapon(slotIndex, weapon);
    }

    public void RefreshAdditional()
    {
      outline.DOColor(isIncrease ? Color.yellow : Color.black, .2f).SetUpdate(true);
    }

    public static int GetWrapperIndex(WeaponSlotWrapper wrapper)
    {
      return wrapper.type switch
      {
        EquipmentType.Player  => 0,
        EquipmentType.Partner => wrapper.partnerIndex + 1,
        _                     => 0
      };
    }


    public override void OnEntered()
    {
      if (weapon == null) return;
      var sb = new StringBuilder();
      var data = GameManager.WeaponData.Get(weapon.Value.name);

      sb.Append
        (
          $"{data.itemName} {(weapon.Value.tier + 1).ToRomanNumeral()}"
           .SetSizePercent(1.25f)
           .SetAlign(TextAlign.Center)
        )
       .Append("\n")
       .Append
        (
          data.attribute.GetTexts()
           .SetSizePercent(1.25f)
           .AddColor(GameManager.GetAttributeColor())
           .SetLineHeight(1.25f)
           .SetAlign(TextAlign.Center)
        )
       .Append("\n")
       .Append
        (
          (isIncrease
            ? data.GetDescriptionWithAdditionalStatus(weapon.Value.tier, highlightStatus.Invoke())
            : data.GetDescription(weapon.Value.tier))
         .SetAlign(TextAlign.Left)
        );

      popupPanel.ShowPopup(sb.ToString(), GameManager.Data.data.GetAttributeChemistryDescriptions(data.attribute));
    }

    public string contextMenuName => "$context_menu_can_duplicate";

    public object[] contextMenuFormats => new object[]
    {
      weapon!.Value.tier < 3 ? (weapon!.Value.tier + 2).ToRomanNumeral() : "최대",
      $"${GameManager.GetIPossessible(weapon!.Value.name).GetPrice(weapon!.Value.tier) / 2}"
    };

    public bool contextMenuCondition => weapon.HasValue;

    public Action<string> contextMenuFunction => res =>
    {
      switch (res)
      {
        case "duplicate":
          parentUI.DuplicateWeapon(weapon!.Value);
          break;

        case "sell":
          var price = GameManager.GetIPossessible(weapon!.Value.name).GetPrice(weapon!.Value.tier) / 2;
          GameManager.Manager.coin.value += price;
          
          var item = GameManager.GetIPossessible(weapon.Value.name);
          var tier = weapon.Value.tier;
          GameManager.Broadcast.Say
          (
            "{0}(을)를 {1}에 판매하였습니다.",
            $"{item.itemName} {(tier + 1).ToRomanNumeral()}",
            $"${price}"
          );
          
          Set(null);
          onSale?.Invoke();
          GameManager.Sound.Play(SoundType.SFX_Normal, "sfx/normal/sell");
          break;
      }
    };
  }
}
