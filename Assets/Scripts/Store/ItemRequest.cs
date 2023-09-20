using Item;
using Store.Equipment;
using UI.DragNDrop;

namespace Store
{
  public class ItemRequest : DragRequest
  {
    public DragType beginDragType { get; set; }
    public WeaponInventoryUI weaponInventoryUI { get; set; } 
    public IPossessible item { get; set; }
    public (int wrapperId, int slotId) weaponSlotData { get; set; }
  }
}