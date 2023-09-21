using UnityEngine;

namespace Manager
{
  public class MapManager : MonoBehaviour
  {
    public Transform start;
    public Transform end;

    public Vector2 GetRandom()
    {
      return new Vector2
      (
        Random.Range(start.position.x, end.position.x),
        Random.Range(start.position.y, end.position.y)
      );
    }
  }
}
