using System;
using System.Collections.Generic;
using System.Linq;
using Weapon;
using Attribute = Weapon.Attribute;

namespace Data
{
  public enum WeaponStatusItem
  {
    Damage, AttackSpeed, MultipleCritical,
    Range, Knockback, BleedingDamage,
    PenetrateCount, BulletCount, ErrorRange,
    BulletSpeed, ExplosionRange, IncreaseDamageWhenStop,
    IncreaseEvasionRate, IncreaseBleedingDamagePercent, IncreaseMoveSpeedPercent,
    Price
  }

  public enum Weapons
  {
    ballista, bamboo_spear, baseball_bat,
    bow, frying_pan, granade_launcher,
    hand_gun, katana, knife,
    lance, lazer_gun, lazer_sword,
    long_sword, mini_gun, rapier,
    rocket_launcher, shot_gun, sniper_gun,
    spear, submachine_gun
  }

  public static class WeaponDataUtility
  {
    public static float GetMinValue(this WeaponStatusItem statusType)
      => statusType switch
      {
        WeaponStatusItem.BulletSpeed => 7,
        WeaponStatusItem.BulletCount => 1,
        _ => 0,
      };

    public static WeaponType GetWeaponType(this string name)
      => Enum.Parse<Weapons>(name) switch
      {
        Weapons.knife or
          Weapons.bamboo_spear or
          Weapons.baseball_bat or
          Weapons.frying_pan or
          Weapons.lance or
          Weapons.katana or
          Weapons.lazer_sword or
          Weapons.long_sword or
          Weapons.spear or
          Weapons.rapier => WeaponType.Melee,
        Weapons.ballista or
          Weapons.bow or
          Weapons.hand_gun or
          Weapons.lazer_gun or
          Weapons.mini_gun or
          Weapons.shot_gun or
          Weapons.sniper_gun or
          Weapons.submachine_gun or
          Weapons.granade_launcher or
          Weapons.rocket_launcher => WeaponType.Ranged,
        _ => throw new ArgumentOutOfRangeException()
      };
  }

  [Serializable]
  public class WeaponData
    : IData
      <
        WeaponData,
        Dictionary<string, (Attribute attribute, Dictionary<WeaponStatusItem, float>[] tiers)>
      >,
      ILoadable
  {
    [Serializable]
    public class Weapon
    {
      [Serializable]
      public class Tier
      {
        [Serializable]
        public class Apply
        {
          public WeaponStatusItem type;
          public float value;
        }

        public int tier;
        public Apply[] status;
      }

      public string name;
      public Attribute attribute;
      public Tier[] tiers;
    }

    public Weapon[] weapons;

    public Dictionary<string, (Attribute attribute, Dictionary<WeaponStatusItem, float>[] tiers)>
      ToSimply()
      => weapons.ToDictionary
      (
        x => x.name,
        x =>
        (
          x.attribute,
          x.tiers.OrderBy(y => y.tier).Select
          (
            y => y.status.ToDictionary
            (
              z => z.type,
              z => z.value
            )
          ).ToArray()
        )
      );

    public WeaponData Parse
    (
      Dictionary<string, (Attribute attribute, Dictionary<WeaponStatusItem, float>[] tiers)> simplyData
    )
    {
      weapons = simplyData.Select
      (
        x => new Weapon()
        {
          name = x.Key,
          attribute = x.Value.attribute,
          tiers = x.Value.tiers.Select
          (
            (y, i) => new Weapon.Tier
            {
              tier = i,
              status = y.Select
              (
                z => new Weapon.Tier.Apply
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
