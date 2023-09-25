using System;
using UnityEngine;

namespace Weapon
{
  [Flags]
  public enum Attribute
  {
    None = 0,
    /// 검
    Sword = 1 << 0,
    /// 정밀
    Sharpness = 1 << 1,
    /// 창
    Spear = 1 << 2,
    /// 레이저
    Lazer = 1 << 3,
    /// 둔기
    Bluntness = 1 << 4,
    /// 중무기
    Heavy = 1 << 5,
    /// 총
    Gun = 1 << 6,
    /// 연사
    AutomaticFire = 1 << 7,
    /// 폭발
    Explosion = 1 << 8,
  }
}
