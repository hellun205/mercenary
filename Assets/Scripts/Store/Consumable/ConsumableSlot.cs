using System;
using Consumable;
using Item;
using Manager;
using UI.DragNDrop;
using UI.Popup;
using UnityEngine;
using UnityEngine.UI;

namespace Store.Consumable
{
  [RequireComponent(typeof(ItemDrag), typeof(ItemDrop))]
  public class ConsumableSlot : UsePopup<ListPopup>
  {
    public override string popupName => "$popup_item";

    [SerializeField]
    private Image iconImg;

    public string itemData { get; private set; }

    private ItemDrop useDrop;
    private ItemDrag useDrag;

    public int index { get; set; }

    protected override void Awake()
    {
      base.Awake();
      useDrag = GetComponent<ItemDrag>();
      useDrop = GetComponent<ItemDrop>();

      useDrag.draggingObject = GameManager.UI.Find<DraggingObject>("$dragging_item");
      useDrag.condition = () => !string.IsNullOrEmpty(itemData);
      useDrag.getter = () => new ItemRequest
      {
        beginDragType = DragType.ConsumableSlot,
        item = itemData,
        draggingImage = GameManager.GetIPossessible(itemData).icon,
        consumableSlotIndex = index
      };

      useDrop.onGetRequest += OnDrop;
    }

    private void OnDrop(ItemRequest data)
    {
      if (GameManager.GetIPossessible(data.item) is not ConsumableItem)
        return;

      switch (data.beginDragType)
      {
        case DragType.Inventory:
          GameManager.Player.inventory.LoseItem(data.item, 0);
          SetItem(data.item);
          break;
        case DragType.ConsumableSlot:
          var wrapper = GameManager.UI.Find<ConsumableSlotWrapper>("$consumable_wrapper");
          var targetSlot = wrapper.slots[data.consumableSlotIndex];

          var temp = itemData;
          
          SetItem(targetSlot.itemData);
          targetSlot.SetItem(temp);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public void SetItem(string item)
    {
      itemData = item;
      iconImg.sprite = string.IsNullOrEmpty(item) ? null : GameManager.GetIPossessible(itemData).icon;
      iconImg.color = string.IsNullOrEmpty(item) ? Color.clear : Color.white;

      var displayImg = GameManager.UI.Find<Image>($"$buff_display_{index}");
      displayImg.sprite = iconImg.sprite;
      displayImg.color = iconImg.color;
    }

    public override void OnEntered()
    {
      if (string.IsNullOrEmpty(itemData)) return;
      
      popupPanel.ShowPopup(GameManager.GetIPossessible(itemData).GetDescription());
    }
  }
}