using System.Collections;
using Manager;
using UnityEngine;

namespace Weapon
{
  public class MeleeWeaponController : WeaponController
  {
    [SerializeField]
    private Transform movingObj;

    private Coroutine fireCrt;

    protected override void OnFire()
    {
      Debug.Log("Fire!");
      // var bulletObj = Instantiate(GameManager.Prefab.Get(bullet), firePosition.position, Quaternion.identity)
      //   .GetComponent<BulletController>();

      // bulletObj.SetTarget(target, weaponData.status.attackDamage);
      if (fireCrt is not null) StopCoroutine(fireCrt);
      fireCrt = StartCoroutine(FireCRT());
    }

    private IEnumerator FireCRT()
    {
      var time = 0f;
      var tmp = 0f;
      var curTarget = target;

      while (movingObj.localPosition.x < weaponData.status.range * 0.9)
      {
        yield return new WaitForEndOfFrame();

        time += Time.deltaTime * 2;

        var pos = movingObj.localPosition;
        tmp = pos.x;
        movingObj.localPosition = new Vector3(Mathf.Lerp(tmp, weaponData.status.range, time), pos.y, pos.z);
      }

      try
      {
        curTarget.Hit(weaponData.status.attackDamage);
      }
      catch
      {
      }

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