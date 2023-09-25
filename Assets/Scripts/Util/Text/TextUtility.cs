using System;
using UnityEngine;

namespace Util.Text
{
  public static class TextUtility
  {
    public static string AddStyle(this string text, string styleTag, string value = null)
      => $"<{styleTag}{(string.IsNullOrEmpty(value) ? "" : $"={value}")}>{text}</{styleTag}>";

    public static string AddStyleWithoutCloseTag(this string text, string styleTag, string value = null)
      => $"<{styleTag}{(string.IsNullOrEmpty(value) ? "" : $"={value}")}>{text}";

    public static string AddColor(this string text, Color color)
      => text.AddStyle("color", $"#{ColorUtility.ToHtmlStringRGBA(color)}");

    public static string SetPos(this string text, float percent)
      => text.AddStyleWithoutCloseTag("pos", $"{percent * 100f}%");

    public static string SetSizePercent(this string text, float percent)
      => text.AddStyle("size", $"{percent * 100f}%");

    public static string SetSizePx(this string text, float px)
      => text.AddStyle("size", $"{px}px");

    public static string SetIndent(this string text, float percent)
      => text.AddStyle("indent", $"{percent * 100}%");

    public static string AddItalic(this string text)
      => text.AddStyle("i");

    public static string AddBold(this string text)
      => text.AddStyle("b");

    public static string SetAlign(this string text, TextAlign value)
      => text.AddStyleWithoutCloseTag("align", value.ToString().ToLower());

    public static string SetUpperCase(this string text)
      => text.AddStyle("uppercase");
    
    public static string SetLowerCase(this string text)
      => text.AddStyle("lowercase");
    
    public static string SetAllCaps(this string text)
      => text.AddStyle("allcaps");
    
    public static string SetSmallCaps(this string text)
      => text.AddStyle("smallcaps");

    public static string SetAlpha(this string text, float percent)
      => text.AddStyle("alpha", $"#{Convert.ToString(Mathf.RoundToInt(percent * 255f), 8)}");

    public static string SetLineHeight(this string text, float percent)
      => text.AddStyle("line-height", $"{percent * 100f}%");
    
    public static string GetViaValue
    (
      this float value,
      Color plusColor,
      Color minusColor,
      Color zeroColor,
      string plus = "+",
      string minus = "-"
    )
    {
      var color = value > 0 ? plusColor : value == 0 ? zeroColor : minusColor;
      var sign = value > 0 ? plus : value == 0 ? "" : minus;
      return $"{sign}{Math.Abs(Math.Round(value, 2))}".AddColor(color);
    }
    public static string GetViaValue(this float value, string plus = "+", string minus = "-")
      => GetViaValue(value, new Color32(47, 157, 39, 255), new Color32(152, 0, 0, 255), Color.white, plus, minus);

    public static string GetViaValue(this int value, string plus = "+", string minus = "-")
      => GetViaValue((float) value, new Color32(47, 157, 39, 255), new Color32(152, 0, 0, 255), Color.white, plus,
        minus);
  }
}
