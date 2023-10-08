using Consumable;
using Manager;

namespace Store.Consumable
{
  public class ConsumableStoreItem : PurchasableObject<ConsumableItem>
  {
    public override bool isConsume => false;

    protected override void OnPurchase(ConsumableItem item)
    {
      var slots = FindObjectsOfType<ConsumableSlot>();
      
      foreach (var consumableSlot in slots)
      {
        if (string.IsNullOrEmpty(consumableSlot.itemData))
        {
          consumableSlot.SetItem(item.specfiedName);
          SubmitPurchase();
          return;
        }
      }
    }
  }
}
