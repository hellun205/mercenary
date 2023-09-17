using System;
using System.Text;
using UnityEngine;

namespace Util
{
  public static class TextUtility
  {
    public static string AddStyle(this string text, string styleTag, string value = null)
      => $"<{styleTag}{(string.IsNullOrEmpty(value) ? "" : $"={value}")}>{text}</{styleTag}>";

    public static string AddColor(this string text, Color color)
      => text.AddStyle("color", $"#{ColorUtility.ToHtmlStringRGBA(color)}");

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
  }
}
