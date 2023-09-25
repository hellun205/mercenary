using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Manager
{
  public enum KeyType
  {
    Down,
    Up,
    Press
  }

  public class KeyManager 
  {
    public Dictionary<string, KeyCode[]> items = new();

    private delegate bool KeyDelegate(KeyCode key);

    public KeyManager()
    {
      items.Add(Keys.PlayerMovementUp, new[] { KeyCode.W, KeyCode.UpArrow });
      items.Add(Keys.PlayerMovementDown, new[] { KeyCode.S, KeyCode.DownArrow });
      items.Add(Keys.PlayerMovementLeft, new[] { KeyCode.A, KeyCode.LeftArrow });
      items.Add(Keys.PlayerMovementRight, new[] { KeyCode.D, KeyCode.RightArrow });
    }

    private KeyDelegate GetKeyFn(KeyType type) => type switch
    {
      KeyType.Down => Input.GetKeyDown,
      KeyType.Up => Input.GetKeyUp,
      KeyType.Press => Input.GetKey,
      _ => throw new Exception("invalid get key type")
    };

    public void KeyMap(KeyType inputType, params (string name, Action fn)[] fns)
    {
      var getKey = GetKeyFn(inputType);
      
      foreach (var (name, fn) in fns)
      {
        if (!items[name].Any(keyCode => getKey.Invoke(keyCode))) continue;
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
  }
}