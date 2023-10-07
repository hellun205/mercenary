using Manager;
using UnityEngine;

namespace Weapon.Ranged
{
  public class RangedWeaponController : WeaponController
  {
    [SerializeField]
    protected Transform firePosition;
  
    protected override void OnFire()
    {
      StartAnimation();
      var stat = weaponData.status[tier];
      for (var i = 0; i < stat.bulletCount; i++)
      {
        var bulletObj = GameManager.Pool.Summon<Bullet>(GetPObj(weaponData.bullet), firePosition.position);
        ApplyDamage(bulletObj);
        bulletObj.speed = stat.bulletSpeed;
        bulletObj.maxPenetrateCount = stat.penetrate;
        bulletObj.SetTarget(transform.eulerAngles.z, stat.errorRange);
      }
    }
  }
}
