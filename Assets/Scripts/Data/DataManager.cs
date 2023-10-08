using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Manager;
using Sound;
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
      public string key { get; set; }
      public string store { get; set; }
      public string consumables { get; set; }
      public string volume { get; set; }

      public string GetPath(string type)
      {
        var property = typeof(Paths).GetProperty(type, BindingFlags.Public | BindingFlags.Instance);
        return $"{dir}\\{property?.GetValue(this)}.json";
      }
    }

    public struct Jsons
    {
      public string items { get; set; }
      public string partner { get; set; }
      public string player { get; set; }
      public string weapons { get; set; }
      public string spawns { get; set; }
      public string attributeChemistry { get; set; }
      public string key { get; set; }
      public string store { get; set; }
      public string consumables { get; set; }
      public string volume { get; set; }
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
      public TextAsset key;
      public TextAsset store;
      public TextAsset consumables;
      public TextAsset volume { get; set; }

      public static implicit operator Jsons(Input i)
      {
        var res = new Jsons();

        var fields = i.GetType().GetFields
        (
          BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly
        );

        foreach (var field in fields)
        {
          var f = res.GetType().GetField
          (
            field.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly
          );
          f!.SetValueDirect(__makeref(res), field.GetValue(i));
        }

        return res;
      }
    }

    public struct Data
    {
      public Dictionary<string, (int tier, Dictionary<ItemStatusItem, float> apply)> items { get; set; }

      public Dictionary<string, Dictionary<Attribute, Dictionary<int, Dictionary<PartnerData.Status, string>>>>
        partner { get; set; }

      public Dictionary<PlayerStatusItem, float> player { get; set; }

      public Dictionary<string, (Attribute attribute, Dictionary<WeaponStatusItem, float>[] tiers)> weapons
      {
        get;
        set;
      }

      public SpawnDataSimply spawns { get; set; }

      public Dictionary<Attribute, Dictionary<int, Dictionary<ApplyStatus, float>>> attributeChemistry { get; set; }
      public Dictionary<string, KeyCode[]> keys { get; set; }
      public StoreData store { get; set; }
      public Dictionary<string, Dictionary<ConsumableApplyStatus, string>> consumables { get; set; }
      public Dictionary<SoundType, float> volume { get; set; }
    }

    public Paths paths { get; }
    public Jsons jsons { get; private set; }
    public Data data { get; private set; }

    public DataManager(Jsons? jsonData = null)
    {
      this.paths = new Paths
      {
        dir = Directory.GetCurrentDirectory() + @"\Data",
        spawns = "SpawnData",
        items = "ItemData",
        partner = "PartnerData",
        weapons = "WeaponData",
        player = "PlayerData",
        attributeChemistry = "AttributeChemistryData",
        key = "KeyData",
        store = "StoreData",
        consumables = "ConsumableData",
        volume = "VolumeData"
      };
      if (jsonData != null)
        this.jsons = jsonData.Value;
      Load(jsonData.HasValue);
    }

    private void Load(bool loadWithJson)
    {
      jsons = loadWithJson switch
      {
        false => new Jsons
        {
          items = LoadJson(paths.GetPath("items")),
          partner = LoadJson(paths.GetPath("partner")),
          player = LoadJson(paths.GetPath("player")),
          weapons = LoadJson(paths.GetPath("weapons")),
          spawns = LoadJson(paths.GetPath("spawns")),
          attributeChemistry = LoadJson(paths.GetPath("attributeChemistry")),
          key = LoadJson(paths.GetPath("key")),
          store = LoadJson(paths.GetPath("store")),
          consumables = LoadJson(paths.GetPath("consumables")),
          volume = LoadJson(paths.GetPath("volume"))
        },
        true => jsons
      };

      data = new Data
      {
        items = LoadData<ItemData>(jsons.items).ToSimply(),
        partner = LoadData<PartnerData>(jsons.partner).ToSimply(),
        player = LoadData<PlayerStatusData>(jsons.player).ToSimply(),
        weapons = LoadData<WeaponData>(jsons.weapons).ToSimply(),
        spawns = LoadData<SpawnData>(jsons.spawns).ToSimply(),
        attributeChemistry = LoadData<AttributeChemistryData>(jsons.attributeChemistry).ToSimply(),
        keys = string.IsNullOrEmpty(jsons.key)
          ? KeyManager.InitalDefaultData(paths.GetPath("key"))
          : LoadData<KeyData>(jsons.key).ToSimply(),
        store = LoadData<StoreData>(jsons.store),
        consumables = LoadData<ConsumableData>(jsons.consumables).ToSimply(),
        volume = string.IsNullOrEmpty(jsons.volume)
          ? SoundManager.defaultVolumes
          : LoadData<VolumeData>(jsons.volume).ToSimply()
      };
    }

    public static string LoadJson(string path)
    {
      if (File.Exists(path))
      {
        using var sr = new StreamReader(path);
        return sr.ReadToEnd();
      }
      else
        return null;
    }

    public static T LoadData<T>(string json) where T : ILoadable
    {
      return JsonUtility.FromJson<T>(json);
    }

    public static void SaveData<T>(string path, T obj) where T : ILoadable
    {
      using var sw = new StreamWriter(path);
      sw.Write(JsonUtility.ToJson(obj, true));
    }

    public void SetKeySetting(Dictionary<string, KeyCode?[]> setting)
    {
      var res = data;
      res.keys = setting.ToDictionary(x => x.Key, x => x.Value.Where(y => y.HasValue).Select(y => y.Value).ToArray());
      data = res;
      SaveData(paths.GetPath("key"), new KeyData().Parse(data.keys));
    }
    
    public void SetVolumeSetting(Dictionary<SoundType, float> setting)
    {
      var res = data;
      res.volume = setting;
      data = res;
      SaveData(paths.GetPath("volume"), new VolumeData().Parse(data.volume));
    }
  }
}
