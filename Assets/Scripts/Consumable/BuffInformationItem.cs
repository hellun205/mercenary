using UI;
using UI.Popup;
using UnityEngine;
using UnityEngine.UI;

namespace Consumable
{
  public class BuffInformationItem : UsePopup<BuffPopupPanel>, IPoolableUI<BuffInformationItem>
  {
    public override string popupName => "$popup_buff";

    public string title { get; set; }
    public string description { get; set; }

    public Sprite icon
    {
      get => iconImage.sprite;
      set => iconImage.sprite = value;
    }
    
    public Image iconImage;
    public Image panelImage;
    public Image timerImage;

    public override void OnEntered()
    {
      popupPanel.rectTransform.pivot = new Vector2(0.9f, 0.9f);
      popupPanel.ShowPopup(title, description, icon);
    }
    public BuffInformationItem component => this;
  }
}