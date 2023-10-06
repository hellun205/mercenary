using System;
using UnityEngine;
using UnityEngine.Pool;

namespace UI
{
  public class PoolableWrapper<T> : MonoBehaviour where T : Component
  {
    public IObjectPool<T> pool { get; private set; }
    public Func<T> instantiateFunc { private get; set; }

    private Action<T> tmpOnGet;
    private Action<T> tmpOnRelease;

    protected virtual void Awake()
    {
      pool = new ObjectPool<T>(CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    private void ActionOnDestroy(T obj)
    {
      Destroy(obj.gameObject);
    }

    private void ActionOnRelease(T obj)
    {
      obj.gameObject.SetActive(false);
      tmpOnRelease?.Invoke(obj);
    }

    private void ActionOnGet(T obj)
    {
      tmpOnGet?.Invoke(obj);
      obj.gameObject.SetActive(true);
    }

    private T CreateFunc()
    {
      var obj = Instantiate(instantiateFunc.Invoke(), transform);
      return obj;
    }

    public T Get(Action<T> setter = null)
    {
      tmpOnGet = setter;
      return pool.Get();
    }

    public void Release(T obj, Action<T> setter = null)
    {
      tmpOnRelease = setter;
      pool.Release(obj);
    }
  }
}