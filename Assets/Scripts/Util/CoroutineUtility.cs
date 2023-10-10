using System;
using System.Collections;
using Manager;
using UnityEngine;

namespace Util
{
  public static class CoroutineUtility
  {
    public static Coroutine Start(params (Yield yieldInstruction, Action fn)[] routines)
      => GameManager.CoroutineObject.StartCoroutine(Routine(routines));

    private static IEnumerator Routine(params (Yield yieldInstruction, Action fn)[] routines)
    {
      foreach (var (yieldInstruction, fn) in routines)
      {
        yield return yieldInstruction?.value;
        // try
        // {
        fn?.Invoke();
        // }
        // catch(Exception ex)
        // {
        //   Debug.LogWarning($"Coroutine error: {ex.Message}");
        //   yield break;
        // }
      }
    }

    public static Coroutine Wait(this float second, Action fn)
      => Start((new WaitForSeconds(second), fn));

    public static Coroutine WaitUnscaled(this float second, Action fn)
      => Start((new WaitForSecondsRealtime(second), fn));

    public static Coroutine WaitUntil(Func<bool> predicate, Action fn)
      => Start((new WaitUntil(predicate), fn));
  }
}
