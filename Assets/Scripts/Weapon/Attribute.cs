using System;
using System.Text;
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

  public static class AttributeUtility
  {
    public static string GetText(this Attribute attribute)
      => attribute switch
      {
        Attribute.Sword         => "검",
        Attribute.Sharpness     => "정밀",
        Attribute.Spear         => "창",
        Attribute.Lazer         => "레이저",
        Attribute.Bluntness     => "둔기",
        Attribute.Heavy         => "중무기",
        Attribute.Gun           => "총",
        Attribute.AutomaticFire => "연사",
        Attribute.Explosion     => "폭발",
        _                       => "Unknown",
      };

    public static string GetTexts(this Attribute attribute)
    {
      var sb = new StringBuilder();

      if ((attribute & Attribute.Sword) != 0)
        sb.Append(Attribute.Sword.GetText()).Append(", ");
      
      if ((attribute & Attribute.Sharpness) != 0)
        sb.Append(Attribute.Sharpness.GetText()).Append(", ");
      
      if ((attribute & Attribute.Spear) != 0)
        sb.Append(Attribute.Spear.GetText()).Append(", ");
      
      if ((attribute & Attribute.Lazer) != 0)
        sb.Append(Attribute.Lazer.GetText()).Append(", ");
      
      if ((attribute & Attribute.Bluntness) != 0)
        sb.Append(Attribute.Bluntness.GetText()).Append(", ");
      
      if ((attribute & Attribute.Heavy) != 0)
        sb.Append(Attribute.Heavy.GetText()).Append(", ");
      
      if ((attribute & Attribute.Gun) != 0)
        sb.Append(Attribute.Gun.GetText()).Append(", ");
      
      if ((attribute & Attribute.AutomaticFire) != 0)
        sb.Append(Attribute.AutomaticFire.GetText()).Append(", ");
      
      if ((attribute & Attribute.Explosion) != 0)
        sb.Append(Attribute.Explosion.GetText()).Append(", ");

      sb.Remove(sb.ToString().LastIndexOf(", "), 2);
      
      return sb.ToString();
    }
  }
}
