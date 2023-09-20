using System;
using Manager;
using UI.DragNDrop;
using UI.Popup;
using UnityEngine;
using UnityEngine.UI;

namespace Store.Equipment
{
  public class WeaponSlot : UsePopup<PopupPanel>
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
        EquipmentType.Player => 0,
        EquipmentType.Partner => wrapper.partnerIndex + 1,
        _ => 0
      };
    }


    public override void OnEntered()
    {
      if (weapon == null) return;
      popupPanel.ShowPopup(weapon.itemName, weapon.description);
    }
  }
}