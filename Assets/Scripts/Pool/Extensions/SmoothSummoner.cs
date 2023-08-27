using UnityEngine;
using Random = UnityEngine.Random;

namespace Pool.Extensions
{
  [RequireComponent(typeof(PoolObject))]
  public class SmoothSummoner : MonoBehaviour
  {
    [Range(0.1f, 5f)]
    public float dirRange;

    [Range(1f, 5f)]
    public float speed;
    
    private PoolObject po;

    private bool isEnabled;
    private float curRange;
    private Vector2 dir;

    private void Awake()
    {
      po = GetComponent<PoolObject>();
      po.onGet += () =>
      {
        dir = new Vector2(Random.Range(0.1f, 1f), Random.Range(0.1f, 1f)).normalized * dirRange;
        curRange = dirRange;
        isEnabled = true;
      };
      po.onReleased += () =>
      {
        isEnabled = false;
      };
    }

    private void Update()
    {
      if (!isEnabled) return;

      transform.Translate(dir * (Time.deltaTime * curRange));
      curRange = Mathf.Lerp(curRange, 0f, Time.deltaTime * speed);
    }
  }
}