using UnityEngine;

namespace Map
{
  public class MapManager
  {
    public Transform start;
    public Transform end;

    public void SetMap(Transform startTr, Transform endTr)
    {
      start = startTr;
      end = endTr;
    }

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