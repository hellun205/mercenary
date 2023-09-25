using System.Text;
using Item;
using Manager;
using Store.Equipment;
using TMPro;
using UI.DragNDrop;
using UI.Popup;
using UnityEngine;
using UnityEngine.UI;
using Util.Text;
using Weapon;

namespace Store.Inventory
{
  public class InventoryItem : UsePopup<ListPopup>
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
      popupPanel.ShowPopup
      (
        itemData.itemName,
        itemData is WeaponData weapon ? weapon.attribute.GetTexts() : "",
        itemData.description
      );

      var sb = new StringBuilder();

      sb.Append
        (
          itemData.itemName
           .SetSizePercent(1.5f)
           .SetAlign(TextAlign.Center)
        )
       .Append("\n");
      if (itemData is WeaponData weaponData)
      {
        sb.Append
          (
            weaponData.attribute.GetTexts()
             .SetSizePercent(1.25f)
             .AddColor(new Color32(72, 156, 255, 255))
             .SetLineHeight(1.25f)
             .SetAlign(TextAlign.Center)
          )
         .Append("\n");
      }

      sb.Append
      (
        itemData.description
         .SetAlign(TextAlign.Left)
      );

      if (itemData is WeaponData weaponData2)
        popupPanel.ShowPopup(sb.ToString(),
          GameManager.Manager.attributeChemistry.GetDescriptions(weaponData2.attribute));
      else
        popupPanel.ShowPopup(text: sb.ToString());
    }
  }
}
