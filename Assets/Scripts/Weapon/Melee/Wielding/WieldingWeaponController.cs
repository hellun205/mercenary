using System.Collections;
using Manager;
using UnityEngine;
using Util;
using Weapon.Extensions;

namespace Weapon.Melee.Wielding
{
  [RequireComponent(typeof(Animator))]
  public class WieldingWeaponController : MeleeWeaponController
  {
    [SerializeField]
    private Transform cRotate;

    private Animator anim;

    public string animName;

    public Coroutiner moveCrt;
    public Coroutiner moveCrt2;

    public bool ready = false;

    [SerializeField]
    private LockRotation lr;

    protected override void Awake()
    {
      base.Awake();
      anim = GetComponent<Animator>();
      moveCrt = new Coroutiner(MoveCRT);
      moveCrt2 = new Coroutiner(MoveCRT2);
    }

    protected override void Update()
    {
      base.Update();
      if (!isReady) return;

      var z = transform.localRotation.eulerAngles.z;
      var s = transform.localScale;
      var l = lr.rotation;
      lr.rotation = l.Setter(Mathf.Abs(l.z) * z is (> 90 and < 270) or (< -90 and > 270) ? -1 : 1);
      cRotate.localScale = s.Setter(x: Mathf.Abs(s.x) * z is (> 90 and < 270) or (< -90 and > 270) ? -1 : 1);
    }

    protected override void OnFire()
    {
      ready = false;
      attackedTargets.Clear();
      moveCrt.Start();
    }

    private IEnumerator MoveCRT()
    {
      var time = 0f;
      moveCrt2.Stop();

      while (!movingObj.localPosition.x.ApproximatelyEqual(weaponData.GetRange()))
      {
        yield return new WaitForEndOfFrame();

        time = Time.deltaTime * weaponData.GetAttackSpeed() * 4f;

        var pos = movingObj.localPosition;
        movingObj.localPosition = new Vector3(Mathf.Lerp(pos.x, weaponData.GetRange(), time), pos.y, pos.z);
      }

      if (!ready)
      {
        ready = true;
        StartWielding();
      }
    }

    private IEnumerator MoveCRT2()
    {
      var time = 0f;
      var tmp = movingObj.localPosition.x;

      while (!movingObj.localPosition.x.ApproximatelyEqual(0f))
      {
        yield return new WaitForEndOfFrame();

        time = Time.deltaTime * weaponData.GetAttackSpeed()* 5f;

        var pos = movingObj.localPosition;
        movingObj.localPosition = new Vector3(Mathf.Lerp(pos.x, 0f, time), pos.y, pos.z);
      }
    }

    public void StartWielding()
    {
      moveCrt.Stop();
      anim.SetFloat("speed", weaponData.GetAttackSpeed());
      anim.Play("");
    }


    public void OnAttack() => isAttacking = true;

    public void EndAttack()
    {
      isAttacking = false;
      moveCrt2.Start();
    }
  }
}
