using System;
using Item;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util;
using Util.UI;
using Weapon;

namespace Store
{
  public abstract class PurchasableObject<T> : MonoBehaviour where T : IPurchasable
  {
    [SerializeField]
    protected TextMeshProUGUI itemName;

    [SerializeField]
    protected TextMeshProUGUI addtive;

    [SerializeField]
    protected TextMeshProUGUI itemDescriptions;

    [SerializeField]
    protected Image itemIcon;

    public Button purchaseButton;

    [SerializeField]
    protected Button lockButton;

    [SerializeField]
    protected Image lockButtonImage;

    [SerializeField]
    protected Sprite lockImage;

    [SerializeField]
    protected Sprite unlockImage;

    [SerializeField]
    protected Image panel;

    [SerializeField]
    protected TextMeshProUGUI purchaseButtonText;

    public bool isLocking { get; private set; }

    private T _data;

    public T data
    {
      get => _data;
      set
      {
        _data = value;
        itemName.text = data.name;
        itemDescriptions.text = data.description;
        itemIcon.sprite = data.icon;
        purchaseButtonText.text = $"${price}";
        addtive.text = data.addtive;
        hasData = true;
        panel.color = data.color;

        SetEnabled(true);
        onDataChanged?.Invoke();
      }
    }

    public bool hasData { get; private set; }

    public int price => data == null ? -1 : data.price;

    public event Action onDataChanged;

    private void Awake()
    {
      purchaseButton.onClick.AddListener(OnPurchaseButtonClick);
      lockButton.onClick.AddListener(OnLockButtonClick);
    }

    private void OnLockButtonClick()
    {
      SetLock(!isLocking);
    }

    private void OnPurchaseButtonClick()
    {
      if (GameManager.Manager.coin.value < price) return;

      SetEnabled(false);
      GameManager.Manager.coin.value -= price;
      OnPurchase();
      SetLock(false);
      hasData = false;
    }

    protected abstract void OnPurchase();

    public void SetLock(bool value)
    {
      isLocking = value;
      lockButtonImage.sprite = isLocking ? lockImage : unlockImage;
    }

    public void SetEnabled(bool value)
    {
      purchaseButton.interactable = value;
      gameObject.SetVisible(value, 0.1f);
    }
  }
}
