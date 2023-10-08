using Manager;
using Player.Partner;
using Store.Equipment;
using UnityEngine;
using Window;
using Window.Contents;

namespace Store.Partner
{
  public class PartnerStoreItem : PurchasableObject<PartnerItem>
  {
    protected override void OnPurchase(PartnerItem item)
    {
      var partners = FindObjectsOfType<PartnerSlot>();
      var partner = (item.partner.specfiedName, item.tier);
      
      foreach (var partnerSlot in partners)
      {
        if (!partnerSlot.partner.HasValue)
        {
          partnerSlot.SetPartner(partner);
          SubmitPurchase();
          return;
        }
      }
      
      foreach (var partnerSlot in partners)
      {
        if (partnerSlot.partner.HasValue && partnerSlot.partner.Value == partner && partner.tier < 3)
        {
          var askBox = GameManager.Window.Open(WindowType.AskBox).GetContent<AskBox>();
          askBox.window.title = "구매";
          askBox.message = $"{item.displayName}(을)를 다음 티어로 결합 할 수 있습니다.\n구매 후 결합하시겠습니까?";
          askBox.onReturn = res =>
          {
            if (res == AskBoxResult.Yes)
            {
              partnerSlot.SetPartner((partner.specfiedName, partner.tier + 1));
              SubmitPurchase();
            }
          };
          return;
        }
      }
      
    }
  }
}
