using System;
using Store.Equipment;
using UnityEngine;
using Util;

namespace Player.Partner
{
  public class PartnerController : MonoBehaviour
  {
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
  }
}