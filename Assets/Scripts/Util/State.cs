using System;

namespace Util
{
  public class State<T>
  {
    private T _value;

    public T value
    {
      get => _value;
      set
      {
        _value = value;
        onSet?.Invoke(value);
      }
    }
    
    public event Action<T> onSet;

    public State(T defaultValue, params Action<T>[] onStateChangeEvents)
    {
      _value = defaultValue;

      foreach (var onStateChangeEvent in onStateChangeEvents)
        onSet += onStateChangeEvent;

      try
      {
        onSet?.Invoke(value);
      }
      catch
      {
      }
    }

    public State(params Action<T>[] onStateChangeEvents) : this(default, onStateChangeEvents)
    {
    }
  }
}
