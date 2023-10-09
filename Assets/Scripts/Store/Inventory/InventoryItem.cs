using System;
using System.Text;
using Consumable;
using Data;
using Manager;
using Store.Equipment;
using TMPro;
using UI;
using UI.Popup;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Util.Text;
using Util.UI;
using Weapon;
using WeaponData = Weapon.WeaponData;

namespace Store.Inventory
{
  public class InventoryItem
    : UsePopup<ListPopup>,
      IPoolableUI<InventoryItem>,
      IUseContextMenu
  {
    public (string name, int tier)? itemData;

    [SerializeField]
    private TextMeshProUGUI count;

    [SerializeField]
    private Image icon;

    private Image img;

    public override string popupName => "$popup_item";

    protected override void Awake()
    {
      base.Awake();
      img = GetComponent<Image>();
    }

    public void SetItem((string name, int tier)? item, ushort count)
    {
      itemData = item;
      icon.sprite = GameManager.GetIPossessible(itemData!.Value.name).icon;
      img.color = GameManager.GetTierColor(itemData.Value.tier);
      SetCount(count);
    }

    public void SetCount(ushort count)
    {
      this.count.text = $"{(count == 1 ? "" : count)}";
    }

    public override void OnEntered()
    {
      popupPanel.HidePopup();
      // popupPanel.ShowPopup
      // (
      //   itemData.itemName,
      //   itemData is WeaponData weapon ? weapon.attribute.GetTexts() : "",
      //   itemData.description
      // );

      var sb = new StringBuilder();
      var item = GameManager.GetIPossessible(itemData!.Value.name);

      sb.Append
        (
          $"{item.itemName} {(itemData.Value.tier + 1).ToRomanNumeral(true)}"
           .SetSizePercent(1.25f)
           .SetAlign(TextAlign.Center)
        )
       .Append("\n");
      if (item is WeaponData weaponData)
      {
        sb.Append
          (
            weaponData.attribute.GetTexts()
             .SetSizePercent(1.25f)
             .AddColor(GameManager.GetAttributeColor())
             .SetLineHeight(1.25f)
             .SetAlign(TextAlign.Center)
          )
         .Append("\n");
      }

      sb.Append
      (
        item.GetDescription(itemData.Value.tier)
         .SetAlign(TextAlign.Left)
      );

      if (item is WeaponData weaponData2)
        popupPanel.ShowPopup(sb.ToString(),
          GameManager.Data.data.GetAttributeChemistryDescriptions(weaponData2.attribute));
      else
        popupPanel.ShowPopup(text: sb.ToString());
    }
    
    public InventoryItem component => this;
    
    public string contextMenuName => "$context_menu_cant_duplicate";

    public object[] contextMenuFormats => new object[]
    {
      $"${GameManager.GetIPossessible(itemData!.Value.name).GetPrice(itemData!.Value.tier) / 2}"
    };

    public bool contextMenuCondition => itemData.HasValue;

    public Action<string> contextMenuFunction => res =>
    {
      switch (res)
      {
        case "sell":
          GameManager.Manager.coin.value +=
            GameManager.GetIPossessible(itemData!.Value.name).GetPrice(itemData!.Value.tier) / 2;
          GameManager.Player.inventory.LoseItem(itemData!.Value.name, itemData.Value.tier);
          break;
      }
    };
  }
}
