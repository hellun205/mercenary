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
    private AttackableObject attackableObject;
    
    private void Start()
    {
      GameManager.Wave.onWaveStart += () => ApplyDamage(attackableObject);
    }

    protected override void OnFire()
    {
      isAttacking = true;
      attackableObject.currentCondition = InteractCondition.Attack;

      movingObj.DOLocalMoveX(status.fireRange, 0.05f * Mathf.Min(status.attackSpeed, 1f))
       .OnComplete(() =>
        {
          isAttacking = false;
          attackableObject.currentCondition = InteractCondition.Normal;
          movingObj.DOLocalMoveX(0f, 0.15f * Mathf.Min(status.attackSpeed, 1f));
        });
    }
  }
}
