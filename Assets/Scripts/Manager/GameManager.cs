using System;
using System.Collections.Generic;
using System.IO;
using Item;
using Map;
using Player;
using Pool;
using Spawn;
using Store.Status;
using TMPro;
using UI;
using UnityEngine;
using Util;
using Wave;
using Weapon;
using Attribute = Weapon.Attribute;
using Transition = Transition.Transition;

namespace Manager
{
  public sealed class GameManager : MonoBehaviour
  {
    public static KeyManager Key { get; private set; }
    public static ObjectCollection Weapons { get; private set; }
    public static WeaponDataCollection WeaponData { get; private set; }
    public static ObjectCollection Prefabs { get; private set; }
    public static PlayerController Player { get; set; }
    public static MapManager Map { get; private set; }
    public static SpawnManager Spawn { get; private set; }
    public static UIManager UI { get; private set; }
    public static PoolManager Pool { get; private set; }
    public static WaveManager Wave { get; private set; }
    public static ScriptableObjectCollection Items { get; private set; }
    public static Status StatusUI { get; private set; }
    public static GameManager Manager { get; private set; }
    public static CameraManager Camera { get; private set; }
    public static global::Transition.Transition Transition { get; private set; }

    public AttributeChemistry attributeChemistry { get; private set; }
    public Spawn.Spawn spawn { get; private set; }

    public State<int> coin;

    public static event Action onLoaded;

    [SerializeField]
    private Sprite m_emptySprite;

    public static Sprite emptySprite => Manager.m_emptySprite;

    // public AttributeChemistry attributeChemistry;

    public TextAsset attributeChemistryData;
    public TextAsset spawnData;

    public bool isTestMode;

    private void Init()
    {
      Manager = this;
      Key = new KeyManager();
      Weapons = transform.Find("@weapon_objects").GetComponent<ObjectCollection>();
      WeaponData = transform.Find("@weapon_data").GetComponent<WeaponDataCollection>();
      Prefabs = transform.Find("@prefab_objects").GetComponent<ObjectCollection>();
      Player = FindObjectOfType<PlayerController>();
      Map = new MapManager();
      Spawn = FindObjectOfType<SpawnManager>();
      UI = FindObjectOfType<UIManager>();
      Pool = new PoolManager();
      Wave = FindObjectOfType<WaveManager>();
      Items = transform.Find("@item_data").GetComponent<ScriptableObjectCollection>();
      StatusUI = FindObjectOfType<Status>();
      Camera = new CameraManager();
      Transition = new global::Transition.Transition();
    }

    private void Awake()
    {
      Init();
      coin = new State<int>(0, v => UI.FindAll<TextMeshProUGUI>("$coin", t => t.text = $"{v}"));

      string acJson;
      string spawnJson;

      if (isTestMode)
      {
        using (var sr = new StreamReader(Directory.GetCurrentDirectory() + @"\Data\AttributeChemistryData.json"))
        {
          acJson = sr.ReadToEnd();
          sr.Close();
        }
        
        using (var sr = new StreamReader(Directory.GetCurrentDirectory() + @"\Data\SpawnData.json"))
        {
          spawnJson = sr.ReadToEnd();
          sr.Close();
        }
        
      }
      else
      {
        acJson = attributeChemistryData.text;
        spawnJson = spawnData.text;
      }
      
      attributeChemistry = new AttributeChemistry(acJson);
      spawn = new Spawn.Spawn(spawnJson);
      
    }

    private void Start()
    {
      onLoaded?.Invoke();
      coin.value = 999999;
    }

    public static IPossessible GetItem(string itemName)
      => Items.Get(itemName) as IPossessible;
  }
}
