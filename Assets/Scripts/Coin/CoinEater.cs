using System;
using Manager;
using UnityEngine;

namespace Coin
{
  [RequireComponent(typeof(CircleCollider2D))]
  public class CoinEater : MonoBehaviour
  {
    public static int coin = 0;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
      if (other.CompareTag("Coin"))
      {
        coin++;
        GameManager.UI.Find("$coin").text = $"{coin}";
        Destroy(other.gameObject);
      }
    }
  }
}