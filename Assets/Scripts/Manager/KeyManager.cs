using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data;
using UnityEngine;

namespace Manager
{
  public enum GetKeyType
  {
    Down, Up, Press
  }

  public class KeyManager
  {
    private delegate bool KeyDelegate(KeyCode key);

    public static Dictionary<string, KeyCode[]> InitalDefaultData(string path)
    {
      var res = new Dictionary<string, KeyCode[]>()
      {
        { Keys.PlayerMovementUp, new[] { KeyCode.W, KeyCode.UpArrow } },
        { Keys.PlayerMovementDown, new[] { KeyCode.S, KeyCode.DownArrow } },
        { Keys.PlayerMovementLeft, new[] { KeyCode.A, KeyCode.LeftArrow } },
        { Keys.PlayerMovementRight, new[] { KeyCode.D, KeyCode.RightArrow } },
        { Keys.MenuToggle, new[] { KeyCode.Escape } }
      };

      using var sw = new StreamWriter(path);
      sw.Write(JsonUtility.ToJson(new KeyData().Parse(res), true));

      return res;
    }

    private KeyDelegate GetKeyFn(GetKeyType type) => type switch
    {
      GetKeyType.Down  => Input.GetKeyDown,
      GetKeyType.Up    => Input.GetKeyUp,
      GetKeyType.Press => Input.GetKey,
      _                => throw new Exception("invalid get key type")
    };

    public void KeyMap(GetKeyType inputType, params (string name, Action fn)[] fns)
    {
      var getKey = GetKeyFn(inputType);

      foreach (var (name, fn) in fns)
      {
        if (!GameManager.Data.data.keys[name].Any(keyCode => getKey.Invoke(keyCode))) continue;
        fn.Invoke();
      }
    }
  }

  public static class Keys
  {
    public const string PlayerMovementUp = "player_movement_up";
    public const string PlayerMovementDown = "player_movement_down";
    public const string PlayerMovementRight = "player_movement_right";
    public const string PlayerMovementLeft = "player_movement_left";
    public const string MenuToggle = "menu_toggle";
  }
}
