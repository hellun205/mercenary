using Item;
using Manager;
using Store.Equipment;
using TMPro;
using UI.DragNDrop;
using UI.Popup;
using UnityEngine;
using UnityEngine.UI;

namespace Store.Inventory
{
  public class InventoryItem : UsePopup<PopupPanel>
  {
    public IPossessible itemData;

    [SerializeField]
    private TextMeshProUGUI count;

    [SerializeField]
    private Image icon;

    public InventoryUI parentUI;
      
    public override string popupName => "$popup_item";

    private ItemDrop useDrop;
    private ItemDrag useDrag;

    protected override void Awake()
    {
      base.Awake();
      parentUI = FindObjectOfType<InventoryUI>();
      useDrag = GetComponent<ItemDrag>();
      useDrop = GetComponent<ItemDrop>();

      useDrag.draggingObject = GameManager.UI.Find<DraggingObject>("$dragging_item");
      useDrag.getter = () => new ItemRequest()
      {
        beginDragType = DragType.Inventory,
        item = itemData,
        draggingImage = itemData.icon,
        weaponInventoryUI = FindObjectOfType<WeaponInventoryUI>()
      };
      
      useDrop.onGetRequest += OnDrop;
    }

    private void OnDrop(ItemRequest data)
    {
      parentUI.OnDrop(data);
    }

    public void SetItem(IPossessible item, ushort count)
    {
      itemData = item;
      icon.sprite = itemData.icon;
      SetCount(count);
    }

    public void SetCount(ushort count)
    {
      this.count.text = $"{(count == 1 ? "" : count)}";
    }

    public override void OnEntered()
    {
      popupPanel.ShowPopup(itemData.itemName, itemData.description);
    }
  }
}
