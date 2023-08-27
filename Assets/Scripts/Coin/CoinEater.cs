using System;
using Manager;
using Pool;
using TMPro;
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
        GameManager.UI.Find<TextMeshProUGUI>("$coin").text = $"{coin}";
        // Destroy(other.gameObject);
        other.GetComponent<PoolObject>().Release();
      }
    }
  }
}