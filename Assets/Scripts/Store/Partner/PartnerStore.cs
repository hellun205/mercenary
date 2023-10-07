using System.Collections.Generic;
using System.Linq;
using Data;
using Manager;
using UnityEngine;
using Util;
using PartnerData = Player.Partner.PartnerData;

namespace Store.Partner
{
  public class PartnerStore : RefreshableStore<PartnerStoreItem, PartnerItem>
  {
    protected override IEnumerable<PartnerItem> LoadCache()
    {
      var list = GameManager.Partners.items.Values.Cast<PartnerData>();
      var res = new List<PartnerItem>();

      4.For(i => res.AddRange(list.Select(x => new PartnerItem { partner = x, tier = i })));

      return res;
    }

    protected override PartnerItem RandomGetter(IEnumerable<PartnerItem> cacheList)
      => cacheList.GetRandom();

    protected override bool ConditionGetter(PartnerItem data)
      => items.All(x => !x.data.Equals(data));

    protected override float ProbabilityGetter(PartnerItem data)
    {
      var increaseProbability = GameManager.Data.data.GetIncreaseProbabilityByWaveWithoutMultiple(data.tier);
      return GameManager.Data.data.GetProbabilityOfPossessibleObject(data.tier) +
             increaseProbability * GameManager.Wave.currentWave +
             increaseProbability * Mathf.FloorToInt(GameManager.Player.RefreshStatus().luck * 100f);
    }
  }
}
