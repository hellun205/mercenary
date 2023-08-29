using System;
using UnityEngine;

namespace Pool.Extensions
{
  [RequireComponent(typeof(PoolObject))]
  public abstract class UsePool : MonoBehaviour
  {
    [NonSerialized]
    public PoolObject po;

    protected virtual void Awake()
    {
      po = GetComponent<PoolObject>();
      po.onGet += OnSummon;
      po.onReleased += OnKilled;
    }

    protected virtual void OnKilled()
    {
    }

    protected virtual void OnSummon()
    {
    }
  }
}
