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
      var bulletObj = GameManager.Pool.Summon<BombBullet>(GetPObj(weaponData.bullet), firePosition.position);
      ApplyDamage(bulletObj);
      bulletObj.targetPosition = target.transform.position;
      bulletObj.maxPenetrateCount = 0;
      bulletObj.explosionTimer.duration = weaponData.explosionDelay;
      bulletObj.mainCtrler = this;
      bulletObj.SetTarget(target);
      bulletObj.SetRange(weaponData.explosionRange);
    }
  }
}