using Manager;
using Transition;
using UnityEngine;
using Util;

namespace Map
{
  public class MapLoader : MonoBehaviour
  {
    private Transform start;
    private Transform end;
    private PolygonCollider2D confinerCollider;
    private Transform startPosition;

    private void Awake()
    {
      start = transform.Find("@start");
      end = transform.Find("@end");
      confinerCollider = transform.Find("@confiner").GetComponent<PolygonCollider2D>();
      startPosition = transform.Find("@start_position");
     
    }

    private void Start()
    {
      GameManager.Transition.Play(Transitions.OUT);
      GameManager.Map.SetMap(start, end);
      GameManager.Camera.SetCamera(confinerCollider);
      GameManager.Player.transform.position = startPosition.position;

      CoroutineUtility.WaitUnscaled(1f, () =>
      {
        GameManager.Transition.Play(Transitions.FADEIN, 0.7f);
        GameManager.Wave.StartWave();
      });
    }
  }
}