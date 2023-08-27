using System.Collections;
using Manager;
using UnityEngine;
using Util;

namespace Weapon.Ranged.Throwing
{
  public class ThrowingWeaponController : WeaponController<ThrowingWeapon>
  {
    private Coroutiner fireCrt;

    protected override void Awake()
    {
      base.Awake();

      fireCrt = new Coroutiner(FireRoutine);
    }

    protected override void OnFire()
    {
      fireCrt.Start();
    }

    protected virtual IEnumerator FireRoutine()
    {
      var t = 0f;
      GameManager.Pool.Summon<ThrowingObjectController>(GetPObj(weaponData.throwingObj), transform.position,
        obj => obj.SetTarget(target.transform, this));
      sr.color = sr.color.Setter(a: 0f);
      yield return null;
      
      while (sr.color.a < 1)
      {
        sr.color = Color.Lerp(sr.color.Setter(a:0f), sr.color.Setter(a: 1f), t);
        t += Time.deltaTime * 1.5f * weaponData.GetAttackSpeed();
        yield return new WaitForEndOfFrame();
      }
    }
  }
}