using UnityEngine;

namespace Weapon.Melee.Wielding
{
  [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
  public class WieldingWeaponCollider : MonoBehaviour
  {
    [SerializeField]
    private WieldingWeaponController mainCtrler;

    private void OnTriggerStay2D(Collider2D col)
    {
      if (!mainCtrler.ready)
      {
        mainCtrler.moveCrt.Stop();
        mainCtrler.ready = true;
        mainCtrler.StartWielding();
      }
    }
  }
}
