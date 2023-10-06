using System.Collections.Generic;
using System.Linq;
using Data;
using Item;
using Manager;
using UnityEngine;
using Util;

namespace Store.Item
{
  public class ItemStore : Store<Item, ItemItem>
  {
    // private Button refreshBtn;
    //
    // private Item[] items;
    //
    // public State<int> refreshPrice;
    // public int plus { get; private set; }
    //
    // private List<IPossessible> cacheItems;
    //
    // private void Awake()
    // {
    //   refreshBtn = GameManager.UI.Find<Button>("$item_refresh_button");
    //   var container = GameManager.UI.Find("$item_items");
    //   var refreshBtnText = GameManager.UI.Find<TextMeshProUGUI>("$item_refresh_button_text");
    //
    //   cacheItems = new List<IPossessible>();
    //   cacheItems.AddRange(GameManager.Items.items.Values.Cast<IPossessible>());
    //   cacheItems.AddRange(GameManager.WeaponData.items.Values);
    //
    //   items = container.GetComponentsInChildren<Item>();
    //   refreshPrice = new State<int>(2, value => refreshBtnText.text = $"<sprite=0> ${value}");
    //   refreshBtn.onClick.AddListener(OnRefreshButtonClick);
    //   GameManager.Wave.onWaveEnd += RefreshItems;
    //   GameManager.Wave.onWaveStart += OnWaveStart;
    //   GameManager.Manager.coin.onSet += _ => RefreshInteractable();
    //   foreach (var item in items)
    //     item.onItemChanged += RefreshInteractable;
    // }
    //
    // private void RefreshInteractable()
    // {
    //   if (!GameManager.isLoaded) return;
    //   refreshBtn.interactable = GameManager.Manager.coin.value >= refreshPrice.value;
    //   foreach (var item in items)
    //     item.purchaseButton.interactable = GameManager.Manager.coin.value >= item.price;
    // }
    //
    // private void OnWaveStart()
    // {
    //   refreshPrice.value = 1;
    //   plus = GameManager.Data.data.GetIncreaseRefreshPrice(GameManager.Wave.currentWave);
    //   RefreshInteractable();
    // }
    //
    // private void OnRefreshButtonClick()
    // {
    //   if (refreshPrice.value > GameManager.Manager.coin.value) return;
    //
    //   GameManager.Manager.coin.value -= refreshPrice.value;
    //   RefreshItems();
    //   refreshPrice.value += plus;
    // }
    //
    // public void RefreshItems()
    // {
    //   items.ForEach(i => i.hasItem = false);
    //
    //   for (var i = 0; i < items.Length; i++)
    //   {
    //     if (items[i].isLocking) continue;
    //
    //     IPossessible item;
    //     int tier;
    //     var hasValue = false;
    //     do
    //     {
    //       item = cacheItems.GetRandom();
    //       tier = item.hasTier ? item.tier : Random.Range(0, GameManager.Data.data.store.tierProbabilities.Length);
    //       if (items.Any(x => x.itemData == item))
    //         continue;
    //
    //       var increaseProbability = GameManager.Data.data.GetIncreaseProbabilityByWaveWithoutMultiple(tier);
    //
    //       hasValue = GameManager.Data.data.GetProbabilityOfPossessibleObject(tier)
    //        .ApplyProbability
    //         (
    //           increaseProbability * GameManager.Wave.currentWave,
    //           increaseProbability * Mathf.FloorToInt(GameManager.Player.GetStatus().luck * 100f)
    //         );
    //     } while (!hasValue);
    //
    //     items[i].SetItem(item.specfiedName, tier);
    //   }
    // }
    //
    // private void Start()
    // {
    //   
    // }


    protected override IEnumerable<ItemItem> LoadCache()
    {
      var res = new List<ItemItem>();
      res.AddRange(GameManager.Items.items.Values.Cast<IPossessible>().Select
      (
        x => new ItemItem { possessible = x, tier = x.tier }
      ));
      4.For(i => res.AddRange(GameManager.WeaponData.items.Values.Select
      (
        x => new ItemItem { possessible = x, tier = i }
      )));
      return res;
    }

    protected override ItemItem RandomGetter(IEnumerable<ItemItem> cacheList)
    {
      return cacheList.GetRandom();
    }

    protected override bool ConditionGetter(ItemItem data)
    {
      return items.All(x => x.data.possessible != data.possessible);
    }

    protected override float ProbabilityGetter(ItemItem data)
    {
      try
      {
        var increaseProbability = GameManager.Data.data.GetIncreaseProbabilityByWaveWithoutMultiple(data.tier);
        return GameManager.Data.data.GetProbabilityOfPossessibleObject(data.tier) +
               increaseProbability * GameManager.Wave.currentWave +
               increaseProbability * Mathf.FloorToInt(GameManager.Player.RefreshStatus().luck * 100f);
      }
      catch
      {
        return 0f;
      }
    }
  }
}