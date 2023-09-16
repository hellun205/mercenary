using Manager;
using UnityEngine;

namespace Weapon.Ranged
{
  public class RangedWeaponController : WeaponController<RangedWeapon>
  {
    [SerializeField]
    private Transform firePosition;
  
    protected override void OnFire()
    {
      var bulletObj = GameManager.Pool.Summon<BulletController>(GetPObj(weaponData.bullet), firePosition.position);
      bulletObj.maxPenetrateCount = weaponData.penetrate;
      bulletObj.SetTarget(target, status.rangedDamage);
    }
  }
}
