using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Manager
{
  public class ObjectCollection<T> : MonoBehaviour where T : Object
  {
    public List<T> items;

    public T Get(string itemName)
    {
      return items.Find(item => item.name == itemName);
    }

    public TComponent Get<TComponent>(string itemName) where TComponent : Component
    {
      return Get(itemName).GetComponent<TComponent>();
    }
  }

  public class ObjectCollection : ObjectCollection<GameObject>
  {
    
  }
}