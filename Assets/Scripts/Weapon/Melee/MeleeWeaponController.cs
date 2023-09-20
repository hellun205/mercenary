using DG.Tweening;
using Interact;
using Manager;
using UnityEngine;

namespace Weapon.Melee
{
  public class MeleeWeaponController : WeaponController<MeleeWeapon>
  {
    [SerializeField]
    protected Transform movingObj;

    [SerializeField]
    protected AttackableObject attackableObject;

    private void Start()
    {
      GameManager.Wave.onWaveStart += () => ApplyDamage(attackableObject);
    }

    protected override void OnFire()
    {
      isAttacking = true;
      attackableObject.RemoveDetection();
      attackableObject.currentCondition = InteractCondition.Attack;

      movingObj.DOLocalMoveX(status.fireRange / 10, 0.05f * Mathf.Min(status.attackSpeed, 1f))
       .OnComplete(() =>
        {
          isAttacking = false;
          movingObj.DOLocalMoveX(0f, 0.15f * Mathf.Min(status.attackSpeed, 1f))
           .OnComplete(() => attackableObject.currentCondition = InteractCondition.Normal);
        });
    }
  }
}
