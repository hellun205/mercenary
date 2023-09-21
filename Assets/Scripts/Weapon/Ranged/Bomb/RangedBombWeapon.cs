using Manager;
using UnityEngine;

namespace Weapon.Ranged.Bomb
{
  public class RangedBombWeapon : WeaponController<RangedBombWeaponData>
  {
    [SerializeField]
    protected Transform firePosition;

    protected override void OnFire()
    {
      if (weaponData.isFixedTargetPosition)
      {
        var bulletObj = GameManager.Pool.Summon<GranadeBullet>(GetPObj(weaponData.bullet), firePosition.position);
        ApplyDamage(bulletObj);
        bulletObj.targetPosition = target.transform.position;
        bulletObj.maxPenetrateCount = 0;
        bulletObj.explosionTimer.duration = weaponData.explosionDelay;
        bulletObj.mainCtrler = this;
        bulletObj.SetTarget(target);
        bulletObj.SetRange(weaponData.explosionRange);
        bulletObj.OnStart();
      }
      else
      {
        var bulletObj = GameManager.Pool.Summon<ExplosionBullet>(GetPObj(weaponData.bullet), firePosition.position);
        ApplyDamage(bulletObj);
        bulletObj.targetPosition = target.transform.position;
        bulletObj.maxPenetrateCount = 0;
        bulletObj.mainCtrler = this;
        bulletObj.SetTarget(target);
        bulletObj.SetRange(weaponData.explosionRange);
        bulletObj.OnStart();
      }
    }
  }
}