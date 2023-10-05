using System;
using Manager;
using UnityEngine;

namespace Store.Equipment
{
  public class WeaponSlotWrapper : MonoBehaviour
  {
    public WeaponInventoryUI list;

    public EquipmentType type;

    public int partnerIndex;

    [NonSerialized]
    public WeaponSlot[] slots;

    public PartnerSlot partnerSlot;

    private void Awake()
    {
      slots = transform.GetComponentsInChildren<WeaponSlot>();

      for (var i = 0; i < slots.Length; i++)
      {
        slots[i].wrapper = this;
        slots[i].slotIndex = i;
      }
    }

    public void SetWeapon(int index, (string name, int tier)? weapon)
    {
      if (type == EquipmentType.Player)
      {
        if (weapon == null)
          GameManager.Player.weaponInventory.RemoveWeapon(index);
        else
          GameManager.Player.weaponInventory.SetWeapon(index, weapon!.Value.name, weapon.Value.tier);
      }
      else
      {
        if (weapon == null)
          GameManager.Player.partners[partnerIndex].weaponInventory.RemoveWeapon(index);
        else
          GameManager.Player.partners[partnerIndex].weaponInventory
           .SetWeapon(index, weapon.Value.name, weapon.Value.tier);
      }
    }

    public void Refresh()
    {
    }
  }
}
