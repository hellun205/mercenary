using UnityEngine;

namespace Manager
{
  public class SpriteCollection : ObjectCollection<Sprite>
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
