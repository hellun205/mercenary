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
    
  }
}
