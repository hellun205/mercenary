using System;
using Manager;
using Player.Partner;
using UI.DragNDrop;
using UI.Popup;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Util.UI;

namespace Store.Equipment
{
  public class PartnerSlot : UsePopup<PopupPanel>
  {
    [SerializeField]
    private GameObject shielder;

    private (string name, int tier)? partner;

    private ItemDrop useDrop;
    private ItemDrag useDrag;

    public int index;

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
      if (obj.beginDragType != DragType.Inventory || data is not PartnerData) return;

      SetPartner((data.specfiedName, data.tier));
      GameManager.Player.inventory.LoseItem(obj.item, obj.tier);
    }

    public void SetPartner((string name, int tier)? data)
    {
      partner = data;
      shielder.SetVisible(data.HasValue, 0.1f);
      GameManager.Player.partnerWeaponInventories[index].gameObject.SetActive(data.HasValue);

      iconImg.sprite = data.HasValue ? GameManager.GetIPossessible(data.Value.name).icon : GameManager.emptySprite;
    }
  }
}
