using TMPro;
using UI.Popup;
using UnityEngine;
using UnityEngine.UI;

namespace Consumable
{
  public class BuffPopupPanel : PopupPanel
  {
    [SerializeField]
    private Image iconImage;
    
    [SerializeField]
    private TextMeshProUGUI titleText;

    public void ShowPopup(string title, string description, Sprite icon)
    {
      iconImage.sprite = icon;
      titleText.text = title;
      
      ShowPopup(description);
    }
  }
}