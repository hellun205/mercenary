using System.Collections.Generic;
using System.Linq;
using Data;
using Manager;
using UnityEngine;
using Util;

namespace Store.Weapon
{
  public class WeaponStore : RefreshableStore<WeaponStoreItem, WeaponItem>
  {
    protected override IEnumerable<WeaponItem> LoadCache()
    {
      var res = new List<WeaponItem>();
      4.For(i => res.AddRange(GameManager.WeaponData.items.Values.Select
      (
        x => new WeaponItem { possessible = x, tier = i }
      )));
      return res;
    }

    protected override WeaponItem RandomGetter(IEnumerable<WeaponItem> cacheList)
    {
      return cacheList.GetRandom();
    }

    protected override bool ConditionGetter(WeaponItem data)
    {
      return items.All(x => x.data.possessible != data.possessible);
    }

    protected override float ProbabilityGetter(WeaponItem data)
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