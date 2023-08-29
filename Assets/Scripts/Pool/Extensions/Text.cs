using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pool.Extensions
{
  public class Text : UsePool
  {
    [SerializeField]
    private TextMeshProUGUI text;
    
    [SerializeField]
    private Image bg;

    public string value
    {
      get => text.text;
      set => text.text = value;
    }

    public Color color
    {
      get => text.color;
      set => text.color = value;
    }

    public Color bgColor
    {
      get => bg.color;
      set => bg.color = value;
    }

    public FontWeight fontWeight
    {
      get => text.fontWeight;
      set => text.fontWeight = value;
    }

    protected override void OnSummon()
    {
      value = "";
      color = Color.white;
      bgColor = new Color(0f, 0f, 0f, 0.5f);
      fontWeight = FontWeight.Regular;
    }
  }
}
