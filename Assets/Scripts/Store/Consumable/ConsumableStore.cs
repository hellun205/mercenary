using System.Collections.Generic;
using System.Linq;
using Consumable;
using Manager;
using UnityEngine;

namespace Store.Consumable
{
  public class ConsumableStore : Store<ConsumableStoreItem, ConsumableItem>
  {
    protected override IEnumerable<ConsumableItem> LoadCache()
      => GameManager.Consumables.items.Values.Cast<ConsumableItem>();
  }
}
