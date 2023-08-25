using Manager;
using UnityEngine;

namespace Weapon.Ranged
{
  public class RangedWeaponController : WeaponController<RangedWeapon>
  {
    public Transform firePosition;
    public string bullet;
  
    protected override void OnFire()
    {
      // var bulletObj = Instantiate(GameManager.Prefab.Get(bullet), firePosition.position, Quaternion.identity)
      //  .GetComponent<BulletController>();
      var bulletObj = GameManager.Pool.Summon<BulletController>(bullet, firePosition.position);
      
      bulletObj.SetTarget(target, weaponData.status.attackDamage);
    }
  }
}
