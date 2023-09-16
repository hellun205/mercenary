using System;
using System.Collections;
using Manager;
using UnityEngine;

namespace Util
{
  public static class CoroutineUtility
  {
    public static Coroutine Start(params (Yield yieldInstruction, Action fn)[] routines)
      => GameManager.Instance.StartCoroutine(Routine(routines));
    
    private static IEnumerator Routine(params (Yield yieldInstruction, Action fn)[] routines)
    {
      foreach (var (yieldInstruction, fn) in routines)
      {
        yield return yieldInstruction?.value;
        fn?.Invoke();
      }
    }
  }
}
