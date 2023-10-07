using Consumable;
using Manager;

namespace Store.Consumable
{
  public class ConsumableStoreItem : PurchasableObject<ConsumableItem>
  {
    public override bool isConsume => false;

    protected override void OnPurchase()
    {
      GameManager.Player.inventory.GainItem(data.specfiedName, -1);
    }
  }
}
