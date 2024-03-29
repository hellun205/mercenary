using System;
using System.Linq;
using Manager;
using Sound;
using Store.Inventory;
using UI.DragNDrop;
using UnityEngine;
using Util;
using Util.Text;

namespace Store.Equipment
{
  public class WeaponInventoryUI : MonoBehaviour
  {
    public WeaponSlotWrapper[] list;

    public event Action onDuplicated;

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

    public void RemoveWeapon((string name, int tier) weapon)
    {
      foreach (var weaponSlotWrapper in list)
      {
        foreach (var weaponSlot in weaponSlotWrapper.slots)
        {
          if (weaponSlot.weapon.HasValue && weaponSlot.weapon.Value == weapon)
          {
            var wrapperIndex = weaponSlotWrapper.GetIndex();
            var slotIndex = weaponSlot.slotIndex;
            list[wrapperIndex].slots[slotIndex].Set(null);
            SetWeapon(wrapperIndex, slotIndex, null);
            return;
          }
        }
      }

      // var l = list.Select
      // (
      //   x => x.slots.Where
      //   (
      //     y => y.weapon.HasValue && y.weapon == weapon
      //   ).Select
      //   (
      //     y => (y.wrapper.GetIndex(), y.slotIndex)
      //   ).First()
      // ).First();
      //
      // SetWeapon(l.Item1, l.slotIndex, null);
      // list[l.Item1].slots[l.slotIndex].Set(null);
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

    public (bool canPurchase, bool canDuplicate) TryAddWeapon((string name, int tier) weapon)
    {
      foreach (var wrapper in list)
      {
        if (wrapper.type == EquipmentType.Partner && !wrapper.partnerSlot.partner.HasValue) continue;
        foreach (var slot in wrapper.slots)
        {
          if (!slot.weapon.HasValue)
          {
            slot.Set(weapon);
            return (true, false);
          }
        }
      }

      foreach (var wrapper in list)
      {
        if (wrapper.type == EquipmentType.Partner && !wrapper.partnerSlot.partner.HasValue) continue;
        foreach (var slot in wrapper.slots)
        {
          if (slot.weapon.HasValue && slot.weapon.Value == weapon && slot.weapon.Value.tier < 3)
          {
            return (true, true);
          }
        }
      }

      return (false, false);
    }

    public void DuplicateWeapon((string name, int tier) weapon)
    {
      (int wrapper, int slot)? tmp = null;
      foreach (var wrapper in list)
      {
        if (wrapper.type == EquipmentType.Partner && !wrapper.partnerSlot.partner.HasValue) continue;
        foreach (var slot in wrapper.slots)
        {
          if (slot.weapon.HasValue && slot.weapon.Value == weapon && slot.weapon.Value.tier < 3)
          {
            if (tmp.HasValue && tmp.Value == (wrapper.GetIndex(), slot.slotIndex)) continue;
            if (!tmp.HasValue)
            {
              tmp = (wrapper.GetIndex(), slot.slotIndex);
              continue;
            }

            slot.Set((weapon.name, weapon.tier + 1));
            RemoveWeapon(weapon);
            onDuplicated?.Invoke();

            var item = GameManager.GetIPossessible(weapon.name);
            GameManager.Broadcast.Say
            (
              "{0}(을)를 {1}티어로 결합하였습니다.",
              $"{item.itemName} {(weapon.tier + 1).ToRomanNumeral()}",
              (weapon.tier + 2).ToRomanNumeral()
            );
            GameManager.Sound.Play(SoundType.SFX_Normal, "sfx/normal/duplicate");
            return;
          }
        }
      }
      
      GameManager.Broadcast.Say("결합할 수 있는 무기가 존재하지 않습니다.");
      GameManager.Sound.Play(SoundType.SFX_UI, "sfx/ui/error");
    }

    public void DuplicateWeaponForce((string name, int tier) weapon)
    {
      foreach (var wrapper in list)
      {
        if (wrapper.type == EquipmentType.Partner && !wrapper.partnerSlot.partner.HasValue) continue;
        foreach (var slot in wrapper.slots)
        {
          if (slot.weapon.HasValue && slot.weapon.Value == weapon && slot.weapon.Value.tier < 3)
          {
            slot.Set((weapon.name, weapon.tier + 1));
            var item = GameManager.GetIPossessible(weapon.name);
            GameManager.Broadcast.Say
            (
              "{0}(을)를 {1}티어로 결합하였습니다.",
              $"{item.itemName} {(weapon.tier + 1).ToRomanNumeral()}",
              (weapon.tier + 2).ToRomanNumeral()
            );
            GameManager.Sound.Play(SoundType.SFX_Normal, "sfx/normal/duplicate");
            return;
          }
        }
      }
    }
  }
}
