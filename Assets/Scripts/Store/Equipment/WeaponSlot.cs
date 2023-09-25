using System;
using System.Collections.Generic;
using System.Text;
using Manager;
using UI.DragNDrop;
using UI.Popup;
using UnityEngine;
using UnityEngine.UI;
using Util.Text;
using Weapon;

namespace Store.Equipment
{
  public class WeaponSlot : UsePopup<ListPopup>
  {
    [SerializeField]
    private Image targetImg;

    public Weapon.WeaponData weapon;

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
        item = weapon,
        weaponSlotData = (GetWrapperIndex(wrapper), slotIndex),
        draggingImage = weapon.icon,
        weaponInventoryUI = parentUI
      };

      useDrop.onGetRequest += OnDrop;
    }

    private void OnDrop(ItemRequest data)
    {
      if (data.item is not Weapon.WeaponData weapon) return;

      switch (data.beginDragType)
      {
        case DragType.Inventory:
          if (this.weapon != null)
            GameManager.Player.inventory.GainItem(this.weapon);
          GameManager.Player.inventory.LoseItem(weapon);
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

    public void Set(Weapon.WeaponData weapon, bool setWeaponInventory = true)
    {
      this.weapon = weapon;
      targetImg.sprite = weapon == null ? null : weapon.icon;
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

      sb.Append
        (
          weapon.itemName
           .SetSizePercent(1.5f)
           .SetAlign(TextAlign.Center)
        )
       .Append("\n")
       .Append
        (
          weapon.attribute.GetTexts()
           .SetSizePercent(1.25f)
           .AddColor(new Color32(72, 156, 255, 255))
           .SetLineHeight(1.25f)
           .SetAlign(TextAlign.Center)
        )
       .Append("\n")
       .Append
        (
          weapon.description
           .SetAlign(TextAlign.Left)
        );

      popupPanel.ShowPopup(sb.ToString(), GameManager.Manager.attributeChemistry.GetDescriptions(weapon.attribute));
    }
  }
}
