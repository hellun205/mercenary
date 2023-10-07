using Manager;
using UnityEngine;

namespace Weapon.Ranged.Bomb
{
  public class RangedBombWeapon : WeaponController
  {
    [SerializeField]
    protected Transform firePosition;

    protected override void OnFire()
    {
      StartAnimation();
      var stat = weaponData.status[tier]; 
      for (var i = 0; i < stat.bulletCount; i++)
      {
        if (weaponData.isFixedTargetPosition)
        {
          var bulletObj = GameManager.Pool.Summon<GranadeBullet>(GetPObj(weaponData.bullet), firePosition.position);
          ApplyDamage(bulletObj);
          bulletObj.targetPosition = target.transform.position;
          bulletObj.speed = stat.bulletSpeed;
          bulletObj.maxPenetrateCount = 0;
          bulletObj.explosionTimer.duration = weaponData.explosionDelay;
          bulletObj.mainCtrler = this;
          bulletObj.SetTarget(transform.eulerAngles.z, stat.errorRange);
          bulletObj.SetRange(stat.explosionRange + GameManager.Player.currentStatus.explosionRange);
          bulletObj.OnStart();
        }
        else
        {
          var bulletObj = GameManager.Pool.Summon<ExplosionBullet>(GetPObj(weaponData.bullet), firePosition.position);
          ApplyDamage(bulletObj);
          bulletObj.targetPosition = target.transform.position;
          bulletObj.speed = stat.bulletSpeed;
          bulletObj.maxPenetrateCount = 0;
          bulletObj.mainCtrler = this;
          bulletObj.SetTarget(transform.eulerAngles.z, stat.errorRange);
          bulletObj.SetRange(stat.explosionRange + GameManager.Player.currentStatus.explosionRange);
          bulletObj.OnStart();
        }
      }
    }
  }
}
