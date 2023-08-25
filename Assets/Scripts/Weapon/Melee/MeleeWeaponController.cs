using System.Collections;
using UnityEngine;
using Util;

namespace Weapon.Melee
{
  public class MeleeWeaponController : WeaponController<MeleeWeapon>
  {
    [SerializeField]
    private Transform movingObj;

    // private Coroutine fireCrt;
    private Coroutiner fireCrt;

    protected override void Awake()
    {
      base.Awake();
      
      fireCrt = new Coroutiner(FireCRT);
    }

    protected override void OnFire()
    {
      Debug.Log("Fire!");
      // var bulletObj = Instantiate(GameManager.Prefab.Get(bullet), firePosition.position, Quaternion.identity)
      //   .GetComponent<BulletController>();

      // bulletObj.SetTarget(target, weaponData.status.attackDamage);
      fireCrt.Start();
    }

    private IEnumerator FireCRT()
    {
      var time = 0f;
      var tmp = 0f;
      SetTarget(target);

      while (movingObj.localPosition.x < weaponData.status.fireRange * 0.9)
      {
        yield return new WaitForEndOfFrame();

        time += Time.deltaTime * 2;

        var pos = movingObj.localPosition;
        tmp = pos.x;
        movingObj.localPosition = new Vector3(Mathf.Lerp(tmp, weaponData.status.fireRange, time), pos.y, pos.z);
      }

      Attack();

      time = 0f;

      while (movingObj.localPosition.x > 0)
      {
        yield return new WaitForEndOfFrame();

        time += Time.deltaTime * 2;

        var pos = movingObj.localPosition;
        tmp = pos.x;
        movingObj.localPosition = new Vector3(Mathf.Lerp(tmp, 0f, time), pos.y, pos.z);
      }
    }
  }
}