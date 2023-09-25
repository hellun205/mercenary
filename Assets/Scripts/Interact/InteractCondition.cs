using System;

namespace Interact
{
  [Flags]
  public enum InteractCondition
  {
    None = 0,
    Normal = 1 << 1,
    Attack = 1 << 2,
  }
}
