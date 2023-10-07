using Manager;
using Player.Partner;
using UnityEngine;

namespace Store.Partner
{
  public class PartnerStoreItem : PurchasableObject<PartnerItem>
  {
    protected override void OnPurchase()
    {
      GameManager.Player.inventory.GainItem(data.partner.specfiedName, data.tier);
    }
  }
}
