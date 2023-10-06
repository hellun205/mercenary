using System;
using System.Collections.Generic;
using Item;
using Manager;
using Store.Inventory;
using UI;
using UnityEngine;
using UnityEngine.Pool;

namespace Player
{
  public class PlayerInventory : PoolableWrapper<InventoryItem, InventoryItem>
  {
    public Dictionary<(string name, int tier), ushort> items = new();

    public Dictionary<(string name, int tier), InventoryItem> uiItems = new();

    public event Action onChanged;

    protected override void Awake()
    {
      base.Awake();
      parent = GameManager.UI.Find("$inventory_items").transform;
      instantiateFunc = () => GameManager.Prefabs.Get<InventoryItem>("inventory_item");
      onChanged += () => GameManager.StatusUI.Refresh();
    }

    private void Start()
    {
      GainItem("cross", -1, 3);
      GainItem("thermonuclear_bomb", -1, 3);
      GainItem("gold_dust", -1, 3);
      GainItem("energy_drink", -1, 3);
      GainItem("boiled_egg", -1, 3);
      GainItem("kiwi", -1, 3);
    }

    // private void ActionOnDestroy(InventoryItem obj)
    //   => Destroy(obj.gameObject);
    //
    // private void ActionOnRelease(InventoryItem obj)
    // {
    //   obj.gameObject.SetActive(false);
    // }
    //
    // private void ActionOnGet(InventoryItem obj)
    // {
    //   
    //   obj.gameObject.SetActive(true);
    // }
    //
    // private InventoryItem CreateFunc()
    //   => Instantiate(, inventoryItemsParent);

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
