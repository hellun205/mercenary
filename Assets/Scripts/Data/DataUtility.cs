using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enemy;
using Manager;
using Player;
using UnityEngine;
using Util;
using Util.Text;
using Weapon;
using Attribute = Weapon.Attribute;

namespace Data
{
  public static class DataUtility
  {
    private static (Attribute attribute, Dictionary<WeaponStatusItem, float>[] tiers) GetWeaponData
    (
      this DataManager.Data data,
      string weaponName
    )
    {
      return data.weapons.TryGetValue(weaponName, out var value)
        ? value
        : throw new Exception($"Weapon({weaponName}) data does not exist.");
    }

    public static Attribute GetWeaponAttribute(this DataManager.Data data, string weaponName)
    {
      return data.GetWeaponData(weaponName).attribute;
    }

    public static float GetWeaponStatusData
    (
      this DataManager.Data data,
      string weaponName,
      int tier,
      WeaponStatusItem statusType
    )
    {
      var value = data.GetWeaponData(weaponName);
      if (value.tiers.Length <= tier)
        throw new Exception($"$Weapon({weaponName})'s tier({tier}) data does not exist.");
      return value.tiers[tier].TryGetValue(statusType, out var res) ? res : statusType.GetMinValue();
    }

    public static WeaponStatus[] GetWeaponStatus(this DataManager.Data data, string weaponName)
    {
      var weapon = data.GetWeaponData(weaponName);

      float GetValue(int tier, WeaponStatusItem statusType)
      {
        return data.GetWeaponStatusData(weaponName, tier, statusType);
      }

      return weapon.tiers.Select
      (
        (_, i) => new WeaponStatus
        {
          type = weaponName.GetWeaponType(),
          price = Convert.ToInt32(GetValue(i, WeaponStatusItem.Price)),
          attackDamage = GetValue(i, WeaponStatusItem.Damage),
          knockback = GetValue(i, WeaponStatusItem.Knockback),
          attackSpeed = GetValue(i, WeaponStatusItem.AttackSpeed),
          bleedingDamage = GetValue(i, WeaponStatusItem.BleedingDamage),
          fireRange = GetValue(i, WeaponStatusItem.Range),
          multipleCritical = GetValue(i, WeaponStatusItem.MultipleCritical),
          bulletSpeed = GetValue(i, WeaponStatusItem.BulletSpeed),
          bulletCount = Convert.ToInt32(GetValue(i, WeaponStatusItem.BulletCount)),
          penetrate = Convert.ToInt32(GetValue(i, WeaponStatusItem.PenetrateCount)),
          criticalPercent = 0,
          errorRange = Convert.ToInt32(GetValue(i, WeaponStatusItem.ErrorRange)),
          explosionRange = Convert.ToInt32(GetValue(i, WeaponStatusItem.ExplosionRange)),
        }
      ).ToArray();
    }

    private static (int tier, Dictionary<ItemStatusItem, float> apply) GetItemData
    (
      this DataManager.Data data,
      string itemName
    )
    {
      return data.items.TryGetValue(itemName, out var value)
        ? value
        : throw new Exception($"Item({itemName}) data does not exist.");
    }

    public static int GetItemTier(this DataManager.Data data, string itemName)
    {
      var value = data.GetItemData(itemName);
      return value.tier;
    }

    public static float GetItemStatusData(this DataManager.Data data, string itemName, ItemStatusItem statusType)
    {
      var value = data.GetItemData(itemName);
      return value.apply.TryGetValue(statusType, out var res) ? res : statusType.GetMinValue();
    }

    public static int GetItemPrice(this DataManager.Data data, string itemName)
      => Convert.ToInt32(data.GetItemStatusData(itemName, ItemStatusItem.Price));

    public static IncreaseStatus GetItemStatus(this DataManager.Data data, string itemName)
    {
      float GetValue(ItemStatusItem statusType)
      {
        return data.GetItemStatusData(itemName, statusType);
      }

      var res = new IncreaseStatus();

      res.SetValue("attackSpeed", GetValue(ItemStatusItem.AttackSpeed));
      res.SetValue("bleedingDamage", GetValue(ItemStatusItem.BleedingDamage));
      res.SetValue("knockback", GetValue(ItemStatusItem.KnockBack));
      res.SetValue("armor", GetValue(ItemStatusItem.Armor));
      res.SetValue("criticalPercent", GetValue(ItemStatusItem.CriticalPercent));
      res.SetValue("moveSpeed", GetValue(ItemStatusItem.MoveSpeed));
      res.SetValue("range", GetValue(ItemStatusItem.Range));
      res.SetValue("luck", GetValue(ItemStatusItem.Luck));
      res.SetValue("meleeDamage", GetValue(ItemStatusItem.MeleeDamage));
      res.SetValue("rangedDamage", GetValue(ItemStatusItem.RangedDamage));
      res.SetValue("maxHp", GetValue(ItemStatusItem.Hp));
      res.SetValue("regeneration", GetValue(ItemStatusItem.Regenaration));
      res.SetValue("drainHp", GetValue(ItemStatusItem.DrainHp));
      res.SetValue("evasionRate", GetValue(ItemStatusItem.EvasionRate));

      return res;
    }

    private static SpawnDataSimply.Enemy GetEnemyData(this DataManager.Data data, string enemyName)
    {
      return data.spawns.enemies.TryGetValue(enemyName, out var value)
        ? value
        : throw new Exception($"Enemy({enemyName}) data does not exist.");
    }

    public static EnemyStatus GetEnemyStatus(this DataManager.Data data, string enemyName, int wave)
    {
      var enemy = data.GetEnemyData(enemyName);
      var def = enemy.defaultStatus;
      var inc = enemy.increaseStatus;

      float? GetDetailValue(SpawnData.Enemy.Detail.StatusTypes statusType)
      {
        return enemy.detailStatus.TryGetValue(statusType, out var value) ? value : null;
      }

      var res = new EnemyStatus
      {
        maxHp = def.hp,
        damage = def.damage,
        moveSpeed = def.moveSpeed,
        drop = Convert.ToInt32(def.dropCoin),
        attackSpeed = GetDetailValue(SpawnData.Enemy.Detail.StatusTypes.AttackSpeed) ?? 1f,
        bulletSpeed = GetDetailValue(SpawnData.Enemy.Detail.StatusTypes.BulletSpeed) ?? 1f,
        fireRange = GetDetailValue(SpawnData.Enemy.Detail.StatusTypes.Range) ?? 30f,
        increaseMoveSpeedPerSecond =
          GetDetailValue(SpawnData.Enemy.Detail.StatusTypes.IncreaseMoveSpeedPerSecond) ?? 0f,
      };

      wave.For(_ =>
      {
        res.maxHp += inc.hp;
        res.damage += inc.damage;
        res.moveSpeed += inc.moveSpeed;
        res.drop += Convert.ToInt32(inc.dropCoin);
      });

      res.maxMoveSpeed = GetDetailValue(SpawnData.Enemy.Detail.StatusTypes.MaxMoveSpeed) ?? res.moveSpeed;
      return res;
    }

    private static Dictionary<int, Dictionary<ApplyStatus, float>> GetAttributeChemitryData
    (
      this DataManager.Data data, Attribute attribute
    )
    {
      return data.attributeChemistry.TryGetValue(attribute, out var value)
        ? value
        : throw new Exception($"Attribute({attribute}) data does not exist.");
    }

    public static IncreaseStatus GetAttributeChemistryIncrease
    (
      this DataManager.Data data,
      Attribute attribute,
      int count
    )
    {
      var attr = data.GetAttributeChemitryData(attribute);
      var c = attr.Keys.LastOrDefault(x => x <= count);

      if (c == default)
        return new IncreaseStatus();
      else
      {
        var res = new IncreaseStatus();

        foreach (var (status, value) in attr[c])
        {
          switch (status)
          {
            case ApplyStatus.Armor:
              res.armor += value;
              break;

            case ApplyStatus.Knockback:
              res.knockback += value;
              break;

            case ApplyStatus.Range:
              res.range += value;
              break;

            case ApplyStatus.AttackSpeed:
              res.attackSpeed += value;
              break;

            case ApplyStatus.CriticalPercent:
              res.criticalPercent += value;
              break;

            case ApplyStatus.RangedDamage:
              res.rangedDamage += value;
              break;

            case ApplyStatus.MeleeDamage:
              res.meleeDamage += value;
              break;

            case ApplyStatus.BleedingDamage:
              res.bleedingDamage += value;
              break;

            case ApplyStatus.ExplosionRange:
              res.explosionRange += value;
              break;

            default:
              throw new ArgumentOutOfRangeException();
          }
        }

        return res;
      }
    }

    public static string GetAttributeChemistryDescription(this DataManager.Data data, Attribute attribute)
    {
      var sb = new StringBuilder();
      var attr = data.GetAttributeChemitryData(attribute);
      GameManager.Player.GetChemistryStatus(out var counts);

      var i = counts.ContainsKey(attribute) ? attr.Keys.LastOrDefault(x => x <= counts[attribute]) : 0;

      sb.Append
        (
          attribute.GetText()
           .SetSizePercent(1.25f)
           .AddColor(new Color32(72, 156, 255, 255))
           .SetLineHeight(1.3f)
        )
       .Append("\n");

      foreach (var (count, value) in attr)
      {
        var sb2 = new StringBuilder();

        foreach (var (status, f) in value)
          sb2.Append(status.GetText())
           .Append(' ')
           .Append(status.GetValue(f).GetViaValue())
           .Append("\n");

        sb2.Remove(sb2.ToString().LastIndexOf("\n", StringComparison.Ordinal), 1);
        sb.Append
          (
            $"({count}){sb2.ToString().SetIndent(0.2f)}"
             .AddColor(i != default && i == count ? Color.white : Color.gray)
          )
         .Append("\n");
      }

      return sb.ToString();
    }

    public static string[] GetAttributeChemistryDescriptions(this DataManager.Data data, Attribute attributeFlags)
    {
      var res = new List<string>();

      foreach (var attribute in attributeFlags.GetFlags())
      {
        if (attribute == 0) continue;
        res.Add(data.GetAttributeChemistryDescription(attribute));
      }

      return res.ToArray();
    }

    public static float GetWaveTime(this DataManager.Data data, int wave)
    {
      if (data.spawns.waves.Length <= wave)
        throw new Exception($"Wave time(wave: {wave}) data does not exist. (count)");
      return data.spawns.waves[wave];
    }

    public static SpawnData.Spawns.Spawn[] GetSpawnData(this DataManager.Data data, int wave)
    {
      if (data.spawns.waves.Length <= wave)
        throw new Exception($"Wave time(wave: {wave}) data does not exist. (count)");
      return data.spawns.spawns[wave].Select(x => new SpawnData.Spawns.Spawn
        {
          name = x.Key,
          count = x.Value.count,
          range = x.Value.range,
          delay = x.Value.delay,
          simultaneousSpawnCount = x.Value.simultaneousSpawnCount
        })
       .ToArray();
    }

    public static float GetPlayerStatusData(this DataManager.Data data, PlayerStatusItem statusType)
    {
      return data.player.TryGetValue(statusType, out var value) ? value : statusType.GetMinValue();
    }

    public static PlayerStatus GetPlayerStatus(this DataManager.Data data)
    {
      return new PlayerStatus
      {
        maxHp = data.GetPlayerStatusData(PlayerStatusItem.MaxHp),
        hp = data.GetPlayerStatusData(PlayerStatusItem.Hp),
        knockback = data.GetPlayerStatusData(PlayerStatusItem.KnockBack),
        bleedingDamage = data.GetPlayerStatusData(PlayerStatusItem.BleedingDamage),
        attackSpeed = data.GetPlayerStatusData(PlayerStatusItem.AttackSpeed),
        meleeDamage = data.GetPlayerStatusData(PlayerStatusItem.MeleeDamage),
        moveSpeed = data.GetPlayerStatusData(PlayerStatusItem.MoveSpeed),
        explosionRange = data.GetPlayerStatusData(PlayerStatusItem.ExplosionRange),
        regeneration = data.GetPlayerStatusData(PlayerStatusItem.Regeneration),
        rangedDamage = data.GetPlayerStatusData(PlayerStatusItem.RangedDamage),
        criticalPercent = data.GetPlayerStatusData(PlayerStatusItem.CriticalPercent),
        armor = data.GetPlayerStatusData(PlayerStatusItem.Armor),
        luck = data.GetPlayerStatusData(PlayerStatusItem.Luck),
        range = data.GetPlayerStatusData(PlayerStatusItem.Range),
        drainHp = data.GetPlayerStatusData(PlayerStatusItem.DrainHp),
        evasionRate = data.GetPlayerStatusData(PlayerStatusItem.EvasionRate)
      };
    }

    public static int GetIncreaseRefreshPrice(this DataManager.Data data, int wave)
    {
      if (data.store.refreshPrices.Length <= wave)
        throw new Exception($"Increase Refresh Price (wave: {wave}) data does not exist.");

      return data.store.refreshPrices[wave];
    }

    public static float GetProbabilityOfPossessibleObject(this DataManager.Data data, int tier)
    {
      if (data.store.tierProbabilities.Length <= tier)
        throw new Exception($"Probability of Possessible Object (tier: {tier}) data does not exist.");

      return data.store.tierProbabilities[tier];
    }

    public static float GetIncreaseProbabilityByWaveWithoutMultiple(this DataManager.Data data, int tier)
    {
      if (data.store.additionalProbabilitiesOfWaves.Length <= tier)
        throw new Exception($"Probability of Possessible Object (tier: {tier}) data does not exist.");

      return data.store.additionalProbabilitiesOfWaves[tier];
    }

    public static float GetIncreaseProbabilityByWave(this DataManager.Data data, int tier, int wave)
    {
      return data.GetIncreaseProbabilityByWaveWithoutMultiple(tier) * wave;
    }

    public static (Attribute attr, Dictionary<int, Dictionary<PartnerData.Status, string>> tiers) GetPartnerData
    (
      this DataManager.Data data, string partnerName
    )
    {
      if (!data.partner.TryGetValue(partnerName, out var value))
        throw new Exception($"Partner({partnerName}) data does not exist.");

      var f = value.First();
      return (f.Key, f.Value);
    }

    public static string GetPartnerStatusData
    (
      this DataManager.Data data, string partnerName, int tier, PartnerData.Status statusType
    )
    {
      var partner = data.GetPartnerData(partnerName);
      if (!partner.tiers.TryGetValue(tier, out var status))
        throw new Exception($"Partner(tier:{tier}) data does not exist.");

      return status.TryGetValue(statusType, out var value) ? value : statusType.GetMinValue();
    }

    public static Attribute GetPartnerAttribute(this DataManager.Data data, string partnerName)
    {
      return data.GetPartnerData(partnerName).attr;
    }

    public static IncreaseStatus GetPartnerStatus(this DataManager.Data data, string partnerName, int tier)
    {
      string GetValue(PartnerData.Status status)
      {
        return data.GetPartnerStatusData(partnerName, tier, status);
      }

      return new IncreaseStatus
      {
        attackSpeed = GetValue(PartnerData.Status.AttackSpeed),
        armor = "0",
        knockback = GetValue(PartnerData.Status.Knockback),
        bleedingDamage = GetValue(PartnerData.Status.BleedingDamage),
        luck = "0",
        regeneration = "0",
        criticalPercent = GetValue(PartnerData.Status.CriticalPercent),
        explosionRange = GetValue(PartnerData.Status.ExplosionRange),
        meleeDamage = GetValue(PartnerData.Status.Damage),
        evasionRate = "0",
        moveSpeed = "0",
        rangedDamage = GetValue(PartnerData.Status.Damage),
        range = GetValue(PartnerData.Status.Range),
        drainHp ="0",
        maxHp = "0",
      };
    }
  }
}
