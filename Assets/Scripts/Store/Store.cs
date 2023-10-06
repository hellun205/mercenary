using System.Collections.Generic;
using System.Linq;
using Data;
using Item;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Store
{
  public abstract class Store<T, TData> : MonoBehaviour
    where T : PurchasableObject<TData>
    where TData : IPurchasable
  {
    [SerializeField]
    protected Button refreshBtn;

    private TextMeshProUGUI refreshBtnText;

    protected T[] items { get; private set; }

    public State<int> refreshPrice;
    public int plus { get; private set; }

    private List<TData> cacheData;

    protected abstract IEnumerable<TData> LoadCache();

    protected virtual void Awake()
    {
      refreshBtnText = refreshBtn.GetComponentInChildren<TextMeshProUGUI>();
      cacheData = new List<TData>(LoadCache());
      items = GetComponentsInChildren<T>();
      refreshPrice = new State<int>(2, value => refreshBtnText.text = $"<sprite=0> ${value}");
      refreshBtn.onClick.AddListener(OnRefreshButtonClick);
      GameManager.Wave.onWaveEnd += RefreshItems;
      GameManager.Wave.onWaveStart += OnWaveStart;
      GameManager.Manager.coin.onSet += _ => RefreshInteractable();
      foreach (var item in items)
        item.onDataChanged += RefreshInteractable;
    }

    private void RefreshInteractable()
    {
      if (!GameManager.isLoaded) return;
      refreshBtn.interactable = GameManager.Manager.coin.value >= refreshPrice.value;
      foreach (var item in items)
        item.purchaseButton.interactable = GameManager.Manager.coin.value >= item.price;
    }

    private void OnWaveStart()
    {
      refreshPrice.value = 1;
      plus = GameManager.Data.data.GetIncreaseRefreshPrice(GameManager.Wave.currentWave);
      RefreshInteractable();
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
      for (var i = 0; i < items.Length; i++)
      {
        if (items[i].isLocking) continue;

        TData item;
        var hasValue = false;
        do
        {
          item = RandomGetter(cacheData);
          if (!ConditionGetter(item))
            continue;

          hasValue = ProbabilityGetter(item).ApplyProbability();
        } while (!hasValue);
        
        items[i].data = item;
      }
    }

    protected abstract TData RandomGetter(IEnumerable<TData> cacheList);

    protected abstract bool ConditionGetter(TData data);

    protected abstract float ProbabilityGetter(TData data);
  }
}
