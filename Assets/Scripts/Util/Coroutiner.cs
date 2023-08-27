using System;
using System.Collections;
using Manager;
using UnityEngine;

namespace Util
{
  public class Coroutiner
  {
    public Coroutine current;

    private Func<IEnumerator> defaultRoutine;
    private Func<object[], IEnumerator> routine;

    private MulticastDelegate GetRoutine() => (MulticastDelegate)defaultRoutine ?? routine;
    
    public Coroutiner(Func<object[],IEnumerator> routine)
    {
      this.routine = routine;
    }
    
    public Coroutiner(Func<IEnumerator> routine)
    {
      this.defaultRoutine = routine;
    }

    public void Start(params object[] parameters)
    {
      Stop();
      current = GameManager.Instance.StartCoroutine((IEnumerator)GetRoutine().DynamicInvoke(parameters));
    }

    public void Stop()
    {
      if (current is not null)
        GameManager.Instance.StopCoroutine(current);
    }
  }
}