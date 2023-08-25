using System;
using UnityEngine;

namespace Pool.Extensions
{
  [RequireComponent(typeof(PoolObject))]
  public class WorldFollower : MonoBehaviour
  {
    public Vector2 position;

    private PoolObject po;

    private bool isEnabled;

    private void Awake()
    {
      po = GetComponent<PoolObject>();
      po.onGet += () =>
      {
        position = transform.position;
        isEnabled = true;
      };

      po.onReleased += () => isEnabled = false;
    }

    private void Update()
    {
      if (!isEnabled) return;
      transform.position = Camera.main.WorldToScreenPoint(position);
    }
  }
}