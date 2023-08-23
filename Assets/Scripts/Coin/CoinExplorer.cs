using System;
using UnityEngine;

namespace Coin
{
  [RequireComponent(typeof(CircleCollider2D))]
  public class CoinExplorer : MonoBehaviour
  {
    public float range = 20f;

    [SerializeField]
    private CircleCollider2D collider;
    
    private void Reset()
    {
      collider = GetComponent<CircleCollider2D>();
      collider.radius = range;
      collider.isTrigger = true;
      collider.includeLayers = LayerMask.GetMask("Coin");
      collider.excludeLayers = int.MaxValue;
    }

    private void OnValidate()
    {
      collider.radius = range;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      other.GetComponent<CoinController>().isFollowing = true;
    }
  }
}