using System.Text;
using Item;
using Manager;
using Player.Partner;
using Unity.Collections;
using UnityEngine;
using Util;
using Util.Text;
using Weapon;

namespace Store.Partner
{
  public struct PartnerItem : IPurchasable
  {
    public int price => partner != null ? partner.GetPrice(tier) : 0;
    public string name => $"{partner.itemName} {(tier + 1).ToRomanNumeral()}";

    public string description => partner.GetDescription(tier);

    public string addtive => partner.GetAttribute().GetTexts();
    public Sprite icon => partner.icon;
    public Color color => GameManager.GetTierColor(tier);

    public PartnerData partner { get; set; }
    public int tier { get; set; }
  }
}
