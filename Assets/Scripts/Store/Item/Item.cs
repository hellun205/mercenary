using Item;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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

    public bool isLocking;

    [SerializeField]
    private TextMeshProUGUI purchaseButtonText;

    public IPossessible itemData;

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
      itemName.text = item.itemName;
      itemDescriptions.text = item.GetDescription(tier);
      itemIcon.sprite = item.icon;
      purchaseButtonText.text = $"${item.price}";
      attribute.text = item is WeaponData weapon ? weapon.attribute.GetTexts() : ""; 
      hasItem = true;
      
      RefreshButtonEnabled();
      SetEnabled(true);
    }

    public void SetEnabled(bool value)
    {
      purchaseButton.interactable = value;
      gameObject.SetActive(value);
    }

    private void RefreshButtonEnabled()
    {
      if (itemData != null)
        purchaseButton.interactable = GameManager.Manager.coin.value >= itemData.price;
    }
  }
}