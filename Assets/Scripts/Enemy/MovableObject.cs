using System;
using Manager;
using Pool;
using UnityEngine;
using Util;

namespace Enemy
{
  public class MovableObject : MonoBehaviour, IUsePool
  {
    public PoolObject poolObject { get; set; }
    public bool canMove { get; set; } = false;
    public Func<float> moveSpeedGetter { get; set; }
    public Func<float> maxMoveSpeedGetter { get; set; }
    public Func<float> increaseAmountGetter { get; set; }

    private float speed;
    private float maxSpeed;
    private float increaseAmount;
    private float increasedMoveSpeed;

    private void Update()
    {
      if (!canMove) return;
      
      transform.rotation = transform.GetRotationOfLookAtObject(GameManager.Player.transform);
      transform.Translate(Vector3.right * (Time.deltaTime * Mathf.Min((speed + increasedMoveSpeed), maxSpeed)));

      increasedMoveSpeed += increaseAmount * Time.deltaTime;
    }

    public void OnSummon()
    {
      speed = moveSpeedGetter.Invoke();
      maxSpeed = maxMoveSpeedGetter.Invoke();
      increaseAmount = increaseAmountGetter.Invoke();
      increasedMoveSpeed = 0f;
    }
  }
}
