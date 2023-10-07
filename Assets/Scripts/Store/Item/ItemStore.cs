using System.Collections.Generic;
using System.Linq;
using Data;
using Item;
using Manager;
using Store.Weapon;
using UnityEngine;
using Util;

namespace Store.Item
{
  public class ItemStore : RefreshableStore<ItemStoreItem, ItemItem>
  {
    protected override IEnumerable<ItemItem> LoadCache()
    {
      var res = new List<ItemItem>();
      res.AddRange(GameManager.Items.items.Values.Cast<IPossessible>().Select
      (
        x => new ItemItem { possessible = x, tier = x.tier }
      ));
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
