using System;
using AYellowpaper.SerializedCollections;
using Item;
using Manager;
using Store.Inventory;
using Store.Status;
using UnityEngine;
using UnityEngine.Pool;

namespace Player
{
  public class PlayerInventory : MonoBehaviour
  {
    [SerializedDictionary("Item", "Count")]
    public SerializedDictionary<IPossessible, ushort> items;

    [SerializedDictionary("Item", "UI")]
    public SerializedDictionary<IPossessible, InventoryItem> uiItems;

    private IObjectPool<InventoryItem> pool;
    private (IPossessible item, ushort count) temp;
    private Transform inventoryItemsParent;

    public event Action onChanged;

    private void Awake()
    {
      pool = new ObjectPool<InventoryItem>(CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy);
      inventoryItemsParent = GameManager.UI.Find("$inventory_items").transform;
      onChanged += () =>  GameManager.StatusUI.Refresh();
    }

    private void ActionOnDestroy(InventoryItem obj)
      => Destroy(obj.gameObject);

    private void ActionOnRelease(InventoryItem obj)
    {
      obj.gameObject.SetActive(false);
    }

    private void ActionOnGet(InventoryItem obj)
    {
      obj.SetItem(temp.item, temp.count);
      obj.gameObject.SetActive(true);
    }

    private InventoryItem CreateFunc()
      => Instantiate(GameManager.Prefabs.Get<InventoryItem>("inventory_item"), inventoryItemsParent);

    public void GainItem(IPossessible item, ushort count = 1)
    {
      if (items.ContainsKey(item))
      {
        items[item] += count;
        uiItems[item].SetCount(items[item]);
      }
      else
      {
        items.Add(item, count);
        temp = (item, count);
        uiItems.Add(item, pool.Get());
      }
      onChanged?.Invoke();
    }

    public void LoseItem(IPossessible item, ushort count = 1)
    {
      if (!items.ContainsKey(item)) return;

      if (items[item] <= count)
      {
        items.Remove(item);
        pool.Release(uiItems[item]);
        uiItems.Remove(item);
      }
      else
      {
        items[item] -= count;
        uiItems[item].SetCount(items[item]);
      }
      onChanged?.Invoke();
    }
    
    
  }
}
