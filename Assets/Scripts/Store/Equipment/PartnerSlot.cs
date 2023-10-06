using System;
using System.Text;
using Manager;
using Player.Partner;
using UI.DragNDrop;
using UI.Popup;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Util.Text;
using Util.UI;
using Weapon;

namespace Store.Equipment
{
  public class PartnerSlot : UsePopup<PopupPanel>
  {
    [SerializeField]
    private GameObject shielder;

    public (string name, int tier)? partner;

    private ItemDrop useDrop;
    private ItemDrag useDrag;

    public int index;

    [SerializeField]
    private Image panelImg;

    [SerializeField]
    private Image iconImg;

    public override string popupName => "$popup_partner";

    protected override void Awake()
    {
      base.Awake();
      useDrag = GetComponent<ItemDrag>();
      useDrop = GetComponent<ItemDrop>();

      useDrag.draggingObject = GameManager.UI.Find<DraggingObject>("$dragging_item");
      useDrag.condition = () => partner != null;
      useDrag.getter = () => new ItemRequest
      {
        beginDragType = DragType.PartnerSlot,
        item = partner!.Value.name,
        tier = partner!.Value.tier,
        partnerData = index,
        partnerSlot = this,
        draggingImage = GameManager.GetIPossessible(partner?.name).icon,
      };

      useDrop.onGetRequest += OnDrop;
    }

    private void OnDrop(ItemRequest obj)
    {
      var data = GameManager.GetIPossessible(obj.item);
      if (data is not PartnerData) return;
      if (obj.beginDragType == DragType.Inventory)
      {
        if (partner.HasValue)
          GameManager.Player.inventory.GainItem(partner.Value.name, partner.Value.tier);
        SetPartner((data.specfiedName, obj.tier));
        GameManager.Player.inventory.LoseItem(obj.item, obj.tier);
      }
      else if (obj.beginDragType == DragType.PartnerSlot)
      {
        var tmp = (obj.partnerSlot.partner!.Value.name, obj.partnerSlot.partner.Value.tier);

        if (partner.HasValue)
          obj.partnerSlot.SetPartner(partner);
        else
          obj.partnerSlot.SetPartner(null);

        SetPartner(tmp);
      }

      OnEntered();
    }

    public void SetPartner((string name, int tier)? data)
    {
      partner = data;
      shielder.SetVisible(!data.HasValue, 0.1f, ignoreEqual: true);
      GameManager.Player.partners[index].partner =
        data.HasValue ? GameManager.GetIPossessible(data.Value.name) as PartnerData : null;
      GameManager.Player.partners[index].tier = data?.tier ?? 0;
      GameManager.Player.partners[index].weaponInventory.gameObject.SetActive(data.HasValue);
      panelImg.color = GameManager.GetTierColor(data?.tier ?? 0);
      iconImg.color = data.HasValue ? Color.white : Color.clear;
      if (data.HasValue)
        iconImg.sprite = GameManager.GetIPossessible(data.Value.name).icon;
    }

    public override void OnEntered()
    {
      if (partner == null) return;
      var data = GameManager.GetIPossessible(partner.Value.name) as PartnerData;
      var sb = new StringBuilder();

      sb.Append
        (
          $"{data.itemName} {(partner.Value.tier + 1).ToRomanNumeral()}"
            .SetSizePercent(1.25f)
            .SetAlign(TextAlign.Center)
        )
        .Append("\n")
        .Append
        (
          data.GetAttribute().GetTexts()
            .SetSizePercent(1.25f)
            .AddColor(GameManager.GetAttributeColor())
            .SetLineHeight(1.25f)
            .SetAlign(TextAlign.Center)
        )
        .Append("\n");


      sb.Append
      (
        data.GetDescription(partner.Value.tier)
          .SetAlign(TextAlign.Left)
      );
      popupPanel.ShowPopup(sb.ToString());
    }
  }
}