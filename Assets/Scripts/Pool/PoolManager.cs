using System;
using System.Collections.Generic;
using Manager;
using Scene;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Pool
{
  public sealed class PoolManager : ISceneChangeEventHandler, IBeforeSceneChangeEventHandler
  {
    public PoolObjectCollection objects;

    public Dictionary<string, IObjectPool<PoolObject>> pools;

    public int index;

    private Vector2 tmpPos;

    public Transform parent = GameObject.Find("@summoned_objects").transform;

    public Transform uiParent = GameObject.Find("@summoned_objects_ui").transform;
    
    public bool canGet { get; private set; }

    public PoolManager()
    {
      pools = new Dictionary<string, IObjectPool<PoolObject>>();
      objects = Object.FindObjectOfType<PoolObjectCollection>();
    }

    public PoolObject Summon(string name, Vector2 pos, Action<PoolObject> setter = null)
    {
      if (!canGet) return null;
      CreateObjectPool(name);

      tmpPos = pos;
      var obj = pools[name].Get();
      setter?.Invoke(obj);
      return obj;
    }

    public T Summon<T>(string name, Vector2 pos, Action<T> setter = null) where T : Component
    {
      if (!canGet) return null;
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
      Object.Destroy(obj.gameObject);
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
      obj.name = $"{obj.type} ({obj.index})";
      obj.OnGet();
      obj.gameObject.SetActive(true);
    }

    private PoolObject CreateFunc(string name)
    {
      var o = Get(name);
      PoolObject obj;
      obj = Object.Instantiate(o, o is UIPoolObject ? uiParent : parent);
      obj.position = tmpPos;
      obj.type = name;
      obj.originalName = o.name;

      return obj;
    }

    public void ClearPools()
    {
      foreach (var (name, obj) in pools)
        obj.Clear();
      pools.Clear();

      index = 0;
      
      foreach (var poolObject in Object.FindObjectsOfType<PoolObject>())
      {
        Object.Destroy(poolObject.gameObject);
      }
    }

    public T Get<T>(string path) where T : Object
      => objects[path].GetComponent<T>();

    public PoolObject Get(string path)
      => Get<PoolObject>(path);

    public void OnSceneChanged(string before, string after)
    {
      canGet = true;
    }

    public void OnBeforeSceneChange(string before, string after)
    {
      ClearPools();
      canGet = false;
    }
  }
}
