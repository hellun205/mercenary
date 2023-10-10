using Consumable;
using Manager;
using Sound;

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
      
      SayMessage($"소모품 슬롯이 부족합니다.");
      GameManager.Sound.Play(SoundType.SFX_UI, "sfx/ui/error");
    }
  }
}
