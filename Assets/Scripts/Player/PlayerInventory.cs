using System;
using System.Collections.Generic;
using Item;
using Manager;
using Store.Inventory;
using UnityEngine;
using UnityEngine.Pool;

namespace Player
{
  public class PlayerInventory : MonoBehaviour
  {
    public Dictionary<(string name, int tier), ushort> items = new();

    public Dictionary<(string name, int tier), InventoryItem> uiItems = new();

    private IObjectPool<InventoryItem> pool;
    private (string item, int tier, ushort count) temp;
    private Transform inventoryItemsParent;

    public event Action onChanged;

    private void Awake()
    {
      pool = new ObjectPool<InventoryItem>(CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy);
      inventoryItemsParent = GameManager.UI.Find("$inventory_items").transform;
      onChanged += () => GameManager.StatusUI.Refresh();
      
      // TODO destroy this code
      GainItem("gold_dust", 0, 3);
    }

    private void ActionOnDestroy(InventoryItem obj)
      => Destroy(obj.gameObject);

    private void ActionOnRelease(InventoryItem obj)
    {
      obj.gameObject.SetActive(false);
    }

    private void ActionOnGet(InventoryItem obj)
    {
      obj.SetItem((temp.item, temp.tier), temp.count);
      obj.transform.SetAsLastSibling();
      obj.gameObject.SetActive(true);
    }

    private InventoryItem CreateFunc()
      => Instantiate(GameManager.Prefabs.Get<InventoryItem>("inventory_item"), inventoryItemsParent);

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
        temp = (item, tier, count);
        uiItems.Add((item, tier), pool.Get());
      }

      onChanged?.Invoke();
    }

    public void LoseItem(string item, int tier, ushort count = 1)
    {
      if (!items.ContainsKey((item, tier))) return;

      if (items[(item, tier)] <= count)
      {
        items.Remove((item, tier));
        pool.Release(uiItems[(item, tier)]);
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
