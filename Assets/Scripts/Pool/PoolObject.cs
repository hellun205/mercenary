using System;
using System.Linq;
using Manager;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pool
{
  public class PoolObject : MonoBehaviour
  {
    [Header("Pool Object")]
    public int index;

    public string type;

    public string originalName;

    public virtual Vector2 position
    {
      get => transform.position;
      set => transform.position = value;
    }

    public delegate void PoolObjectEventListener();

    public event PoolObjectEventListener onGet;
    public event PoolObjectEventListener onReleased;

    public void OnGet()
    {
      onGet?.Invoke();
    }

    public void OnReleased()
    {
      onReleased?.Invoke();
    }

    public void Release() => GameManager.Pool.pools[type].Release(this);

    private void Awake()
    {
      var components = GetComponents(typeof(IUsePool));
      if (!components.Any()) return;

      foreach (var component in components)
      {
        var usePool = component as IUsePool;
        usePool.poolObject = this;
        onGet += usePool.OnSummon;
        onReleased += usePool.OnKilled;
      }
    }
  }
}
