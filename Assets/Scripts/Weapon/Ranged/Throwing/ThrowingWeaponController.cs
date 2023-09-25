using System.Collections;
using DG.Tweening;
using Manager;
using UnityEngine;
using Util;

namespace Weapon.Ranged.Throwing
{
  public class ThrowingWeaponController : WeaponController<ThrowingWeaponData>
  {
    protected override void OnFire()
    {
      // fireCrt.Start();
      var to = GameManager.Pool.Summon<ThrowingObjectController>(GetPObj(weaponData.throwingObj), transform.position);
      ApplyDamage(to);
      to.SetTarget(target.transform, this);

      sr.color = sr.color.Setter(a: 0f);

      sr.DOFade(1f, status.attackSpeed * 0.7f);
    }
  }
}