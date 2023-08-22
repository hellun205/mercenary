using System;
using UnityEngine;

namespace Manager
{
  public abstract class MonoBehaviourSingleTon<T> : MonoBehaviour where T : MonoBehaviourSingleTon<T>
  {
    public static T Instance { get; private set; }
    
    protected virtual void Awake()
    {
      if (Instance is null)
        Instance = (T)this;
      else
      {
        Destroy(this);
        throw new Exception("already exists SingleTon Object:" + name);
      }
      
      if (this is IDontDestroyObject)
        DontDestroyOnLoad(gameObject);
    }
  }
}