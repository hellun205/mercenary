using System;
using System.Collections;
using Manager;
using UnityEngine;

namespace Util
{
  public static class CoroutineUtility
  {
    public static Coroutine Start(params (Yield yieldInstruction, Action fn)[] routines)
      => GameManager.Manager.StartCoroutine(Routine(routines));
    
    private static IEnumerator Routine(params (Yield yieldInstruction, Action fn)[] routines)
    {
      foreach (var (yieldInstruction, fn) in routines)
      {
        yield return yieldInstruction?.value;
        fn?.Invoke();
      }
    }

    public static Coroutine Wait(float second, Action fn)
      => Start((new WaitForSeconds(second), fn));
    
    public static Coroutine WaitUnscaled(float second, Action fn)
      => Start((new WaitForSecondsRealtime(second), fn));
    
    public static Coroutine WaitUntil(Func<bool> predicate, Action fn)
      => Start((new WaitUntil(predicate), fn));
  }
}
