using Item;
using Manager;
using UnityEngine;
using Util;
using Weapon;

namespace Store.Item
{
  public struct ItemItem : IPurchasable
  {
    public int price => possessible?.GetPrice(tier) ?? -1;
    public string name => $"{possessible.itemName} {(tier + 1).ToRomanNumeral()}";
    public string description => possessible.GetDescription(tier);
    public string addtive => possessible is WeaponData w ? w.attribute.GetTexts() : string.Empty;
    public Sprite icon => possessible.icon;
    public Color color => GameManager.GetTierColor(tier);

    public IPossessible possessible { get; set; }
    public int tier { get; set; }
  }
}