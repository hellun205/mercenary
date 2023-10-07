using System;
using System.Collections.Generic;
using System.Linq;
using Consumable;
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
    protected T[] items { get; private set; }

    private List<TData> cacheData;

    protected abstract IEnumerable<TData> LoadCache();

    protected virtual void Awake()
    {
      cacheData = new List<TData>(LoadCache());
      items = GetComponentsInChildren<T>();

      GameManager.Wave.onWaveStart += OnWaveStart;
      GameManager.Manager.coin.onSet += _ => RefreshInteractable();
      foreach (var item in items)
        item.onDataChanged += RefreshInteractable;
    }

    private void Start()
    {
      for (var i = 0; i < items.Length; i++)
      {
        items[i].data = cacheData[i];
      }
    }

    private void RefreshInteractable()
    {
      if (!GameManager.isLoaded) return;
      foreach (var item in items)
        item.purchaseButton.interactable = GameManager.Manager.coin.value >= item.price;
    }

    private void OnWaveStart()
    {
      RefreshInteractable();
    }
    
  }
}
