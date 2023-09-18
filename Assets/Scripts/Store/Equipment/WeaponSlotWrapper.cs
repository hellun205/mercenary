using System;
using Manager;
using UnityEngine;

namespace Store.Equipment
{
  public class WeaponSlotWrapper : MonoBehaviour
  {
    public WeaponSlotWrapperList list;

    public EquipmentType type;

    public int partnerIndex;

    [NonSerialized]
    public WeaponSlot[] slots;

    private void Awake()
    {
      slots = transform.GetComponentsInChildren<WeaponSlot>();

      for (var i = 0; i < slots.Length; i++)
      {
        slots[i].wrapper = this;
        slots[i].slotIndex = i;
      }
    }

    public void SetWeapon(int index, Weapon.Weapon weapon)
    {
      if (type == EquipmentType.Player)
      {
        if (weapon == null)
          GameManager.Player.weaponInventory.RemoveWeapon(index);
        else
          GameManager.Player.weaponInventory.SetWeapon(index, weapon.name);
      }
      else
      {
        if (weapon == null)
          GameManager.Player.partnerWeaponInventories[partnerIndex].RemoveWeapon(index);
        else
          GameManager.Player.partnerWeaponInventories[partnerIndex].SetWeapon(index, weapon.name);
      }
    }

    public void Refresh()
    {
    }
  }
}
