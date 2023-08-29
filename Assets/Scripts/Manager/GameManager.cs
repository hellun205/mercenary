using Player;
using Pool;
using Spawn;
using UI;
using Wave;
using Weapon;

namespace Manager
{
  public sealed class GameManager : MonoBehaviourSingleTon<GameManager>
  {
    public static KeyManager Key { get; private set; }
    public static ObjectCollection Weapons { get; private set; }
    public static WeaponDataCollection WeaponData { get; private set; }
    public static PlayerController Player { get; private set; }
    public static MapManager Map { get; private set; }
    public static SpawnManager Spawn { get; private set; }
    public static UIManager UI { get; private set; }
    public static PoolManager Pool { get; private set; }
    public static WaveManager Wave { get; private set; }

    protected override void Awake()
    {
      base.Awake();

      Key = KeyManager.Instance;
      Weapons = transform.Find("@weapon_objects").GetComponent<ObjectCollection>();
      WeaponData = transform.Find("@weapon_data").GetComponent<WeaponDataCollection>();
      Player = FindObjectOfType<PlayerController>();
      Map = FindObjectOfType<MapManager>();
      Spawn = FindObjectOfType<SpawnManager>();
      UI = FindObjectOfType<UIManager>();
      Pool = PoolManager.Instance;
      Wave = FindObjectOfType<WaveManager>();
    }
  }
}