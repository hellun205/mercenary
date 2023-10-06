using UI;
using UI.Popup;
using UnityEngine;
using UnityEngine.UI;

namespace Consumable
{
  public class BuffInformationItem : UsePopup<BuffPopupPanel>
  {
    public override string popupName => "$popup_buff";

    public string title { get; set; }
    public string description { get; set; }
    public Sprite icon { get; set; }
    
    public Image iconImage;
    public Image panelImage;
    public Image timerImage;

    public override void OnEntered()
    {
      popupPanel.ShowPopup(title, description, icon);
    }
  }
}