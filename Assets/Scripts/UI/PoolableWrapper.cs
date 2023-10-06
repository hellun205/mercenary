using System;
using UnityEngine;
using UnityEngine.Pool;

namespace UI
{
  public class PoolableWrapper<T, T2> : MonoBehaviour where T : class, IPoolableUI<T2> where T2 : Component
  {
    public IObjectPool<T> pool { get; private set; }
    public Func<T> instantiateFunc { private get; set; }
    public Transform parent { get; protected set; }

    protected virtual void Awake()
    {
      pool = new ObjectPool<T>(CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy);
      parent = transform;
    }

    private void ActionOnDestroy(T obj)
    {
      Destroy(obj.gameObject);
    }

    private void ActionOnRelease(T obj)
    {
      obj.gameObject.SetActive(false);
    }

    private void ActionOnGet(T obj)
    {
      // obj.gameObject.SetActive(true);
    }

    private T CreateFunc()
    {
      var obj = Instantiate(instantiateFunc.Invoke().gameObject, parent);
      return obj.GetComponent<T>();
    }

    public IPoolableUI<T2> Get()
    {
      var obj = pool.Get();
      obj.gameObject.SetActive(true);
      return obj;
    }

    public void Release(T obj)
    {
      obj.gameObject.SetActive(false);
      pool.Release(obj);
    }
  }
}
