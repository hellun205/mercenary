using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pool.Extensions
{
  [RequireComponent(typeof(PoolObject))]
  public class Text : MonoBehaviour
  {
    public TextMeshProUGUI text;
    public Image bg;
    
    public PoolObject po;

    private void Awake()
    {
      po = GetComponent<PoolObject>();
    }

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
  }
}
