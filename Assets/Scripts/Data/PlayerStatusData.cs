using System;
using System.Collections.Generic;
using System.Linq;

namespace Data
{
  public enum PlayerStatusItem
  {
    MaxHp, Hp, Regeneration,
    DrainHp, MeleeDamage, RangedDamage,
    BleedingDamage, CriticalPercent, AttackSpeed,
    Range, KnockBack, ExplosionRange,
    Armor, MoveSpeed, Luck,
    EvasionRate, CoinDetectRange, Coin
  }

  public static class PlayerStatusItemUtility
  {
    public static float GetMinValue(this PlayerStatusItem statusType)
    {
      return statusType switch
      {
        PlayerStatusItem.MaxHp => 15,
        PlayerStatusItem.Hp    => 15,
        _                      => 0,
      };
    }
  }

  [Serializable]
  public class PlayerStatusData
    : IData<PlayerStatusData, Dictionary<PlayerStatusItem, float>>,
      ILoadable
  {
    [Serializable]
    public class Status
    {
      public PlayerStatusItem type;
      public float value;
    }

    public Status[] status;

    public Dictionary<PlayerStatusItem, float> ToSimply() => status.ToDictionary(x => x.type, x => x.value);

    public PlayerStatusData Parse(Dictionary<PlayerStatusItem, float> simplyData)
    {
      status = simplyData.Select(x => new Status() { type = x.Key, value = x.Value }).ToArray();
      return this;
    }
  }
}
