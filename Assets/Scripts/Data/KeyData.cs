using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data
{
  [Serializable]
  public class KeyData : IData<KeyData, Dictionary<string, KeyCode[]>>, ILoadable
  {
    [Serializable]
    public class Key
    {
      public string name;
      public KeyCode[] key;
    }

    public Key[] keys;

    public Dictionary<string, KeyCode[]> ToSimply() => keys.ToDictionary(x => x.name, x => x.key);

    public KeyData Parse(Dictionary<string, KeyCode[]> simplyData)
    {
      keys = simplyData.Select(x => new Key { name = x.Key, key = x.Value }).ToArray();
      return this;
    }
  }
}
