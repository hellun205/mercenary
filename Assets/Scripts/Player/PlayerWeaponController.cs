using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
  public class PlayerWeaponController : MonoBehaviour
  {
    public List<Weapon.Weapon> weapons;

    public Transform weaponParent;

    private void Awake()
    {
      weaponParent = transform.Find("@weapons");
      if (weaponParent is null)
      {
        var go =  new GameObject("@weapons");
        go.transform.SetParent(transform);

        weaponParent = go.transform;
      }
    }
  }
}