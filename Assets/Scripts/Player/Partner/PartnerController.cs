using System;
using UnityEngine;

namespace Player.Partner
{
  public class PartnerController : MonoBehaviour
  {
    public PartnerData partner { get; set; }

    public int tier { get; set; }
    
    public WeaponInventory weaponInventory { get; private set; }

    private void Awake()
    {
      weaponInventory = GetComponentInChildren<WeaponInventory>();
      weaponInventory.isPartnerOwner = true;
      weaponInventory.statusGetter = () => partner.GetStatus(tier);
    }
  }
}