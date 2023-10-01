using System;
using System.Collections.Generic;
using System.Text;
using Data;
using Manager;
using UI.DragNDrop;
using UI.Popup;
using UnityEngine;
using UnityEngine.UI;
using Util.Text;
using Weapon;
using WeaponData = Weapon.WeaponData;

namespace Store.Equipment
{
  public class WeaponSlot : UsePopup<ListPopup>
  {
    [SerializeField]
    private Image targetImg;

    public (string name, int tier)? weapon;

    public int slotIndex;

    private ItemDrag useDrag;
    private ItemDrop useDrop;

    [NonSerialized]
    public WeaponSlotWrapper wrapper;

    private WeaponInventoryUI parentUI;

    public override string popupName => "$popup_weapon";

    protected override void Awake()
    {
      base.Awake();
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
        draggingImage = GameManager.WeaponData.Get(weapon?.name).icon ,
        weaponInventoryUI = parentUI,
      };

      useDrop.onGetRequest += OnDrop;
    }

    private void OnDrop(ItemRequest data)
    {
      var item = GameManager.GetIPossessible(data.item);
      if (item is not WeaponData) return;

      switch (data.beginDragType)
      {
        case DragType.Inventory:
          if (this.weapon != null)
            GameManager.Player.inventory.GainItem(this.weapon.Value.name, this.weapon.Value.tier);
          GameManager.Player.inventory.LoseItem(weapon!.Value.name, data.tier);
          Set(weapon);
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
      if (setWeaponInventory)
        wrapper.SetWeapon(slotIndex, weapon);
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
      var item = GameManager.WeaponData.Get(weapon.Value.name);

      sb.Append
        (
          item.itemName
           .SetSizePercent(1.5f)
           .SetAlign(TextAlign.Center)
        )
       .Append("\n")
       .Append
        (
          item.attribute.GetTexts()
           .SetSizePercent(1.25f)
           .AddColor(new Color32(72, 156, 255, 255))
           .SetLineHeight(1.25f)
           .SetAlign(TextAlign.Center)
        )
       .Append("\n")
       .Append
        (
          item.description
           .SetAlign(TextAlign.Left)
        );

      popupPanel.ShowPopup(sb.ToString(), GameManager.Data.data.GetAttributeChemistryDescriptions(item.attribute));
    }
  }
}
