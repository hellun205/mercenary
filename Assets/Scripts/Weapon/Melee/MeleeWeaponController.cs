using System.Collections;
using UnityEngine;
using Util;

namespace Weapon.Melee
{
  public class MeleeWeaponController : WeaponController<MeleeWeapon>
  {
    [SerializeField]
    protected Transform movingObj;

    // private Coroutine fireCrt;
    private Coroutiner fireCrt;

    protected override void Awake()
    {
      base.Awake();

      fireCrt = new Coroutiner(FireCRT);
    }

    protected override void Update()
    {
      base.Update();
     
      isAttacking = movingObj.localPosition.x > weaponData.status.fireRange * 0.3f;
    }

    protected override void OnFire()
    {
      base.OnFire();
      fireCrt.Start();
    }

    private IEnumerator FireCRT()
    {
      var time = 0f;
      var tmp = 0f;
      isAttacking = true;
      while (movingObj.localPosition.x < weaponData.status.fireRange * 0.9)
      {
        yield return new WaitForEndOfFrame();

        time += Time.deltaTime * 1.2f * weaponData.status.attackSpeed;

        var pos = movingObj.localPosition;
        tmp = pos.x;
        movingObj.localPosition = new Vector3(Mathf.Lerp(tmp, weaponData.status.fireRange, time), pos.y, pos.z);
      }

      time = 0f;


      while (movingObj.localPosition.x > 0)
      {
        yield return new WaitForEndOfFrame();

        time += Time.deltaTime * 1.2f * weaponData.status.attackSpeed;

        var pos = movingObj.localPosition;
        tmp = pos.x;
        movingObj.localPosition = new Vector3(Mathf.Lerp(tmp, 0f, time), pos.y, pos.z);
      }

      isAttacking = false;
    }
  }
}