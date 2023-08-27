using UnityEngine;

namespace Weapon.Extensions
{
  public class LockRotation : MonoBehaviour
  {
    public Vector3 rotation;

    private void Update()
    {
      transform.rotation = Quaternion.Euler(rotation);
    }
  }
}
