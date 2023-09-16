using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        camera = Camera.main;
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

    public static bool ApplyPercentage(this float percentage, params float[] additive)
    {
      var random = Random.Range(0f, 1f);
      foreach (var add in additive)
        percentage += add;
      return Mathf.Min(1f, Mathf.Max(0,percentage)) > random;
    }
  }
}
