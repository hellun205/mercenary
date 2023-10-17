using System;
using Manager;
using Pool;
using UnityEngine;
using Util;

namespace Coin
{
  [RequireComponent(typeof(PoolObject))]
  public class CoinController : MonoBehaviour
  {
    public bool isFollowing;
    
    public bool isLock;
    private Timer timer = new Timer();

    private static Transform target => GameManager.Player.transform;

    [NonSerialized]
    public PoolObject po;

    private void Awake()
    {
      po = GetComponent<PoolObject>();
      po.onGet += () =>
      {
        isFollowing = false;
        isLock = true;
        timer.Start();
      };
      timer.onEnd += _ => isLock = false;
      timer.duration = 0.6f;
    }

    public void Update()
    {
      if (!isFollowing || isLock) return;

      transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * 10f);
    }
  }
}