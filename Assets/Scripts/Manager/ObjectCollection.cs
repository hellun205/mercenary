using System;
using AYellowpaper.SerializedCollections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Manager
{
  public class ObjectCollection<T> : MonoBehaviour where T : Object
  {
#if UNITY_EDITOR
    [Header("Find Assets")]
    [SerializeField]
    protected string root;

    [SerializeField]
    protected string exts = "prefab";

    [SerializeField]
    protected bool isPath = true;
#endif

    [SerializedDictionary("path", "object")]
    public SerializedDictionary<string, T> items;

    public T Get(string itemName) => this[itemName];

    public T this[string path]
    {
      get
      {
        if (!items.ContainsKey(path))
          throw new Exception($"invalid path of summonable object: \"{path}\"");
        return items[path];
      }
    }

    public TComponent Get<TComponent>(string itemName) where TComponent : Component
    {
      return Get(itemName).GetComponent<TComponent>();
    }

#if UNITY_EDITOR
    protected void FindObjectsAndAddToList()
    {
      items.Clear();

      var guids = AssetDatabase.FindAssets("t:Object", new[] { $"Assets/{root}" });

      foreach (var guid in guids)
      {
        var path = AssetDatabase.GUIDToAssetPath(guid);
        if (string.IsNullOrEmpty(exts))
        {
          var asset = (T) AssetDatabase.LoadAssetAtPath(path, typeof(T));
          if (asset is not T) continue;
          var key = isPath ? path.Replace($"Assets/{root}/", "").Split('.')[0] : asset.name;
          items.Add(key, asset);
        }
        else if (path.Contains($".{exts}"))
        {
          var asset = (T) AssetDatabase.LoadAssetAtPath(path, typeof(T));
          var key = isPath ? path.Replace($"Assets/{root}/", "").Replace($".{exts}", "").ToLower() : asset.name;
          items.Add(key, asset);
        }
      }
    }
#endif
  }

  public class ObjectCollection : ObjectCollection<GameObject>
  {
#if UNITY_EDITOR
    [ContextMenu("Apply Assets")]
    private void ApplyAsset() => FindObjectsAndAddToList();
#endif
  }
}
