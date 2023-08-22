using System;
using Manager;
using UnityEngine;

namespace Weapon
{
  [RequireComponent(typeof(WeaponController))]
  public class RangedWeaponController : MonoBehaviour
  {
    public WeaponController baseController;
    public Transform firePosition;
    public string bullet;

    private void Reset()
    {
      baseController = GetComponent<WeaponController>();
    }

    private void Awake()
    {
      baseController.onFire = OnFire;
    }

    private void OnFire()
    {
      Debug.Log("Fire!");
      var bulletObj = Instantiate(GameManager.Prefab.Get(bullet), firePosition.position, Quaternion.identity)
       .GetComponent<BulletController>();
      
      bulletObj.SetTarget(baseController.target, baseController.weaponData.status.attackDamage);
    }
  }
}
