using Manager;
using UnityEngine;

namespace Weapon
{
  public class WeaponDataCollection : ObjectCollection<Weapon>
  {
#if UNITY_EDITOR
    private void Reset()
    {
      exts = "asset";
    }
    
    [ContextMenu("Apply Assets")]
    private void ApplyAsset() => FindObjectsAndAddToList();
#endif
  }
}
