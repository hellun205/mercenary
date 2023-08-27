using UnityEngine;

namespace Weapon.Melee
{
  [RequireComponent(typeof(Collider2D))]
  [RequireComponent(typeof(Rigidbody2D))]
  public class MeleeWeaponCollider : MonoBehaviour
  {
    [SerializeField]
    private MeleeWeaponController mainCtrler;

    private void OnTriggerStay2D(Collider2D other)
    {
      mainCtrler.Attack(other.GetComponent<TargetableObject>());
    }
  }
}
