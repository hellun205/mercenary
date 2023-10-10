using System;
using System.Text;
using Data;
using Item;
using Manager;
using Store;
using UnityEngine;
using UnityEngine.Serialization;
using Util.Text;
using Weapon;
using Attribute = Weapon.Attribute;

namespace Player.Partner
{
  [CreateAssetMenu(fileName = "PartnerData", menuName = "PartnerData", order = 0)]
  public class PartnerData : ScriptableObject, IPossessible
  {
    [SerializeField]
    private string _displayName;

    [SerializeField]
    private Sprite _icon;

    [Multiline]
    public string description;

    public string specfiedName => name;
    public string itemName => $"[용병] {_displayName}";
    public Sprite icon => _icon;
    public bool hasTier => false;
    public int tier => -1;

    public string GetDescription(int tier)
    {
      var sb = new StringBuilder();

      sb.Append(_displayName)
       .Append("(이)가 장착한 ")
       .Append(GetAttribute().GetTexts().AddColor(GameManager.GetAttributeColor()))
       .Append(" 속성 무기 능력치 증가\n")
       .Append(GetStatus(tier).GetDescription())
       .Append(description);
        
      return sb.ToString();
    }

    public int GetPrice(int tier)
      => Convert.ToInt32(GameManager.Data.data.GetPartnerStatusData(name, tier, Data.PartnerData.Status.Price));

    public IncreaseStatus GetStatus(int tier)
      => GameManager.Data.data.GetPartnerStatus(name, tier);

    public Attribute GetAttribute()
      => GameManager.Data.data.GetPartnerAttribute(name);
  }
}
