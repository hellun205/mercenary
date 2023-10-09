using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Popup
{
  public class PopupPanelWithTitle : PopupPanel
  {
    [FormerlySerializedAs("nameText")]
    [SerializeField]
    protected TextMeshProUGUI titleText;

    [SerializeField]
    protected Image iconImage;

    public void ShowPopup(string title, string desc, Sprite icon)
    {
      titleText.text = title;
      valueText.text = desc;
      iconImage.sprite = icon;
      ShowPopup();
    }
  }
}
