using System;
using System.Collections.Generic;
using System.Linq;
using Manager;
using UnityEngine;

namespace UI
{
  public class UIManager : MonoBehaviourSingleTon<UIManager>
  {
    public List<GameObject> items;

    protected override void Awake()
    {
      base.Awake();
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
  }
}