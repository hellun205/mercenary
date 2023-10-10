using System;
using Manager;
using Store.Equipment;
using UnityEngine;
using Util;

namespace Player.Partner
{
  public class PartnerController : MonoBehaviour
  {
    [SerializeField]
    private string layerName;

    private PartnerData _partner;

    public PartnerData partner
    {
      get => _partner;
      set
      {
        _partner = value;
        if (value != null)
          wrapper.slots.ForEach(x =>
          {
            x.highlightAttribute = _partner.GetAttribute();
            x.highlightStatus = () => _partner.GetStatus(tier);
          });
        else
          wrapper.slots.ForEach(x => x.highlightAttribute = null);

        wrapper.slots.ForEach(x => x.RefreshAdditional());
      }
    }

    public int tier { get; set; }

    public WeaponInventory weaponInventory { get; private set; }
    public WeaponSlotWrapper wrapper { get; set; }

    private void Awake()
    {
      weaponInventory = GetComponentInChildren<WeaponInventory>();
      weaponInventory.isPartnerOwner = true;
      weaponInventory.statusGetter = () => partner.GetStatus(tier);
    }

    public void SetPartner(string partnerName)
    {
      var characterSlot = transform.Find("@character");
      RemovePartner();

      var obj = Instantiate
      (
        GameManager.Prefabs.Get<PartnerAnimation>($"partners/{partnerName}"),
        characterSlot.transform.position,
        Quaternion.identity,
        characterSlot
      );
      obj.sr.gameObject.layer = LayerMask.NameToLayer(layerName);
    }

    public void RemovePartner()
    {
      var characterSlot = transform.Find("@character");
      if (characterSlot.childCount > 0)
        Destroy(characterSlot.GetChild(0).gameObject);
    }
  }
}