using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Manager;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Util
{
  public static class Utils
  {
    public static float GetAngleOfLookAtObject(this Transform sender, Transform target)
    {
      var offset = target.transform.position - sender.position;
      return Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
    }

    public static Quaternion GetRotationOfLookAtObject(this Transform sender, Transform target)
    {
      var euler = sender.rotation.eulerAngles;
      return Quaternion.Euler(euler.x, euler.y, GetAngleOfLookAtObject(sender, target));
    }

    public static T GetRandom<T>(this IEnumerable<T> enumerable)
    {
      var array = enumerable.ToArray();

      return array[Random.Range(0, array.Length)];
    }

    public static Color Setter(this Color original, float? r = null, float? g = null, float? b = null, float? a = null)
    {
      return new Color(r ?? original.r, g ?? original.g, b ?? original.b, a ?? original.a);
    }

    public static Vector3 Setter(this Vector3 original, float? x = null, float? y = null, float? z = null)
    {
      return new Vector3(x ?? original.x, y ?? original.y, z ?? original.z);
    }

    public static Vector2 Setter(this Vector2 original, float? x = null, float? y = null)
    {
      return new Vector2(x ?? original.x, y ?? original.y);
    }

    public static Vector3 WorldToScreenSpace(this RectTransform canvas, Vector3 worldPos)
    {
      var screenPoint = Camera.main.WorldToScreenPoint(worldPos);
      screenPoint.z = 0;

      if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenPoint, Camera.main, out var screenPos))
        return screenPos;

      return screenPoint;
    }

    public static Vector3 WorldToCanvasPosition(this Canvas canvas, Vector3 worldPosition, Camera camera = null)
    {
      if (camera == null)
      {
        camera = GameManager.Camera.camera;
      }

      var viewportPosition = camera.WorldToViewportPoint(worldPosition);
      return canvas.ViewportToCanvasPosition(viewportPosition);
    }

    public static Vector3 ScreenToCanvasPosition(this Canvas canvas, Vector3 screenPosition)
    {
      var viewportPosition = new Vector3(screenPosition.x / Screen.width,
        screenPosition.y / Screen.height,
        0);
      return canvas.ViewportToCanvasPosition(viewportPosition);
    }

    public static Vector3 ViewportToCanvasPosition(this Canvas canvas, Vector3 viewportPosition)
    {
      var centerBasedViewPortPosition = viewportPosition - new Vector3(0.5f, 0.5f, 0);
      var canvasRect = canvas.GetComponent<RectTransform>();
      var scale = canvasRect.sizeDelta;
      return Vector3.Scale(centerBasedViewPortPosition, scale);
    }

    public static bool ApproximatelyEqual(this float a, float b, float errorRange = 0.3f)
      => Mathf.Abs(a - b) < errorRange;

    public static Vector2 GetAroundRandom(this Transform main, float radius)
    {
      var p = main.position;
      var min = new Vector2(p.x - radius, p.y - radius);
      var max = new Vector2(p.x + radius, p.y + radius);
      return new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
    }

    public static bool ApplyProbability(this float probability, params float[] additive)
    {
      return ApplyProbability(probability, out var _, additive);
    }

    public static bool ApplyProbability(this float probability, out float randomValue, params float[] additive)
    {
      return ApplyProbability(probability, out randomValue, out var _, additive);
    }

    public static bool ApplyProbability
    (
      this float probability,
      out float randomValue,
      out float finalProbability,
      params float[] additive
    )
    {
      randomValue = Random.Range(0f, 1f);
      finalProbability = probability + additive.Sum();
      return Mathf.Min(1f, Mathf.Max(0f, finalProbability)) >= randomValue;
    }

    public static void ExitGame()
    {
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }

    private static TweenerCore<float, float, FloatOptions> timeScaleTweener;

    public static void Pause(bool smooth = false, float duration = 1f)
    {
      timeScaleTweener.Kill();

      if (smooth)
        timeScaleTweener = DOTween.To(() => Time.timeScale, v => Time.timeScale = v, 0f, duration);
      else
        Time.timeScale = 0f;
    }

    public static void UnPause(bool smooth = false, float duration = 1f)
    {
      timeScaleTweener.Kill();
      if (smooth)
        timeScaleTweener = DOTween.To(() => Time.timeScale, v => Time.timeScale = v, 1f, duration);
      else
        Time.timeScale = 1f;
    }

    public static IEnumerable<Enum> GetFlags(this Enum input)
    {
      return Enum.GetValues(input.GetType()).Cast<Enum>().Where(value => input.HasFlag(value));
    }

    public static IEnumerable<T> GetFlags<T>(this T input) where T : Enum
    {
      return Enum.GetValues(input.GetType()).Cast<T>().Where(value => input.HasFlag(value));
    }

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> fn)
    {
      foreach (var item in source)
        fn?.Invoke(item);
    }

    public static void For<T>(this IEnumerable<T> source, Action<int> fn)
    {
      var enumerable = source as T[] ?? source.ToArray();
      for (int i = 0; i < enumerable.Length; i++)
        fn?.Invoke(i);
    }

    public static void For(this int count, Action<int> fn)
    {
      for (var i = 0; i < count; i++)
        fn?.Invoke(i);
    }

    public static string ToRomanNumeral(this int number, bool emptyOnOutOfRange = false)
      => number switch
      {
        1  => "Ⅰ",
        2  => "Ⅱ",
        3  => "Ⅲ",
        4  => "Ⅳ",
        5  => "Ⅴ",
        6  => "Ⅵ",
        7  => "Ⅶ",
        8  => "Ⅷ",
        9  => "Ⅸ",
        10 => "Ⅹ",
        _  => emptyOnOutOfRange ? string.Empty : number.ToString()
      };

    public static string GetKeyString(this KeyCode keyCode)
      => keyCode switch
      {
        KeyCode.Alpha0 => "0",
        KeyCode.Alpha1 => "1",
        KeyCode.Alpha2 => "2",
        KeyCode.Alpha3 => "3",
        KeyCode.Alpha4 => "4",
        KeyCode.Alpha5 => "5",
        KeyCode.Alpha6 => "6",
        KeyCode.Alpha7 => "7",
        KeyCode.Alpha8 => "8",
        KeyCode.Alpha9 => "9",
        KeyCode.LeftControl => "LCtrl",
        KeyCode.RightControl => "RCtrl",
        KeyCode.LeftShift => "LShift",
        KeyCode.RightShift => "RShift",
        KeyCode.Escape => "ESC",
        KeyCode.Slash => "/",
        KeyCode.Colon => ":",
        KeyCode.Semicolon => ";",
        KeyCode.Minus => "-",
        KeyCode.Plus => "+",
        KeyCode.BackQuote => "`",
        KeyCode.LeftAlt => "LAlt",
        KeyCode.RightAlt => "RAlt",
        _ => keyCode.ToString()
      };
  }
}
