using UnityEngine;

namespace Util
{
  public static class Utils
  {
    public static float GetAngleOfLookAtObject(this Transform sender, Transform target)
    {
      var offset = target.transform.position - sender.position;
      return Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
    }

    public static Quaternion GetAngleOfLookAtObject(this Quaternion rotation, Transform sender, Transform target)
    {
      var euler = rotation.eulerAngles;
      return Quaternion.Euler(euler.x, euler.y, GetAngleOfLookAtObject(sender, target));
    }
  }
}
