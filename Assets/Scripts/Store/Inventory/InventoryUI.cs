using Manager;
using UnityEngine;

namespace Store.Inventory
{
  public class InventoryUI : MonoBehaviour
  {
    private ItemDrop useDrop;

    private void Awake()
    {
      useDrop = GetComponent<ItemDrop>();
      useDrop.onGetRequest += OnDrop;
    }

    public void OnDrop(ItemRequest data)
    {
      switch (data.beginDragType)
      {
        case DragType.WeaponSlot:
          data.weaponInventoryUI.list[data.weaponSlotData.wrapperId].slots[data.weaponSlotData.slotId].Set(null);
          GameManager.Player.inventory.GainItem(data.item, data.tier);
          break;

        case DragType.PartnerSlot:
          data.partnerSlot.SetPartner(null);
          GameManager.Player.inventory.GainItem(data.item, data.tier);
          break;
      }
    }
  }
}