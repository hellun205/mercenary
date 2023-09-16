﻿using Item;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Store.Item
{
  public class Item : MonoBehaviour
  {
    [SerializeField]
    private TextMeshProUGUI itemName;

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

    private ItemData itemData;

    private void Awake()
    {
      purchaseButton.onClick.AddListener(OnPurchase);
      lockButton.onClick.AddListener(OnLockButtonClick);
      GameManager.Instance.coin.onSet += _ => RefreshButtonEnabled();
    }

    private void OnLockButtonClick()
    {
      SetLock(!isLocking);
    }

    private void OnPurchase()
    {
      if (GameManager.Instance.coin.value < itemData.price) return;

      SetEnabled(false);
      GameManager.Instance.coin.value -= itemData.price;
      GameManager.Player.inventory.GainItem(itemData);
      SetLock(false);
    }

    public void SetLock(bool value)
    {
      isLocking = value;
      lockButtonImage.sprite = isLocking ? lockImage : unlockImage;
    }

    public void SetItem(ItemData item)
    {
      itemData = item;
      itemName.text = item.itemName;
      itemDescriptions.text = item.GetDescription();
      itemIcon.sprite = item.icon;
      purchaseButtonText.text = $"${item.price}";
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
      purchaseButton.interactable = GameManager.Instance.coin.value >= itemData.price;
    }
  }
}