namespace Manager
{
  public class GameManager : MonoBehaviourSingleTon<GameManager>, IDontDestroyObject
  {
    public static KeyManager Key { get; private set; }

    protected override void Awake()
    {
      base.Awake();

      Key = KeyManager.Instance;
    }
  }
}