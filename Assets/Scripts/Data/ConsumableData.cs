using System;
using System.Collections.Generic;
using System.Linq;

namespace Data
{
  public enum ConsumableApplyStatus
  {
    Hp, Regeneration, DrainHp,
    MeleeDamage, RangedDamage, CriticalPercent,
    BleedingDamage, AttackSpeed, Range,
    Armor, EvasionRate, Knockback,
    MoveSpeed, Luck, Price,
    Duration, Resurrection, KillEnemy
  }

  public static class ConsumableDataUtility
  {
    public static string GetMinValue(this ConsumableApplyStatus statusType)
      => statusType switch
      {
        _ => "0"
      };

    public static string GetFieldName(this ConsumableApplyStatus statusType)
      => statusType switch
      {
        ConsumableApplyStatus.Hp => "hp",
        ConsumableApplyStatus.Regeneration => "regeneration",
        ConsumableApplyStatus.DrainHp => "drainHp",
        ConsumableApplyStatus.MeleeDamage => "meleeDamage",
        ConsumableApplyStatus.RangedDamage => "rangedDamage",
        ConsumableApplyStatus.CriticalPercent => "criticalPercent",
        ConsumableApplyStatus.BleedingDamage => "bleedingDamage",
        ConsumableApplyStatus.AttackSpeed => "attackSpeed",
        ConsumableApplyStatus.Range => "fireRange",
        ConsumableApplyStatus.Armor => "armor",
        ConsumableApplyStatus.EvasionRate => "evasionRate",
        ConsumableApplyStatus.Knockback => "knockback",
        ConsumableApplyStatus.MoveSpeed => "moveSpeed",
        ConsumableApplyStatus.Luck => "luck",
        ConsumableApplyStatus.Price => "price",
        ConsumableApplyStatus.Duration => "duration",
        ConsumableApplyStatus.Resurrection => "resurrection",
        ConsumableApplyStatus.KillEnemy => "killEnemy",
        _ => throw new ArgumentOutOfRangeException(nameof(statusType), statusType, null)
      };
  }

  public class ConsumableData
    : IData<ConsumableData, Dictionary<string, Dictionary<ConsumableApplyStatus, string>>>,
      ILoadable
  {
    [Serializable]
    public class Item
    {
      [Serializable]
      public class Apply
      {
        public ConsumableApplyStatus type;
        public string value;
      }

      public string name;
      public Apply[] useStatus;
    }

    public Item[] consumables;

    public Dictionary<string, Dictionary<ConsumableApplyStatus, string>> ToSimply()
      => consumables.ToDictionary
      (
        x => x.name,
        x => x.useStatus.ToDictionary
        (
          y => y.type,
          y => y.value
        )
      );

    public ConsumableData Parse(Dictionary<string, Dictionary<ConsumableApplyStatus, string>> simplyData)
    {
      consumables = simplyData.Select
      (
        x => new Item
        {
          name = x.Key,
          useStatus = x.Value.Select
          (
            y => new Item.Apply
            {
              type = y.Key,
              value = y.Value
            }
          ).ToArray()
        }
      ).ToArray();

      return this;
    }
  }
}
