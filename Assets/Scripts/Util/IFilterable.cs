using System;

namespace Util
{
  public interface IFilterable<T> where T : Enum
  {
    public T filterType { get; }
  }
}
