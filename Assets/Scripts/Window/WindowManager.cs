using System;
using System.Collections.Generic;
using System.Linq;
using Manager;
using UnityEngine;
using Window.Contents;

namespace Window
{
  public class WindowManager
  {
    public Dictionary<WindowType, HashSet<Components.Window>> windows { get; private set; }
    public Canvas canvas { get; private set; }

    public bool isOpened => windows.Any(x => x.Value.Count > 0);

    public WindowManager()
    {
      windows = Enum.GetValues(typeof(WindowType)).OfType<WindowType>().ToDictionary
      (
        x => x,
        _ => new HashSet<Components.Window>()
      );
      GameManager.onLoaded += () => canvas = GameManager.UI.Find<Canvas>("$window_canvas"); 
    }

    public void Close(WindowType type, Components.Window window)
    {
      windows[type].Remove(window);
      UnityEngine.Object.Destroy(window.gameObject);
    }

    public Components.Window Open(WindowType type, Vector2 position = default)
    {
      var prefab = GameManager.Prefabs.Get<Components.Window>($"windows/{type.ToString().ToLower()}");
      var obj = UnityEngine.Object.Instantiate(prefab, canvas.transform);
      ((RectTransform) obj.transform).anchoredPosition = position == default ? Vector2.zero : position;
        
      windows[type].Add(obj);
      return obj;
    }
  }

  public static class WindowUtility
  {
    public static T GetContent<T>(this Components.Window window) where T : IWindowContent
      => (T) window.content;
  }
}
