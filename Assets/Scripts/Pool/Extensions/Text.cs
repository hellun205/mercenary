using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pool.Extensions
{
  [RequireComponent(typeof(PoolObject))]
  public class Text : MonoBehaviour
  {
    public TextMeshProUGUI t;
    public Image bg;
    public PoolObject po;

    private void Awake()
    {
      po = GetComponent<PoolObject>();
    }

    public string value
    {
      get => t.text;
      set => t.text = value;
    }

    public Color color
    {
      get => t.color;
      set => t.color = value;
    }

    public Color bgColor
    {
      get => bg.color;
      set => bg.color = value;
    }
  }
}
