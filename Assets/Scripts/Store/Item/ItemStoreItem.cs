using Manager;

namespace Store.Item
{
  public class ItemStoreItem : PurchasableObject<ItemItem>
  {
    protected override void OnPurchase(ItemItem item)
    {
      GameManager.Player.inventory.GainItem(data.possessible.specfiedName, data.tier);
      SubmitPurchase();
    }
  }
}
