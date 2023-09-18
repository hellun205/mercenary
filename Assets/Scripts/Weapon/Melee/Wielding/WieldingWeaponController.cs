using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Interact;
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
    
    public bool ready = false;

    [SerializeField]
    private LockRotation lr;

    public TweenerCore<Vector3, Vector3, VectorOptions> tweener;

    protected override void Awake()
    {
      base.Awake();
      anim = GetComponent<Animator>();
    }

    protected override void Update()
    {
      base.Update();
      if (!isReady) return;

      var z = transform.localRotation.eulerAngles.z;
      var s = transform.localScale;
      var l = lr.rotation;
      
      
      lr.rotation = l.Setter(z:Mathf.Abs(l.z) * z is (> 90 and < 270) or (< -90 and > 270) ? -1 : 1);
      cRotate.localScale = s.Setter(x: Mathf.Abs(s.x) * z is (> 90 and < 270) or (< -90 and > 270) ? -1 : 1);
    }

    protected override void OnFire()
    {
      ready = false;
      // moveCrt.Start();
      tweener = movingObj.DOLocalMoveX(status.fireRange, 0.3f * Mathf.Min(1, status.attackSpeed))
       .OnComplete(StartWielding);
    }

    public void StartWielding()
    {
      tweener.Kill();
      attackableObject.currentCondition = InteractCondition.Attack;
      anim.SetFloat("speed", status.attackSpeed);
      anim.Play("");
    }
    
    public void OnAttack() => isAttacking = true;

    public void EndAttack()
    {
      isAttacking = false;
      movingObj.DOLocalMoveX(0f, 0.4f * Mathf.Min(1, status.attackSpeed));
      attackableObject.currentCondition = InteractCondition.Normal;
    }
  }
}
