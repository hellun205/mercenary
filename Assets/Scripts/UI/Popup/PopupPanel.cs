using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util;
using Util.UI;

namespace UI.Popup
{
  public class PopupPanel : MonoBehaviour
  {
    public RectTransform rectTransform { get; private set; }
    
    [FormerlySerializedAs("nameText")]
    [SerializeField]
    protected TextMeshProUGUI valueText;

    public bool perfectHide;
    
    private void Awake()
    {
      rectTransform = GetComponent<RectTransform>();
      gameObject.SetVisible(false);
    }

    public void ShowPopup(string text)
    {
      ShowPopup();
      valueText.text = text;
      this.RebuildLayout(20);
    }

    public void ShowPopup()
    {
      if (perfectHide)
      {
        gameObject.SetActive(true);
        gameObject.SetVisible(true);
      }
      else
        gameObject.SetVisible(true);
    }

    public void HidePopup()
    {
      if (perfectHide)
      {
        gameObject.SetActive(false);
        gameObject.SetVisible(true);
      }
      else
        gameObject.SetVisible(false);
    }
  }
}
