using Manager;
using Sound;
using UnityEngine;
using UnityEngine.Serialization;

namespace Coin
{
  [RequireComponent(typeof(CircleCollider2D))]
  public class CoinExplorer : MonoBehaviour
  {
    public float range = 20f;

    [FormerlySerializedAs("collider")]
    [SerializeField]
    private CircleCollider2D col;

    private void Reset()
    {
      col = GetComponent<CircleCollider2D>();
      col.radius = range;
      col.isTrigger = true;
      col.includeLayers = LayerMask.GetMask("Coin");
      col.excludeLayers = int.MaxValue;
    }

    private void OnValidate()
    {
      col.radius = range;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (!other.TryGetComponent<CoinController>(out var coinController)) return;
      coinController.isFollowing = true;
      GameManager.Sound.Play(SoundType.SFX_Normal, "sfx/normal/pickup");
    }
  }
}
