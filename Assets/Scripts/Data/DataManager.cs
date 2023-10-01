using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Weapon;
using Attribute = Weapon.Attribute;

namespace Data
{
  public class DataManager
  {
    public struct Paths
    {
      public string dir { get; set; }
      public string items { get; set; }
      public string partner { get; set; }
      public string player { get; set; }
      public string weapons { get; set; }
      public string spawns { get; set; }
      public string attributeChemistry { get; set; }
    }

    public struct Jsons
    {
      public string items { get; set; }
      public string partner { get; set; }
      public string player { get; set; }
      public string weapons { get; set; }
      public string spawns { get; set; }
      public string attributeChemistry { get; set; }
    }

    [Serializable]
    public struct Input
    {
      public TextAsset items;
      public TextAsset partner;
      public TextAsset player;
      public TextAsset weapons;
      public TextAsset spawns;
      public TextAsset attributeChemistry;

      public static implicit operator Jsons(Input i) => new Jsons()
      {
        items = i.items.text,
        partner = i.partner.text,
        player = i.player.text,
        weapons = i.weapons.text,
        spawns = i.spawns.text,
        attributeChemistry = i.attributeChemistry.text,
      };
    }

    public struct Data
    {
      public Dictionary<string, Dictionary<ItemStatusItem, float>[]> items { get; set; }

      public Dictionary<string, Dictionary<Attribute, Dictionary<int, Dictionary<PartnerData.Status, string>>>>
        partner { get; set; }

      public Dictionary<PlayerStatusItem, float> player { get; set; }

      public Dictionary<string, (Attribute attribute, Dictionary<WeaponStatusItem, float>[] tiers)> weapons { get; set; }

      public SpawnDataSimply spawns { get; set; }

      public Dictionary<Attribute, Dictionary<int, Dictionary<ApplyStatus, float>>> attributeChemistry { get; set; }
    }

    public Paths? paths { get; }
    public Jsons jsons { get; private set; }
    public Data data { get; private set; }

    public DataManager(Paths paths)
    {
      this.paths = paths;
      Load();
    }

    public DataManager(Jsons jsonData)
    {
      this.jsons = jsonData;
      Load();
    }

    private void Load()
    {
      jsons = paths.HasValue switch
      {
        true => new Jsons
        {
          items = LoadJson($"{paths.Value.dir}\\{paths.Value.items}.json"),
          partner = LoadJson($"{paths.Value.dir}\\{paths.Value.partner}.json"),
          player = LoadJson($"{paths.Value.dir}\\{paths.Value.player}.json"),
          weapons = LoadJson($"{paths.Value.dir}\\{paths.Value.weapons}.json"),
          spawns = LoadJson($"{paths.Value.dir}\\{paths.Value.spawns}.json"),
          attributeChemistry = LoadJson($"{paths.Value.dir}\\{paths.Value.attributeChemistry}.json"),
        },
        false => jsons!
      };

      data = new Data
      {
        items = LoadData<ItemData>(jsons.items).ToSimply(),
        partner = LoadData<PartnerData>(jsons.partner).ToSimply(),
        player = LoadData<PlayerStatusData>(jsons.player).ToSimply(),
        weapons = LoadData<WeaponData>(jsons.weapons).ToSimply(),
        spawns = LoadData<SpawnData>(jsons.spawns).ToSimply(),
        attributeChemistry = LoadData<AttributeChemistryData>(jsons.attributeChemistry).ToSimply(),
      };
    }

    public static string LoadJson(string path)
    {
      using var sr = new StreamReader(path);
      return sr.ReadToEnd();
    }

    public static T LoadData<T>(string json) where T : ILoadable
    {
      return JsonUtility.FromJson<T>(json);
    }
  }
}
