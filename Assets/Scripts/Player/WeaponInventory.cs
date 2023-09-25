using System;
using System.Collections.Generic;
using Manager;
using Store.Status;
using UnityEngine;
using Weapon;

namespace Player
{
  public class WeaponInventory : MonoBehaviour
  {
    public int weaponMaxCount;

    public List<Weapon.WeaponData> weapons;

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

    private void Update()
    {
      for (var i = 0; i < weapons.Count; i++)
      {
        if (weapons[i] == null) continue;

        weaponSlots[i].transform.GetChild(0).position = weaponSlots[i].transform.position;
      }
    }

    public void SetWeapon(int slot, string weaponName, int tier)
    {
      if (weapons[slot] != null)
        RemoveWeapon(slot);
      var data = GameManager.WeaponData[weaponName];

      weapons[slot] = data.weapons[tier];
      var obj = Instantiate(GameManager.Weapons.Get(data.name), weaponSlots[slot].transform.position,
        Quaternion.identity,
        weaponSlots[slot]);
      obj.name = weaponName;
      (obj.GetComponent(typeof(ITier)) as ITier).tier = tier;
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
        var o = weapons[original].information;
        SetWeapon(to, o.name, o.tier);
        RemoveWeapon(original);
      }
      else
      {
        var t = weapons[to].information;
        var o = weapons[original].information;
        RemoveWeapon(to);
        SetWeapon(to, o.name, o.tier);
        RemoveWeapon(original);
        SetWeapon(original, t.name, t.tier);
      }
    }

    [ContextMenu("Set Weapon for test")]
    public void Test()
    {
      SetWeapon(0, "knife", 0);
      // SetWeapon(1, "melee/testaxe");
      // SetWeapon(2, "ranged/testgun");
      // SetWeapon(3, "ranged/testbomb");
    }
  }
}