using System;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Pool
{
  public sealed class PoolManager : SingleTon<PoolManager>
  {
    public Dictionary<string, IObjectPool<PoolObject>> pools;

    public int index;

    private Vector2 tmpPos;

    public Transform parent = (GameObject.Find("@summoned_objects") ?? new GameObject("@summoned_objects")).transform;

    public PoolManager()
    {
      pools = new Dictionary<string, IObjectPool<PoolObject>>();
    }

    public PoolObject Summon(string name, Vector2 pos, Action<PoolObject> setter = null)
    {
      CreateObjectPool(name);

      tmpPos = pos;
      var obj = pools[name].Get();
      setter?.Invoke(obj);
      return obj;
    }

    public T Summon<T>(string name, Vector2 pos, Action<T> setter = null) where T: Component
    {
      var summon = Summon(name, pos, o => setter?.Invoke(o.GetComponent<T>()));
      return summon.GetComponent<T>();
    }

    public void Kill(PoolObject obj) => pools[obj.type].Release(obj);

    private void CreateObjectPool(string name)
    {
      if (pools.ContainsKey(name)) return;

      pools.Add(name, new ObjectPool<PoolObject>
      (
        () => CreateFunc(name),
        ActionOnGet,
        ActionOnRelease,
        ActionOnDestroy
      ));
    }

    private void ActionOnDestroy(PoolObject obj)
    {
      Object.Destroy(obj);
    }

    private void ActionOnRelease(PoolObject obj)
    {
      obj.gameObject.SetActive(false);
      obj.OnReleased();
    }

    private void ActionOnGet(PoolObject obj)
    {
      obj.transform.position = tmpPos;
      obj.index = index++;
      obj.OnGet();
      obj.gameObject.SetActive(true);
    }

    private PoolObject CreateFunc(string name)
    {
      var obj = Object.Instantiate(GameManager.Prefab.Get(name), tmpPos, Quaternion.identity, parent)
        .GetComponent<PoolObject>();
      obj.type = name;
      // obj.index = index++;
      // obj.OnGet();
      return obj;
    }

    public void ClearPools()
    {
      foreach (var (name, obj) in pools)
        obj.Clear();
      pools.Clear();
    }
  }
}