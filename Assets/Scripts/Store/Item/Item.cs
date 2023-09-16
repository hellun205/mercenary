using Item;
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
    private TextMeshProUGUI purchaseButtonText;

    private ItemData itemData;

    private void Awake()
    {
      purchaseButton.onClick.AddListener(OnPurchase);
      GameManager.Instance.coin.onSet += _ => RefreshButtonEnabled();
    }

    private void OnPurchase()
    {
      if (GameManager.Instance.coin.value < itemData.price) return;

      SetEnabled(false);
      GameManager.Instance.coin.value -= itemData.price;
      GameManager.Player.inventory.GainItem(itemData);
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
