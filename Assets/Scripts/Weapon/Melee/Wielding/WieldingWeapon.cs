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
  public class WieldingWeapon : MeleeWeapon
  {
    [SerializeField]
    private Transform cRotate;
    
    public bool ready = false;

    [SerializeField]
    private LockRotation lr;

    public TweenerCore<Vector3, Vector3, VectorOptions> tweener;

    protected override void FlipY(float z)
    {
      transform.localScale = transform.localScale.Setter(y: z is (<= 90 and > -90) ? 1 : -1);
    }

    protected override void OnFire()
    {
      ready = false;
      // moveCrt.Start();
      tweener = movingObj.DOLocalMoveX(status.fireRange / 10, 0.3f * Mathf.Min(1, status.attackSpeed))
       .OnComplete(StartWielding);
    }

    public void StartWielding()
    {
      tweener.Kill();
      attackableObject.currentCondition = InteractCondition.Attack;
      StartAnimation();
      PlayFireSound();
    }
    
    public void OnAttack() => isAttacking = true;

    public void EndAttackFirst()
    {
      isAttacking = false;
      attackableObject.currentCondition = InteractCondition.Normal;
    }

    public void EndAttackSecond()
    {
      movingObj.DOLocalMoveX(0f, 0.4f * Mathf.Min(1, status.attackSpeed));
    }
  }
}
