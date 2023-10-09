using Manager;
using Store.Equipment;
using Window;
using Window.Contents;

namespace Store.Weapon
{
  public class WeaponStoreItem : PurchasableObject<WeaponItem>
  {
    protected override void OnPurchase(WeaponItem item)
    {
      // GameManager.Player.inventory.GainItem(data.possessible.specfiedName, data.tier);
      var weaponInventory = FindObjectOfType<WeaponInventoryUI>();
      var weapon = (item.possessible.specfiedName, item.tier);
      var result = weaponInventory.TryAddWeapon(weapon);

      if (result.canDuplicate && result.canPurchase)
      {
        var askBox = GameManager.Window.Open(WindowType.AskBox).GetContent<AskBox>();
        askBox.window.title = "구매";
        askBox.message = $"{item.displayName}(을)를 다음 티어로 결합 할 수 있습니다.\n구매 후 결합하시겠습니까?";
        askBox.onReturn = res =>
        {
          if (res == AskBoxResult.Yes)
          {
            weaponInventory.DuplicateWeaponForce(weapon);
            SubmitPurchase();
          }
        };
      }
      else if (result.canPurchase)
      {
        SubmitPurchase();
      }
    }
  }
}
