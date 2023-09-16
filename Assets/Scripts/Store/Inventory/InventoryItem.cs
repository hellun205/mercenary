using System;
using Item;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Util;

namespace Store.Inventory
{
  public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
  {
    private Canvas canvas;
    public ItemData itemData;

    [SerializeField]
    private TextMeshProUGUI count;

    [SerializeField]
    private Image icon;

    private static Popup popup;

    private void Awake()
    {
      popup ??= GameManager.UI.Find<Popup>("$popup_item");
      canvas = GetComponentInParent<Canvas>();
    }

    public void SetItem(ItemData item, ushort count)
    {
      itemData = item;
      icon.sprite = itemData.icon;
      SetCount(count);
    }

    public void SetCount(ushort count)
    {
      this.count.text = $"{(count == 1 ? "" : count)}";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      Move(eventData);

      popup.ShowPopup(itemData.itemName, itemData.GetDescription());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      popup.HidePopup();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
      Move(eventData);
    }

    public void Move(PointerEventData eventData)
    {
      popup.rectTransform.anchoredPosition = canvas.ScreenToCanvasPosition(eventData.position);
    }
  }
}
