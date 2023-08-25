using System;
using UnityEngine;

namespace Pool.Extensions
{
  [RequireComponent(typeof(PoolObject))]
  public class Follower : MonoBehaviour
  {
    private PoolObject target;

    public bool isEnabled => target is not null;
    
    private PoolObject po;

    private void Awake()
    {
      po = GetComponent<PoolObject>();
      po.onReleased += () =>
      {
        target = null;
      };
    }

    public void SetTarget(PoolObject obj)
    {
      target = obj;
      target.onReleased += TargetOnReleased;
    }

    private void TargetOnReleased()
    {
      target.onReleased -= TargetOnReleased;
      po.Release();
    }

    private void Update()
    {
      if (!isEnabled) return;

      transform.position = target.position;
    }
  }
}
