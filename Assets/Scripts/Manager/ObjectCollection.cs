using System.Collections.Generic;
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
  }

  public class ObjectCollection : ObjectCollection<GameObject>
  {
    
  }
}