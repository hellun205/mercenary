using Item;
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

namespace Manager
{
  public sealed class GameManager : MonoBehaviourSingleTon<GameManager>
  {
    public static KeyManager Key { get; private set; }
    public static ObjectCollection Weapons { get; private set; }
    public static WeaponDataCollection WeaponData { get; private set; }
    public static ObjectCollection Prefabs { get; private set; }
    public static PlayerController Player { get; private set; }
    public static MapManager Map { get; private set; }
    public static SpawnManager Spawn { get; private set; }
    public static UIManager UI { get; private set; }
    public static PoolManager Pool { get; private set; }
    public static WaveManager Wave { get; private set; }
    public static ScriptableObjectCollection Items { get; private set; }
    public static Status StatusUI { get; private set; }

    public State<int> coin;

    private void Init()
    {
      Key = KeyManager.Instance;
      Weapons = transform.Find("@weapon_objects").GetComponent<ObjectCollection>();
      WeaponData = transform.Find("@weapon_data").GetComponent<WeaponDataCollection>();
      Prefabs = transform.Find("@prefab_objects").GetComponent<ObjectCollection>();
      Player = FindObjectOfType<PlayerController>();
      Map = FindObjectOfType<MapManager>();
      Spawn = FindObjectOfType<SpawnManager>();
      UI = FindObjectOfType<UIManager>();
      Pool = PoolManager.Instance;
      Wave = FindObjectOfType<WaveManager>();
      Items = transform.Find("@item_data").GetComponent<ScriptableObjectCollection>();
      StatusUI = FindObjectOfType<Status>();
    }
    
    protected override void Awake()
    {
      base.Awake();
      Init();
        
      coin = new State<int>(0, v => UI.FindAll<TextMeshProUGUI>("$coin", t => t.text = $"{v}"));;
    }

    public static ItemData GetItem(string itemName)
      => Items.Get(itemName) as ItemData;
  }
}