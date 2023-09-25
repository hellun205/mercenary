using TMPro;
using UI.Popup;
using UnityEngine;
using UnityEngine.Serialization;

namespace Store
{
  public class WeaponPopup : PopupPanelWithTitle
  {
    [FormerlySerializedAs("attribute")]
    [SerializeField]
    protected TextMeshProUGUI attributeText;
    
    public void ShowPopup(string name, string attribute , string desc)
    {
      titleText.text = name;
      attributeText.text = attribute;
      valueText.text = desc;
      gameObject.SetActive(true);
    }
  }
}
