using UnityEngine;
using Random = UnityEngine.Random;

namespace Pool.Extensions
{
  public class SmoothSummoner : MonoBehaviour, IUsePool
  {
    public PoolObject poolObject { get; set; }

    [Range(0.1f, 5f)]
    public float dirRange;

    [Range(1f, 5f)]
    public float speed;

    private bool isEnabled;
    private float curRange;
    private Vector2 dir;

    private void Update()
    {
      if (!isEnabled) return;

      transform.Translate(dir * (Time.deltaTime * curRange));
      curRange = Mathf.Lerp(curRange, 0f, Time.deltaTime * speed);
    }

    public void OnSummon()
    {
      dir = new Vector2(Random.Range(0.1f, 1f), Random.Range(0.1f, 1f)).normalized * dirRange;
      curRange = dirRange;
      isEnabled = true;
    }

    public void OnKilled()
    {
      isEnabled = false;
    }
  }
}
