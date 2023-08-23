using Enemy;
using Player;
using Spawn;

namespace Manager
{
  public sealed class GameManager : MonoBehaviourSingleTon<GameManager>, IDontDestroyObject
  {
    public static KeyManager Key { get; private set; }
    public static ObjectCollection Prefab { get; private set; }
    public static PlayerController Player { get; private set; }
    public static MapManager Map { get; private set; }
    public static SpawnManager Spawn { get; private set; }
    public static UIManager UI { get; private set; }

    protected override void Awake()
    {
      base.Awake();

      Key = KeyManager.Instance;
      Prefab = transform.Find("@prefabs").GetComponent<ObjectCollection>();
      Player = FindObjectOfType<PlayerController>();
      Map = FindObjectOfType<MapManager>();
      Spawn = FindObjectOfType<SpawnManager>();
      UI = FindObjectOfType<UIManager>();
    }
  }
}