using System;
using System.Text;
using Consumable;
using Item;
using Manager;
using UI.DragNDrop;
using UI.Popup;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Util.Text;
using Util.UI;
using Weapon;

namespace Store.Consumable
{
  [RequireComponent(typeof(ItemDrag), typeof(ItemDrop))]
  public class ConsumableSlot : UsePopup<ListPopup>, IUseContextMenu
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
          GameManager.Player.inventory.LoseItem(data.item, -1);
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
      OnEntered();
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
      var sb = new StringBuilder();
      var item = GameManager.GetIPossessible(itemData);

      sb.Append
        (
          $"{item.itemName}"
           .SetSizePercent(1.25f)
           .SetAlign(TextAlign.Center)
        )
       .Append("\n");

      sb.Append
      (
        item.GetDescription()
         .SetAlign(TextAlign.Left)
      );
      
      popupPanel.ShowPopup(sb.ToString());
    }
    
    public string contextMenuName => "$context_menu_cant_duplicate";

    public object[] contextMenuFormats => new object[]
    {
      $"${GameManager.GetIPossessible(itemData).GetPrice() / 2}"
    };

    public bool contextMenuCondition => !string.IsNullOrEmpty(itemData);

    public Action<string> contextMenuFunction => res =>
    {
      switch (res)
      {
        case "sell":
          GameManager.Manager.coin.value += GameManager.GetIPossessible(itemData).GetPrice() / 2;
          SetItem(null);
          break;
      }
    };
  }
}