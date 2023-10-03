using System;
using UnityEngine;

namespace Window.Contents
{
  public abstract class WindowContent<TReturn> : MonoBehaviour, IWindowContent
  {
    public abstract WindowType type { get; }
    public Components.Window window { get; private set; }

    public Action<TReturn> onReturn { private get; set; }

    protected virtual void Awake()
    {
      window = transform.GetComponentInParent<Components.Window>();
    }

    protected void Return(TReturn returnValue, bool closeWindow = true)
    {
      onReturn?.Invoke(returnValue);
      if (closeWindow) window.Close();
    }
  }
}
