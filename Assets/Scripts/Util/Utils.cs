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

    public static void Wait(float second, Action fn) => GameManager.Instance.StartCoroutine(WaitRoutine(second, fn));

    private static IEnumerator WaitRoutine(float second, Action fn)
    {
      yield return new WaitForSecondsRealtime(second);
      fn.Invoke();
    }

    public static Vector3 WorldToScreenSpace(this RectTransform canvas, Vector3 worldPos)
    {
      var screenPoint = Camera.main.WorldToScreenPoint(worldPos);
      screenPoint.z = 0;

      if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenPoint, Camera.main, out var screenPos))
        return screenPos;

      return screenPoint;
    }

    public static bool isSimilar(this float a, float b, float criteria = 0.3f)
      => Mathf.Abs(a - b) < criteria;
    
    [MenuItem("Assets/GetAssetPath")]
    static void GetAssetPath()
    {
      UnityEngine.Object pSelectObj = Selection.activeObject;
      string sAssetPath = AssetDatabase.GetAssetPath(pSelectObj.GetInstanceID());
      Debug.Log(sAssetPath);
    }
  }
}