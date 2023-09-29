using System;
using System.Collections.Generic;
using System.Linq;
using Item;
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

    private List<IPossessible> cacheItems;

    private void Awake()
    {
      var container = GameManager.UI.Find("$item_items");
      var refreshBtn = GameManager.UI.Find<Button>("$item_refresh_button");
      var refreshBtnText = GameManager.UI.Find<TextMeshProUGUI>("$item_refresh_button_text");

      cacheItems = new List<IPossessible>();
      cacheItems.AddRange(GameManager.Items.items.Values.Cast<IPossessible>());
      cacheItems.AddRange(GameManager.WeaponData.items.Values.Select(x => x.weapons[0]));
      
      items = container.GetComponentsInChildren<Item>();
      refreshPrice = new State<int>(2, value => refreshBtnText.text = $"<sprite=0> ${value}");
      refreshBtn.onClick.AddListener(OnRefreshButtonClick);
      GameManager.Wave.onWaveEnd += RefreshItems;
    }

    private void OnRefreshButtonClick()
    {
      if (refreshPrice.value > GameManager.Manager.coin.value) return;

      GameManager.Manager.coin.value -= refreshPrice.value;
      RefreshItems();
      refreshPrice.value += plus;
    }

    public void RefreshItems()
    {
      items.ForEach(i => i.hasItem = false);
      
      for (var i = 0; i < items.Length; i++)
      {
        if (items[i].isLocking) continue;

        IPossessible item;
        do
        {
          item = cacheItems.GetRandom();
        } while (items.Any(x => x.itemData == item));

        items[i].SetItem(item);
      }
    }

    private void Start()
    {
      RefreshItems();
    }
  }
}
