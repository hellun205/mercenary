using Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Store.Inventory
{
  public class InventoryItem : MonoBehaviour
  {
    public ItemData itemData;
    
    [SerializeField]
    private TextMeshProUGUI count;
    
    [SerializeField]
    private Image icon;

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
  }
}
