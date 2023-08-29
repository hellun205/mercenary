using System;
using System.Collections.Generic;
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
    
    private List<int> ateCoins = new();

    private TextMeshProUGUI coinText;

    private void Awake()
    {
      coinText = GameManager.UI.Find<TextMeshProUGUI>("$coin");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (other.CompareTag("Coin"))
      {
        var po = other.GetComponent<PoolObject>();
        ateCoins.Add(po.index);
        po.Release();
        coinText.text = $"{++coin}";
      }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
      if (other.CompareTag("Coin"))
      {
        var po = other.GetComponent<PoolObject>();
        if (ateCoins.Contains(po.index))
          ateCoins.Remove(po.index);
      }
    }
  }
}