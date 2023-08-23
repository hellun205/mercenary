using System;
using Manager;
using UnityEngine;

namespace Coin
{
  public class CoinController : MonoBehaviour
  {
    public bool isFollowing;

    private static Transform target => GameManager.Player.transform;

    public void Update()
    {
      if (!isFollowing) return;

      transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * 10f);
    }
  }
}