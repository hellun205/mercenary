using System;
using Manager;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Store.Item
{
  public class ItemStore : MonoBehaviour
  {
    private Item[] items;

    private void Awake()
    {
      var container = GameManager.UI.Find("$item_items");
      var refreshBtn = GameManager.UI.Find<Button>("$item_refresh_button");
      
      items = container.GetComponentsInChildren<Item>();
      refreshBtn.onClick.AddListener(RefreshItems);
      GameManager.Wave.onWaveEnd += RefreshItems;
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
