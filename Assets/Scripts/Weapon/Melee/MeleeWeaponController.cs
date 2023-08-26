using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Weapon.Melee
{
  public class MeleeWeaponController : WeaponController<MeleeWeapon>
  {
    [SerializeField]
    protected Transform movingObj;

    private Coroutiner fireCrt;

    protected List<int> attackedTargets = new List<int>();

    protected override void Awake()
    {
      base.Awake();

      fireCrt = new Coroutiner(FireCRT);
    }

    protected override void OnFire()
    {
      attackedTargets.Clear();
      fireCrt.Start();
    }

    private IEnumerator FireCRT()
    {
      var time = 0f;
      var tmp = 0f;
      isAttacking = true;

      while (!movingObj.localPosition.x.ApproximatelyEqual(weaponData.status.fireRange))
      {
        yield return new WaitForEndOfFrame();

        time += Time.deltaTime * weaponData.status.attackSpeed;

        var pos = movingObj.localPosition;
        tmp = pos.x;
        movingObj.localPosition = new Vector3(Mathf.Lerp(tmp, weaponData.status.fireRange, time), pos.y, pos.z);
      }

      time = 0f;


      while (!movingObj.localPosition.x.ApproximatelyEqual(0))
      {
        yield return new WaitForEndOfFrame();

        time += Time.deltaTime * weaponData.status.attackSpeed;

        var pos = movingObj.localPosition;
        tmp = pos.x;
        movingObj.localPosition = new Vector3(Mathf.Lerp(tmp, 0f, time), pos.y, pos.z);
      }

      isAttacking = false;
    }

    public void Attack(TargetableObject targetObj)
    {
      try
      {
        if (!isAttacking || attackedTargets.Contains(targetObj.index)) return;
        attackedTargets.Add(targetObj.index);
        if (targetObj.canTarget && targetObj.gameObject.activeSelf)
          targetObj.Hit(weaponData.status.attackDamage);
      }
      catch (Exception ex)
      {
        Debug.Log("Error Attack target:" + ex.Message);
      }
    }
  }
}
