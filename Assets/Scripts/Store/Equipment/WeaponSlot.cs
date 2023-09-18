using System;
using Manager;
using Popup;
using Store.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Util;

namespace Store.Equipment
{
  public class WeaponSlot : UsePopup<PopupPanel>, IDropHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
  {
    [SerializeField]
    private Image targetImg;

    public Weapon.Weapon weapon;

    public int slotIndex;

    [NonSerialized]
    public WeaponSlotWrapper wrapper;

    public static DragItem draggingItem;

    public override string popupName => "$popup_weapon";

    protected override void Awake()
    {
      base.Awake();
      draggingItem ??= GameManager.UI.Find<DragItem>("$dragging_item");
    }

    public void Set(Weapon.Weapon weapon, bool setWeaponInventory = true)
    {
      this.weapon = weapon;
      targetImg.sprite = weapon == null ? null : weapon.icon;
      targetImg.color = weapon == null ? Color.clear : Color.white;
      if (setWeaponInventory)
        wrapper.SetWeapon(slotIndex, weapon);
    }

    public void OnDrop(PointerEventData eventData)
    {
      if (!draggingItem.isDragging || draggingItem.itemData is not Weapon.Weapon weapon) return;

      if (draggingItem.isWeaponInventory)
      {
        if (draggingItem.wrapperIndex == GetWrapperIndex(wrapper) && draggingItem.weaponInventoryIndex == slotIndex)
          return;
        wrapper.list.Move
        (
          (draggingItem.wrapperIndex, draggingItem.weaponInventoryIndex),
          (GetWrapperIndex(wrapper), slotIndex)
        );
      }
      else
      {
        if (this.weapon != null)
          GameManager.Player.inventory.GainItem(this.weapon);
        GameManager.Player.inventory.LoseItem(weapon);
        Set(weapon);
      }

      draggingItem.EndDrag();
      OnEntered();
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

    public void OnBeginDrag(PointerEventData eventData)
    {
      if (weapon == null) return;
      draggingItem.SetItem(weapon, true, GetWrapperIndex(wrapper), slotIndex);
    }

    public void OnDrag(PointerEventData eventData)
    {
      if (!draggingItem.isDragging) return;
      draggingItem.rectTransform.anchoredPosition = canvas.ScreenToCanvasPosition(Input.mousePosition);
    }

    public override void OnEntered()
    {
      if (weapon == null) return;
      popupPanel.ShowPopup(weapon.itemName, weapon.description);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
      draggingItem.EndDrag();
    }
  }
}
