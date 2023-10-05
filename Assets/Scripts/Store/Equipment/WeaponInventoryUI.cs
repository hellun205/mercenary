using Manager;
using Store.Inventory;
using UI.DragNDrop;
using UnityEngine;

namespace Store.Equipment
{
  public class WeaponInventoryUI : MonoBehaviour
  {
    public WeaponSlotWrapper[] list;

    private void Awake()
    {
      foreach (var weaponSlotWrapper in list)
      {
        weaponSlotWrapper.list = this;
      }

      GameManager.Wave.onWaveEnd += Refresh;
    }

    public void Move((int wrapperIndex, int slotIndex) a, (int wrapperIndex, int slotIndex) b)
    {
      var wrapperA = list[a.wrapperIndex];
      var wrapperB = list[b.wrapperIndex];
      var slotA = wrapperA.slots[a.slotIndex];
      var slotB = wrapperB.slots[b.slotIndex];

      if (slotB.weapon == null)
      {
        slotB.Set(slotA.weapon);
        slotA.Set(null);
      }
      else
      {
        var tmp = slotB.weapon;
        slotB.Set(slotA.weapon);
        slotA.Set(tmp);
      }
      // SetWeapon(a.wrapperIndex, a.slotIndex, slotA.weapon);
      // SetWeapon(b.wrapperIndex, b.slotIndex, slotB.weapon);
    }

    public void SetWeapon(int wrapperIndex, int slotIndex, (string name, int tier)? weapon)
    {
      if (wrapperIndex == 0)
      {
        if (weapon == null)
          GameManager.Player.weaponInventory.RemoveWeapon(slotIndex);
        else
          GameManager.Player.weaponInventory.SetWeapon(slotIndex, weapon!.Value.name, weapon.Value.tier);
      }
      else
      {
        if (weapon == null)
          GameManager.Player.partners[wrapperIndex - 1].weaponInventory.RemoveWeapon(slotIndex);
        else
          GameManager.Player.partners[wrapperIndex - 1].weaponInventory
           .SetWeapon(slotIndex, weapon.Value.name, weapon.Value.tier);
      }
    }

    public void Refresh()
    {
      var playerWeapons = GameManager.Player.weaponInventory.weapons;
      for (var i = 0; i < playerWeapons.Count; i++)
      {
        if (playerWeapons[i] == null) continue;
        list[0].slots[i].Set(playerWeapons[i], false);
      }

      var partner = GameManager.Player.partners;

      for (var i = 0; i < partner.Length; i++)
      {
        var weapons = partner[i].weaponInventory.weapons;
        for (var j = 0; j < weapons.Count; j++)
        {
          if (weapons[j] == null) continue;
          list[i + 1].slots[j].Set(weapons[j], false);
        }
      }
    }
  }
}
