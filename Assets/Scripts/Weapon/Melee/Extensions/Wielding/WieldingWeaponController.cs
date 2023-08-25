using UnityEngine;

namespace Weapon.Melee.Extensions.Wielding
{
  [RequireComponent(typeof(Animator))]
  public class WieldingWeaponController : WeaponController<WieldingWeapon>
  {
    private Animator anim;

    protected override void Awake()
    {
      base.Awake();
      anim = GetComponent<Animator>();
    }

    protected override void OnFire()
    {
      
    }
  }
}