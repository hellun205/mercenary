using System;
using System.Collections.Generic;
using System.Linq;
using Item;
using Manager;
using Store.Inventory;
using UI;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using Util.UI;

namespace Player
{
  public class PlayerInventory : PoolableWrapper<InventoryItem, InventoryItem>
  {
    public Dictionary<(string name, int tier), ushort> items = new();

    public Dictionary<(string name, int tier), InventoryItem> uiItems = new();

    private InventoryItemType _filtered;

    public InventoryItemType filtered
    {
      get => _filtered;
      set
      {
        _filtered = value;

        void SetVisible(InventoryItem item, bool visible)
        {
          item.SetVisible(visible);
          item.GetComponent<LayoutElement>().ignoreLayout = !visible;
        }

        if (filtered == InventoryItemType.All)
        {
          foreach (var item in uiItems.Values)
            SetVisible(item, true);

          return;
        }

        foreach (var item in uiItems.Values)
          SetVisible(item, item.filterType == filtered);
      }
    }

    public event Action onChanged;

    protected override void Awake()
    {
      base.Awake();
      parent = GameManager.UI.Find("$inventory_items").transform;
      instantiateFunc = () => GameManager.Prefabs.Get<InventoryItem>("inventory_item");
      onChanged += () => GameManager.StatusUI.Refresh();
      GameManager.UI.Find<RadioButtonList>("$inventory_filters").onChanged += OnFilterChanged;
    }

    private void OnFilterChanged(string filterName)
    {
      filtered = Enum.Parse<InventoryItemType>(filterName);
    }

    public void GainItem(string item, int tier, ushort count = 1)
    {
      if (items.ContainsKey((item, tier)))
      {
        items[(item, tier)] += count;
        uiItems[(item, tier)].SetCount(items[(item, tier)]);
      }
      else
      {
        items.Add((item, tier), count);
        var obj = Get();
        obj.component.SetItem((item, tier), count);
        obj.transform.SetAsLastSibling();
        obj.Ready();
        uiItems.Add((item, tier), obj.component);
      }

      onChanged?.Invoke();
    }

    public void LoseItem(string item, int tier, ushort count = 1)
    {
      if (!items.ContainsKey((item, tier))) return;

      if (items[(item, tier)] <= count)
      {
        items.Remove((item, tier));
        Release(uiItems[(item, tier)]);
        uiItems.Remove((item, tier));
      }
      else
      {
        items[(item, tier)] -= count;
        uiItems[(item, tier)].SetCount(items[(item, tier)]);
      }

      onChanged?.Invoke();
    }
  }
}
