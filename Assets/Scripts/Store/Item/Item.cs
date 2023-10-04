using System;
using Item;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util.UI;
using Weapon;

namespace Store.Item
{
  public class Item : MonoBehaviour
  {
    [SerializeField]
    private TextMeshProUGUI itemName;

    [SerializeField]
    private TextMeshProUGUI attribute;

    [SerializeField]
    private TextMeshProUGUI itemDescriptions;

    [SerializeField]
    private Image itemIcon;

    [SerializeField]
    private Button purchaseButton;

    [SerializeField]
    private Button lockButton;

    [SerializeField]
    private Image lockButtonImage;

    [SerializeField]
    private Sprite lockImage;

    [SerializeField]
    private Sprite unlockImage;

    [SerializeField]
    private Image panel;

    public bool isLocking;

    [SerializeField]
    private TextMeshProUGUI purchaseButtonText;

    public IPossessible itemData;

    public int tier;

    public bool hasItem;

    private void Awake()
    {
      purchaseButton.onClick.AddListener(OnPurchase);
      lockButton.onClick.AddListener(OnLockButtonClick);
      GameManager.Manager.coin.onSet += _ => RefreshButtonEnabled();
    }

    private void OnLockButtonClick()
    {
      SetLock(!isLocking);
    }

    private void OnPurchase()
    {
      if (GameManager.Manager.coin.value < itemData.price) return;

      SetEnabled(false);
      GameManager.Manager.coin.value -= itemData.price;
      GameManager.Player.inventory.GainItem(itemData.specfiedName, 0);
      SetLock(false);
      hasItem = false;
    }

    public void SetLock(bool value)
    {
      isLocking = value;
      lockButtonImage.sprite = isLocking ? lockImage : unlockImage;
    }

    public void SetItem(string name, int tier)
    {
      var item = GameManager.GetIPossessible(name);
      itemData = item;
      this.tier = tier;
      var t = tier switch
      {
        0 => 'Ⅰ',
        1 => 'Ⅱ',
        2 => 'Ⅲ',
        3 => 'Ⅳ',
        _ => throw new ArgumentOutOfRangeException(nameof(tier), tier, null)
      };
      itemName.text = $"{item.itemName} {t}";
      itemDescriptions.text = item.GetDescription(tier);
      itemIcon.sprite = item.icon;
      purchaseButtonText.text = $"${item.price}";
      attribute.text = item is WeaponData weapon ? weapon.attribute.GetTexts() : "";
      hasItem = true;
      panel.color = tier switch
      {
        0 => new Color32(72, 72, 72, 255),
        1 => new Color32(39, 101, 45, 255),
        2 => new Color32(109, 27, 108, 255),
        3 => new Color32(115, 26, 45, 255),
        _ => throw new ArgumentOutOfRangeException()
      };

      RefreshButtonEnabled();
      SetEnabled(true);
    }

    public void SetEnabled(bool value)
    {
      purchaseButton.interactable = value;
      gameObject.SetVisible(value);
    }

    private void RefreshButtonEnabled()
    {
      if (itemData != null)
        purchaseButton.interactable = GameManager.Manager.coin.value >= itemData.price;
    }
  }
}