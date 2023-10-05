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

    public List<(string name, int tier)?> weapons = new();

    public List<Transform> weaponSlots;

    public Transform weaponParent;

    public event Action onChanged;

    public bool isPartnerOwner { get; set; }

    public Func<IncreaseStatus> statusGetter { get; set; }

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

      weapons[slot] = (weaponName, tier);
      var obj = Instantiate(GameManager.Weapons.Get(weaponName), weaponSlots[slot].transform.position,
        Quaternion.identity,
        weaponSlots[slot]);
      obj.name = weaponName;
      var wc = obj.GetComponent<WeaponController>();
      wc.additionalStatusGetter = statusGetter;
      wc.isAdditionalStatus = isPartnerOwner;
      ((ITier) obj.GetComponent(typeof(ITier))).tier = tier;
      
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
        SetWeapon(to, weapons[original]!.Value.name, weapons[original].Value.tier);
        RemoveWeapon(original);
      }
      else
      {
        var t = (weapons[to]!.Value.name, weapons[to].Value.tier);
        RemoveWeapon(to);
        SetWeapon(to, weapons[original]!.Value.name, weapons[original].Value.tier);
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
