using System;
using Item;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Util.UI;
using Weapon;

namespace Store.Item
{
  public class Item : PurchasableObject<ItemItem>
  {
    // [SerializeField]
    // private TextMeshProUGUI itemName;
    //
    // [SerializeField]
    // private TextMeshProUGUI attribute;
    //
    // [SerializeField]
    // private TextMeshProUGUI itemDescriptions;
    //
    // [SerializeField]
    // private Image itemIcon;
    //
    // public Button purchaseButton;
    //
    // [SerializeField]
    // private Button lockButton;
    //
    // [SerializeField]
    // private Image lockButtonImage;
    //
    // [SerializeField]
    // private Sprite lockImage;
    //
    // [SerializeField]
    // private Sprite unlockImage;
    //
    // [SerializeField]
    // private Image panel;
    //
    // [SerializeField]
    // private TextMeshProUGUI purchaseButtonText;
    //
    // public bool isLocking { get; private set; }
    //
    // public IPossessible itemData { get; set; }
    //
    // public int tier { get; set; }
    //
    // public bool hasItem { get; set; }
    //
    // public int price => itemData == null ? -1 : itemData.GetPrice(tier);
    //
    // public event Action onItemChanged; 
    //
    // private void Awake()
    // {
    //   purchaseButton.onClick.AddListener(OnPurchase);
    //   lockButton.onClick.AddListener(OnLockButtonClick);
    // }
    //
    // private void OnLockButtonClick()
    // {
    //   SetLock(!isLocking);
    // }
    //
    // private void OnPurchase()
    // {
    //   if (GameManager.Manager.coin.value < price) return;
    //
    //   SetEnabled(false);
    //   GameManager.Manager.coin.value -= price;
    //   SetLock(false);
    //   hasItem = false;
    // }
    //
    // public void SetLock(bool value)
    // {
    //   isLocking = value;
    //   lockButtonImage.sprite = isLocking ? lockImage : unlockImage;
    // }
    //
    // public void SetItem(string name, int tier)
    // {
    //   var item = GameManager.GetIPossessible(name);
    //   itemData = item;
    //   this.tier = tier;
    //   itemName.text = $"{item.itemName} {tier.ToRomanNumeral()}";
    //   itemDescriptions.text = item.GetDescription(tier);
    //   itemIcon.sprite = item.icon;
    //   purchaseButtonText.text = $"${price}";
    //   attribute.text = item is WeaponData weapon ? weapon.attribute.GetTexts() : "";
    //   hasItem = true;
    //   panel.color = GameManager.GetTierColor(tier);
    //
    //   SetEnabled(true);
    //   onItemChanged?.Invoke();
    // }
    //
    // public void SetEnabled(bool value)
    // {
    //   purchaseButton.interactable = value;
    //   gameObject.SetVisible(value);
    // }
    protected override void OnPurchase()
    {
      GameManager.Player.inventory.GainItem(data.possessible.specfiedName, data.tier);
    }
  }
}
