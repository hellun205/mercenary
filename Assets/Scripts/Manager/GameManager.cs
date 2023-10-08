using System;
using System.Linq;
using Data;
using Item;
using Map;
using Player;
using Pool;
using Scene;
using Spawn;
using Store.Status;
using TMPro;
using Transition;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;
using Util.UI;
using Wave;
using Weapon;
using Window;
using Window.Contents;
using ItemData = Item.ItemData;
using Object = System.Object;
using PartnerData = Player.Partner.PartnerData;

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
    public static DataManager Data { get; private set; }
    public static WindowManager Window { get; private set; }
    public static SpriteCollection Sprites { get; private set; }
    public static ScriptableObjectCollection Partners { get; private set; }
    public static ScriptableObjectCollection Consumables { get; private set; }

    public static DontDestroyObject CoroutineObject =>
      GameObject.Find("@game").GetComponent<DontDestroyObject>();

    public State<int> coin;

    public static event Action onLoaded;
    public static bool isLoaded { get; private set; }


    public static Color GetTierColor(int tier)
      => tier switch
      {
        1 => new Color32(39, 101, 45, 255),
        2 => new Color32(109, 27, 108, 255),
        3 => new Color32(115, 26, 45, 255),
        _ => new Color32(72, 72, 72, 255),
      };
    public static Color GetAttributeColor()
      => new Color32(72, 156, 255, 255);

    [SerializeField]
    private Sprite m_emptySprite;

    public static Sprite emptySprite => Manager.m_emptySprite;

    public bool isTestMode;

    public DataManager.Input dataJsons;

    public bool isMenuOpened { get; private set; }

    public string startWeaponName { get; set; }

    private void Init()
    {
      Manager = this;
      Data = Manager.isTestMode switch
      {
        true  => new DataManager(),
        false => new DataManager(dataJsons),
      };
      Key ??= new KeyManager();
      Weapons = transform.Find("@weapon_objects").GetComponent<ObjectCollection>();
      WeaponData = transform.Find("@weapon_data").GetComponent<WeaponDataCollection>();
      Prefabs = transform.Find("@prefab_objects").GetComponent<ObjectCollection>();
      Partners = transform.Find("@partner_data").GetComponent<ScriptableObjectCollection>();
      Consumables = transform.Find("@consumable_data").GetComponent<ScriptableObjectCollection>();
      Player = FindObjectOfType<PlayerController>();
      Map = new MapManager();
      Spawn = FindObjectOfType<SpawnManager>();
      UI = FindObjectOfType<UIManager>();
      Pool = new PoolManager();
      Wave = FindObjectOfType<WaveManager>();
      Items = transform.Find("@item_data").GetComponent<ScriptableObjectCollection>();
      StatusUI = FindObjectOfType<Status>();
      Camera = new CameraManager();
      Transition ??= new global::Transition.Transition();
      Window ??= new WindowManager();
      Sprites = transform.Find("@sprites").GetComponent<SpriteCollection>();
    }

    private void Awake()
    {
      Init();
      coin = new State<int>(0, v => UI.FindAll<TextMeshProUGUI>("$coin", t => t.text = $"{v}"));

      new SceneLoader(SceneManager.GetActiveScene().name).HandleSceneChanged();
    }

    private void Start()
    {
      isLoaded = true;
      onLoaded?.Invoke();
      UI.Find<Button>("$menu_btn").onClick.AddListener(ToggleGameMenu);
      UI.Find<Button>("$menu_resume_btn").onClick.AddListener(ToggleGameMenu);
      UI.Find<Button>("$menu_gotomain_btn").onClick.AddListener(AskGotoMain);
      UI.Find<Button>("$menu_shutdown_btn").onClick.AddListener(AskExit);
      UI.Find("$menu_panel").SetVisible(false);

      coin.value = Convert.ToInt32(Data.data.GetPlayerStatusData(PlayerStatusItem.Coin));
    }

    public static IPossessible GetItem(string itemName)
      => Items.Get(itemName) as IPossessible;

    public static IPossessible GetIPossessible(string name)
    {
      IPossessible res = null;

      if (Items.items.TryGetValue(name, out var item))
        res = (IPossessible) item;
      else if (WeaponData.items.TryGetValue(name, out var weapon))
        res = (IPossessible) weapon;
      else if (Partners.items.TryGetValue(name, out var partner))
        res = (IPossessible) partner;
      else if (Consumables.items.TryGetValue(name, out var consumable))
        res = (IPossessible) consumable;

      return res ?? throw new Exception($"can't find IPossessble object: {name}");
    }

    private void Update()
    {
      Key.KeyMap(GetKeyType.Down, (Keys.MenuToggle, ToggleGameMenu));
    }

    private void ToggleGameMenu()
    {
      if (Window.isOpened) return;
      isMenuOpened = !isMenuOpened;
      GameManager.UI.Find("$menu_panel").SetVisible(isMenuOpened, 0.2f);
      if (isMenuOpened) Utils.Pause();
      else Utils.UnPause();
    }

    public void AskGotoMain()
    {
      var askBox = Window.Open(WindowType.AskBox).GetContent<AskBox>();
      askBox.window.title = "메인 화면으로 이동";
      askBox.message = "메인 화면으로 이동하시겠습니까?";
      askBox.onReturn = res =>
      {
        if (res == AskBoxResult.Yes)
        {
          new SceneLoader("Main")
           .Out(Transitions.FADEOUT)
           .In(Transitions.FADEIN, delay: 2f)
           .OnEndOut(() =>
            {
              ToggleGameMenu();
              ExitGame();
            })
           .Load();
        }
      };
    }

    private void ExitGame()
    {
      GameManager.Wave.EndWave(false);
      GameManager.UI.Find("$store").SetVisible(false);
      Player = null;
    }

    public void AskExit()
    {
      var askBox = Window.Open(WindowType.AskBox).GetContent<AskBox>();
      askBox.window.title = "종료";
      askBox.message = "진짜로 종료하시겠습니까?";
      askBox.onReturn = res =>
      {
        if (res == AskBoxResult.Yes) Utils.ExitGame();
      };
    }
  }
}
