using Item;
using Manager;
using Popup;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Util;

namespace Store.Inventory
{
  public class InventoryItem : UsePopup<PopupPanel>, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerUpHandler
  {
    public IPossessible itemData;

    [SerializeField]
    private TextMeshProUGUI count;

    [SerializeField]
    private Image icon;

    public InventoryUI parentUI;
      
    public override string popupName => "$popup_item";
    
    public static DragItem draggingItem;

    protected override void Awake()
    {
      base.Awake();
      draggingItem ??= GameManager.UI.Find<DragItem>("$dragging_item");
      parentUI = FindObjectOfType<InventoryUI>();
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

    public void OnBeginDrag(PointerEventData eventData)
    {
      draggingItem.SetItem(itemData, false);
    }

    public void OnDrag(PointerEventData eventData)
    {
      if (!draggingItem.isDragging) return;
      draggingItem.rectTransform.anchoredPosition = canvas.ScreenToCanvasPosition(Input.mousePosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
      draggingItem.EndDrag();
    }

    public void OnDrop(PointerEventData eventData)
    {
      parentUI.OnDrop(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
      draggingItem.EndDrag();
    }
  }
}
