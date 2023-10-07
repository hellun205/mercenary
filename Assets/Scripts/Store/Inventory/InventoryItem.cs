using System;
using System.Linq;
using System.Text;
using Consumable;
using Data;
using Item;
using Manager;
using Store.Equipment;
using TMPro;
using UI;
using UI.DragNDrop;
using UI.Popup;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Util;
using Util.Text;
using Weapon;
using ItemData = Item.ItemData;
using PartnerData = Player.Partner.PartnerData;
using WeaponData = Weapon.WeaponData;

namespace Store.Inventory
{
  public class InventoryItem
    : UsePopup<ListPopup>,
      IPointerClickHandler,
      IPoolableUI<InventoryItem>,
      IFilterable<InventoryItemType>
  {
    public InventoryItemType filterType
    {
      get
      {
        var data = itemData!.Value;
        var get = GameManager.GetIPossessible(data.name);
        return get switch
        {
          ItemData       => InventoryItemType.StatusItem,
          WeaponData     => InventoryItemType.Weapon,
          ConsumableItem => InventoryItemType.ConsumableItem,
          PartnerData    => InventoryItemType.Partner,
          _ => throw new ArgumentOutOfRangeException()
        };
      }
    }

    public (string name, int tier)? itemData;

    [SerializeField]
    private TextMeshProUGUI count;

    [SerializeField]
    private Image icon;

    public InventoryUI parentUI;

    private Image img;

    public override string popupName => "$popup_item";

    private ItemDrop useDrop;
    private ItemDrag useDrag;

    private Timer doubleClickTimer = new Timer { duration = 0.3f };
    private bool isClicked;

    protected override void Awake()
    {
      base.Awake();
      doubleClickTimer.onEnd += _ => isClicked = false;
      img = GetComponent<Image>();
      parentUI = FindObjectOfType<InventoryUI>();
      useDrag = GetComponent<ItemDrag>();
      useDrop = GetComponent<ItemDrop>();

      useDrag.draggingObject = GameManager.UI.Find<DraggingObject>("$dragging_item");
      useDrag.getter = () => new ItemRequest()
      {
        beginDragType = DragType.Inventory,
        item = itemData!.Value.name,
        tier = itemData!.Value.tier,
        draggingImage = GameManager.GetIPossessible(itemData!.Value.name).icon,
        weaponInventoryUI = FindObjectOfType<WeaponInventoryUI>()
      };

      useDrop.onGetRequest += OnDrop;
    }

    private void OnDrop(ItemRequest data)
    {
      parentUI.OnDrop(data);
    }

    public void SetItem((string name, int tier)? item, ushort count)
    {
      itemData = item;
      icon.sprite = GameManager.GetIPossessible(itemData!.Value.name).icon;
      img.color = GameManager.GetTierColor(itemData.Value.tier);
      SetCount(count);
    }

    public void SetCount(ushort count)
    {
      this.count.text = $"{(count == 1 ? "" : count)}";
    }

    public override void OnEntered()
    {
      popupPanel.HidePopup();
      // popupPanel.ShowPopup
      // (
      //   itemData.itemName,
      //   itemData is WeaponData weapon ? weapon.attribute.GetTexts() : "",
      //   itemData.description
      // );

      var sb = new StringBuilder();
      var item = GameManager.GetIPossessible(itemData!.Value.name);

      sb.Append
        (
          $"{item.itemName} {(itemData.Value.tier + 1).ToRomanNumeral(true)}"
           .SetSizePercent(1.25f)
           .SetAlign(TextAlign.Center)
        )
       .Append("\n");
      if (item is WeaponData weaponData)
      {
        sb.Append
          (
            weaponData.attribute.GetTexts()
             .SetSizePercent(1.25f)
             .AddColor(GameManager.GetAttributeColor())
             .SetLineHeight(1.25f)
             .SetAlign(TextAlign.Center)
          )
         .Append("\n");
      }

      sb.Append
      (
        item.GetDescription(itemData.Value.tier)
         .SetAlign(TextAlign.Left)
      );

      if (item is WeaponData weaponData2)
        popupPanel.ShowPopup(sb.ToString(),
          GameManager.Data.data.GetAttributeChemistryDescriptions(weaponData2.attribute));
      else
        popupPanel.ShowPopup(text: sb.ToString());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      if (isClicked)
      {
        if (doubleClickTimer.isPlaying)
        {
          isClicked = false;
          EquipAny();
        }
      }
      else
      {
        isClicked = true;
        doubleClickTimer.Start();
      }
    }

    private void EquipAny()
    {
      var f = FindObjectsOfType<WeaponSlot>();

      foreach (var weaponSlot in f)
      {
        if (weaponSlot.wrapper.type == EquipmentType.Partner && !weaponSlot.wrapper.partnerSlot.partner.HasValue
            || weaponSlot.weapon.HasValue)
          continue;

        weaponSlot.Set(itemData);
        if (itemData.HasValue)
          GameManager.Player.inventory.LoseItem(itemData.Value.name, itemData.Value.tier);
        break;
      }
    }
    public InventoryItem component => this;
  }
}
