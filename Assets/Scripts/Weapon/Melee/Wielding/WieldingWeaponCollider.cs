using DG.Tweening;
using UnityEngine;

namespace Weapon.Melee.Wielding
{
  [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
  public class WieldingWeaponCollider : MonoBehaviour
  {
    [SerializeField]
    private WieldingWeapon mainCtrler;

    private void OnTriggerStay2D(Collider2D col)
    {
      if (!mainCtrler.ready)
      {
        mainCtrler.tweener.Kill();
        mainCtrler.ready = true;
        mainCtrler.StartWielding();
      }
    }
  }
}
