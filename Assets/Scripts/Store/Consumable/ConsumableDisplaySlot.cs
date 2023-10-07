using System.Text;
using Consumable;
using Manager;
using UI.Popup;
using UnityEngine;
using UnityEngine.UI;
using Util.Text;

namespace Store.Consumable
{
  public class ConsumableDisplaySlot : UsePopup<BuffPopupPanel>
  {
    public int GetIndex() => int.Parse(name.Split('.')[1]);

    public override string popupName => "$popup_buff";

    public override void OnEntered()
    {
      var wrapper = GameManager.UI.Find<ConsumableSlotWrapper>("$consumable_wrapper");
      var itemData = wrapper.slots[GetIndex()].itemData;
      
      if (string.IsNullOrEmpty(itemData)) return;
      var sb = new StringBuilder();
      var item = GameManager.GetIPossessible(itemData);

      sb.Append
      (
        item.GetDescription()
         .SetAlign(TextAlign.Left)
      );

      popupPanel.rectTransform.pivot = new Vector2(0.5f, 0.1f);
      popupPanel.ShowPopup(item.itemName, sb.ToString(), item.icon);
    }
  }
}
