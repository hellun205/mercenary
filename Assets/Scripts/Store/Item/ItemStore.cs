using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Item;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Random = UnityEngine.Random;

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
      cacheItems.AddRange(GameManager.WeaponData.items.Values);

      items = container.GetComponentsInChildren<Item>();
      refreshPrice = new State<int>(2, value => refreshBtnText.text = $"<sprite=0> ${value}");
      refreshBtn.onClick.AddListener(OnRefreshButtonClick);
      GameManager.Wave.onWaveEnd += RefreshItems;
    }

    private void OnRefreshButtonClick()
    {
      // if(GameManager.Manager.isTestMode) GameManager.Manager.coin.value = 999999;
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
        int tier;
        var hasValue = false;
        do
        {
          item = cacheItems.GetRandom();
          tier = item.hasTier ? item.tier : Random.Range(0, GameManager.Data.data.store.tierProbabilities.Length);
          if (items.Any(x => x.itemData == item))
            continue;

          var increaseProbability = GameManager.Data.data.GetIncreaseProbabilityByWaveWithoutMultiple(tier);

          hasValue = GameManager.Data.data.GetProbabilityOfPossessibleObject(tier)
            .ApplyPercentage
            (
              increaseProbability * GameManager.Wave.currentWave,
              increaseProbability * Mathf.FloorToInt(GameManager.Player.currentStatus.luck * 100f)
            );
          
          Debug.Log($"[{i}] {item.specfiedName}({tier}) {hasValue}");
        } while (!hasValue);

        items[i].SetItem(item.specfiedName, tier);
      }
    }

    private void Start()
    {
      // RefreshItems();
    }
  }
}