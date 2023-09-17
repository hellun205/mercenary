using System;
using Manager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Store.Inventory
{
  public class InventoryUI : MonoBehaviour, IDropHandler
  {
    public static DragItem draggingItem;

    private void Awake()
    {
      draggingItem ??= GameManager.UI.Find<DragItem>("$dragging_item");
    }
    
    public void OnDrop(PointerEventData eventData)
    {
      if (!draggingItem.isWeaponInventory) return;
      
      draggingItem.list.list[draggingItem.wrapperIndex].slots[draggingItem.weaponInventoryIndex].Set(null);
      GameManager.Player.inventory.GainItem(draggingItem.itemData);
    }
  }
}
