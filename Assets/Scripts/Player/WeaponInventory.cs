using System;
using System.Collections.Generic;
using Manager;
using Store.Status;
using UnityEngine;

namespace Player
{
  public class WeaponInventory : MonoBehaviour
  {
    public int weaponMaxCount;

    public List<Weapon.Weapon> weapons;

    public List<Transform> weaponSlots;

    public Transform weaponParent;

    public event Action onChanged;

    private void Awake()
    {
      weaponParent = transform.Find("@weapons");
      for (var i = 1; i <= weaponMaxCount; i++)
      {
        weapons.Add(null);
        weaponSlots.Add(weaponParent.Find($"{i}"));
      }

      onChanged += () => GameManager.StatusUI.Refresh();
    }

    public void SetWeapon(int slot, string weaponName)
    {
      if (weapons[slot] != null)
        RemoveWeapon(slot);
      var data = GameManager.WeaponData[weaponName];

      weapons[slot] = data;
      var obj = Instantiate(GameManager.Weapons.Get(data.name), weaponSlots[slot].transform.position,
        Quaternion.identity,
        weaponSlots[slot]);
      obj.name = weaponName;
      onChanged?.Invoke();
    }

    public void RemoveWeapon(int slot)
    {
      weapons[slot] = null;
      Destroy(weaponSlots[slot].GetChild(0).gameObject);
      onChanged?.Invoke();
    }

    public void MoveWeapon(int original, int to)
    {
      if (weapons[to] != null)
      {
        SetWeapon(to, weapons[original].name);
        RemoveWeapon(original);
      }
      else
      {
        var tmp = weapons[to].name;
        RemoveWeapon(to);
        SetWeapon(to, weapons[original].name);
        RemoveWeapon(original);
        SetWeapon(original, tmp);
      }
    }

    [ContextMenu("Set Weapon for test")]
    public void Test()
    {
      // SetWeapon(0, "melee/testknife");
      // SetWeapon(1, "melee/testaxe");
      // SetWeapon(2, "ranged/testgun");
      SetWeapon(3, "ranged/testbomb");
    }
  }
}
