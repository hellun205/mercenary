using System;
using Manager;
using TMPro;
using UI.Popup;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Util.Text;

namespace Store.Status
{
  public class StatusItem : UsePopup<PopupPanelWithTitle>
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

    public string valueFormat;

    [Multiline]
    public string description;

    public override string popupName => "$popup_stat";

    private void OnValidate()
    {
      Refresh();
    }

    public void Refresh()
    {
      statIconImage.sprite = statIcon;
      statNameText.text = statName;
      statValueText.text = GetValue(valueFormat);
    }

    public override void OnEntered()
    {
      popupPanel.ShowPopup(statName, GetValue(description), statIcon);
    }

    private string GetValue(string format)
    {
      try
      {
        return string.Format
        (
          format,
          statValue.GetViaValue(""),
          TaggedText.Parse(Mathf.FloorToInt(statValue * 100f).GetViaValue("")).AppendText("%"),
          (statValue * 0.1f).GetViaValue(""),
          Mathf.FloorToInt(statValue * 100f).GetViaValue(""),
          (Mathf.FloorToInt(GameManager.Player.moveSpeedPercent * 100f)).GetViaValue(""),
          TaggedText.Parse((Mathf.FloorToInt(GameManager.Player.moveSpeedPercent * 100f)).GetViaValue(""))
           .AppendText("%")
        );
      }
      catch
      {
        return format;
      }
    }
  }
}
