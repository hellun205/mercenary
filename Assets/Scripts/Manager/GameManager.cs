using Player;

namespace Manager
{
  public class GameManager : MonoBehaviourSingleTon<GameManager>, IDontDestroyObject
  {
    public static KeyManager Key { get; private set; }
    public static ObjectCollection Prefab { get; private set; }
    public static PlayerController Player { get; private set; }

    protected override void Awake()
    {
      base.Awake();

      Key = KeyManager.Instance;
      Prefab = transform.Find("@prefabs").GetComponent<ObjectCollection>();
      Player = FindObjectOfType<PlayerController>();
    }
  }
}