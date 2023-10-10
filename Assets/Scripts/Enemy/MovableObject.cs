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

    private SpriteRenderer sr;

    private void Awake()
    {
      sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
      if (!canMove) return;

      // transform.rotation = transform.GetRotationOfLookAtObject(GameManager.Player.transform);
      var dir = (GameManager.Player.transform.position - transform.position).normalized;
      transform.Translate(dir * (Time.deltaTime * Mathf.Min((speed + increasedMoveSpeed), maxSpeed)));
      // sr.flipX =;

      var scale = transform.localScale;
      transform.localScale = scale.Setter(x: Mathf.Abs(scale.x) *  dir.x > 0 ? -1 : 1);
      
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
