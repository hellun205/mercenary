using UnityEngine;

namespace Weapon
{
  [CreateAssetMenu(menuName = "Status/Normal")]
  public class TargetableStatus : ScriptableObject
  {
    public float maxHp;
  }
}