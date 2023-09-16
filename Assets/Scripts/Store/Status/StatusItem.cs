using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Store.Status
{
  public class StatusItem : MonoBehaviour
  {
    [Header("Bind")]
    [SerializeField]
    private Image statIconImage;
    
    [SerializeField]
    private TextMeshProUGUI statNameText;
    
    [SerializeField]
    private TextMeshProUGUI statValueText;

    [Header("Value")]
    public Sprite statIcon;
    public string statName;
    public float statValue;

    private void OnValidate()
    {
      Refresh();
    }

    public void Refresh()
    {
      statIconImage.sprite = statIcon;
      statNameText.text = statName;
      statValueText.text = statValue.GetViaValue();
    }
  }
}
