using System;
using Manager;
using UnityEngine;

namespace Weapon
{
  public class RangedWeaponController : WeaponController
  {
    public Transform firePosition;
    public string bullet;
  
    protected override void OnFire()
    {
      var bulletObj = Instantiate(GameManager.Prefab.Get(bullet), firePosition.position, Quaternion.identity)
       .GetComponent<BulletController>();
      
      bulletObj.SetTarget(target, weaponData.status.attackDamage);
    }
  }
}
