using Manager;

namespace Store.Item
{
  public class ItemStoreItem : PurchasableObject<ItemItem>
  {
    protected override void OnPurchase()
    {
      GameManager.Player.inventory.GainItem(data.possessible.specfiedName, data.tier);
    }
  }
}
