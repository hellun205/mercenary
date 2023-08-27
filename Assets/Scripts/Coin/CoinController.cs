using System;
using Manager;
using Pool;
using UnityEngine;

namespace Coin
{
  [RequireComponent(typeof(PoolObject))]
  public class CoinController : MonoBehaviour
  {
    public bool isFollowing;

    private static Transform target => GameManager.Player.transform;

    [NonSerialized]
    public PoolObject po;

    private void Awake()
    {
      po = GetComponent<PoolObject>();
      po.onGet += () => isFollowing = false;
    }

    public void Update()
    {
      if (!isFollowing) return;

      transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * 10f);
    }
  }
}