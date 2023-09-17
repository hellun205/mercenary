using System;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Store.Item
{
  public class ItemStore : MonoBehaviour
  {
    private Item[] items;

    public State<int> refreshPrice;
    public int plus = 2;

    private void Awake()
    {
      var container = GameManager.UI.Find("$item_items");
      var refreshBtn = GameManager.UI.Find<Button>("$item_refresh_button");
      var refreshBtnText = GameManager.UI.Find<TextMeshProUGUI>("$item_refresh_button_text");
      
      items = container.GetComponentsInChildren<Item>();
      refreshPrice = new State<int>(2, value => refreshBtnText.text = $"<sprite=0> ${value}");
      refreshBtn.onClick.AddListener(OnRefreshButtonClick);
      GameManager.Wave.onWaveEnd += RefreshItems;
    }

    private void OnRefreshButtonClick()
    {
      if (refreshPrice.value > GameManager.Instance.coin.value) return;

      GameManager.Instance.coin.value -= refreshPrice.value;
      RefreshItems();
      refreshPrice.value += plus;
    }
      
    public void RefreshItems()
    {
      for (var i = 0; i < 3; i++)
      {
        if (items[i].isLocking) continue;
        var item = GameManager.Items.items.Keys.GetRandom();
        
        items[i].SetItem(GameManager.GetItem(item));
      }
    }
  }
}
