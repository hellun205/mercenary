using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util;

namespace UI.Popup
{
  public class PopupPanel : MonoBehaviour
  {
    public RectTransform rectTransform { get; private set; }
    
    [FormerlySerializedAs("nameText")]
    [SerializeField]
    protected TextMeshProUGUI valueText;
    
    private void Awake()
    {
      rectTransform = GetComponent<RectTransform>();
      gameObject.SetActive(false);
    }

    public void ShowPopup(string text)
    {
      ShowPopup();
      valueText.text = text;
      this.RebuildLayout(20);
    }

    public void ShowPopup() => gameObject.SetActive(true);
    public void HidePopup() => gameObject.SetActive(false);
  }
}
