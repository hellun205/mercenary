using Item;
using Manager;
using UnityEngine;
using Util;

namespace Store.Item
{
  public struct ItemItem : IPurchasable
  {
    public int price => possessible?.GetPrice(tier) ?? -1;
    public string displayName => $"{possessible.itemName} {(tier + 1).ToRomanNumeral()}";
    public string description => possessible.GetDescription(tier);
    public string addtive => string.Empty;
    public Sprite icon => possessible.icon;
    public Color color => GameManager.GetTierColor(tier);

    public IPossessible possessible { get; set; }
    public int tier { get; set; }
  }
}
