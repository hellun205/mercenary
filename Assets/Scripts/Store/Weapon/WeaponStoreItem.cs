using Manager;

namespace Store.Weapon
{
  public class WeaponStoreItem : PurchasableObject<WeaponItem>
  {
    protected override void OnPurchase()
    {
      GameManager.Player.inventory.GainItem(data.possessible.specfiedName, data.tier);
    }
  }
}
