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
      if (data.beginDragType != DragType.WeaponSlot) return;

      data.weaponInventoryUI.list[data.weaponSlotData.wrapperId].slots[data.weaponSlotData.slotId].Set(null);
      GameManager.Player.inventory.GainItem(data.item);
    }
  }
}