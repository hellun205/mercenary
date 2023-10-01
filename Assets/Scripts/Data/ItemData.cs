using System;
using System.Collections.Generic;
using System.Linq;

namespace Data
{
  public enum ItemStatusItem
  {
    Hp, Regenaration, DrainHp,
    MeleeDamage, RangedDamage, CriticalPercent,
    BleedingDamage, AttackSpeed, Range,
    Armor, KnockBack, MoveSpeed,
    Luck, Price, EvasionRate
  }

  public static class ItemDataUtility
  {
    public static float GetMinValue(this ItemStatusItem statusType)
      => statusType switch
      {
        _ => 0,
      };
  }

  [Serializable]
  public class ItemData
    : IData<ItemData, Dictionary<string, Dictionary<ItemStatusItem, float>[]>>,
      ILoadable
  {
    [Serializable]
    public class Item
    {
      [Serializable]
      public class Tier
      {
        [Serializable]
        public class Apply
        {
          public ItemStatusItem type;
          public float value;
        }

        public int tier;
        public Apply[] status;
      }

      public string name;
      public Tier[] tiers;
    }

    public Item[] items;

    public Dictionary<string, Dictionary<ItemStatusItem, float>[]>
      ToSimply()
      => items.ToDictionary
      (
        x => x.name,
        x =>
          x.tiers.OrderBy(y => y.tier).Select
          (
            y => y.status.ToDictionary
            (
              z => z.type,
              z => z.value
            )
          ).ToArray()
      );

    public ItemData Parse
    (
      Dictionary<string, Dictionary<ItemStatusItem, float>[]> simplyData
    )
    {
      items = simplyData.Select
      (
        x => new Item()
        {
          name = x.Key,
          tiers = x.Value.Select
          (
            (y, i) => new Item.Tier()
            {
              tier = i,
              status = y.Select
              (
                z => new Item.Tier.Apply()
                {
                  type = z.Key,
                  value = z.Value
                }
              ).ToArray()
            }
          ).ToArray()
        }
      ).ToArray();

      return this;
    }
  }
}
