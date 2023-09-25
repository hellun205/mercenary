using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Popup
{
  public class PopupPanelWithTitle : PopupPanel
  {
    [FormerlySerializedAs("nameText")]
    [SerializeField]
    protected TextMeshProUGUI titleText;

    public void ShowPopup(string title, string desc)
    {
      titleText.text = title;
      valueText.text = desc;
      ShowPopup();
    }
  }
}
