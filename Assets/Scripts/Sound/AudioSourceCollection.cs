using Manager;
using UnityEngine;

namespace Sound
{
  public class AudioSourceCollection : ObjectCollection<AudioClip>
  {
#if UNITY_EDITOR
    [ContextMenu("Apply Assets")]
    private void ApplyAsset() => FindObjectsAndAddToList();
#endif
  }
}
