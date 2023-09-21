using Manager;
using UnityEngine;

namespace Weapon.Ranged
{
  public class RangedWeaponController : WeaponController<RangedWeaponData>
  {
    [SerializeField]
    protected Transform firePosition;
  
    protected override void OnFire()
    {
      for (var i = 0; i < weaponData.bulletCount; i++)
      {
        var bulletObj = GameManager.Pool.Summon<Bullet>(GetPObj(weaponData.bullet), firePosition.position);
        ApplyDamage(bulletObj);
        bulletObj.speed = weaponData.bulletSpeed;
        bulletObj.maxPenetrateCount = weaponData.penetrate;
        bulletObj.SetTarget(target, weaponData.errorRange);
      }
    }
  }
}
