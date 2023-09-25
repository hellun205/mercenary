using System;
using System.Collections.Generic;
using System.Linq;
using Manager;
using UnityEngine;

namespace UI
{
  public class UIManager : MonoBehaviour
  {
    public List<GameObject> items;

    private void Awake()
    {
      items = FindObjectsOfType<GameObject>().Where(tmp => tmp.name.Contains('$')).ToList();
    }

    public GameObject Find(string name, Action<GameObject> setter = null)
    {
      var obj = items.Find(tmp => tmp.name == name);

      if (obj == null)
        throw new Exception($"invalid managed ui: \"{name}\"");

      setter?.Invoke(obj);
      return obj;
    }

    public T Find<T>(string name, Action<T> setter = null) where T : UnityEngine.Object
    {
      var obj = items.Find(tmp => tmp.name == name).GetComponent<T>();

      if (obj == null)
        throw new Exception($"invalid managed ui: \"{name}\" (\"{typeof(T).Name}\")");

      setter?.Invoke(obj);
      return obj;
    }

    public GameObject[] FindAll(string name, Action<GameObject> setter = null)
    {
      var objs = items.FindAll(x => x.name == name).ToArray();

      if (!objs.Any())
        throw new Exception($"invalid managed ui: \"{name}\"");
      
      if (setter is not null)
        foreach (var obj in objs)
          setter.Invoke(obj);

      return objs;
    }
    
    public T[] FindAll<T>(string name, Action<T> setter = null) where T : Component
    {
      var objs = items.FindAll(x => x.name == name).Select(x => x.GetComponent<T>()).ToArray();
      
      if (!objs.Any())
        throw new Exception($"invalid managed ui: \"{name}\" (\"{typeof(T).Name}\")");
      
      if (setter is not null)
        foreach (var obj in objs)
          setter.Invoke(obj);

      return objs;
    }
  }
}
