using System;

namespace Util.Text
{
  public struct TaggedText
  {
    public string text;
    public string tag;
    public string value;

    public static readonly TaggedText empty = new ("");
    
    public TaggedText(string text, string tag = "", string value = "")
    {
      this.text = text;
      this.tag = tag;
      this.value = value;
    }

    public override string ToString() => $"{OpenTag}{text}{CloseTag}";

    public string OpenTag => $"<{tag}{(string.IsNullOrEmpty(value) ? "" : $"={value}")}>";
    public string CloseTag => $"</{tag}>";

    public TaggedText AppendText(string str)
    {
      text = $"{text}{str}";
      return this;
    }

    public static implicit operator string(TaggedText o) => o.ToString();

    public static TaggedText Parse(string str)
    {
      try
      {
        var openSplit = str.Split('<')[1].Split('>')[0];
        string text;
        string tag;
        var value = "";

        if (openSplit.Contains('='))
        {
          var split = openSplit.Split('=');
          tag = split[0];
          if (split[1].Contains('"'))
          {
            var rm = split[1].Replace("\"", "");
            value = rm;
          }
          else
            value = split[1];
        }
        else
        {
          tag = openSplit;
        }

        var open = $"<{openSplit}>";
        var openAfter = str.Remove(str.IndexOf(open), open.Length);

        var close = $"</{tag}>";
        text = openAfter.Remove(openAfter.IndexOf(close), close.Length);

        return new TaggedText(text, tag, value);
      }
      catch
      {
        throw new Exception("can't parse to TaggedText");
      }
    }

    public static bool TryParse(string str, out TaggedText outValue)
    {
      outValue = empty;
      try
      {
        outValue = Parse(str);
        return true;
      }
      catch
      {
        return false;
      }
    }
  }
}
