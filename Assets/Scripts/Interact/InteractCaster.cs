using System;

namespace Interact
{
  [Flags]
  public enum InteractCaster
  {
    Nothing = 0,
    Player = 1 << 1,
    Others = 1 << 2
  }
}