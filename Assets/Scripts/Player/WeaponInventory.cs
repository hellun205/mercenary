using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace Player
{
  public class WeaponInventory : MonoBehaviour
  {
    public int weaponMaxCount;

    public List<Weapon.Weapon> weapons;

    public List<Transform> weaponSlots;

    public Transform weaponParent;

    private void Awake()
    {
      weaponParent = transform.Find("@weapons");
      for (var i = 1; i <= weaponMaxCount; i++)
      {
        weapons.Add(null);
        weaponSlots.Add(weaponParent.Find($"{i}"));
      }
      // SetWeapon(0, "melee/testknife");
      // Utils.Wait(5f, () => RemoveWeapon(0));
    }

    public void SetWeapon(int slot, string weaponName)
    {
      if (weapons[slot] != null)
        RemoveWeapon(slot);
      var data = GameManager.WeaponData[weaponName];
      
      weapons[slot] = data;
      Instantiate(GameManager.Weapons.Get(data.name), weaponSlots[slot].transform.position, Quaternion.identity,
        weaponSlots[slot]);
    }

    public void RemoveWeapon(int slot)
    {
      weapons[slot] = null;
      Destroy(weaponSlots[slot].GetChild(0).gameObject);
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
  }
}
