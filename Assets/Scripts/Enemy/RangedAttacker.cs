using System;
using Manager;
using Pool;
using UnityEngine;
using Util;

namespace Enemy
{
  public class RangedAttacker : MonoBehaviour ,IUsePool
  {
    public PoolObject poolObject { get; set; }

    [SerializeField]
    private Detector detector;

    [SerializeField]
    private Timer fireTimer = new ();
    
    public Func<float> bulletSpeedGetter { private get; set; }
    public Func<float> damageGetter { private get; set; }
    public Func<float> fireDurationGetter { private get; set; }
    public Func<float> detectRangeGetter { private get; set; }

    private void Awake()
    {
      fireTimer.onEnd += OnTimerEnd;
    }

    private void OnTimerEnd(Timer sender)
    {
      Fire();
      CheckDetect();
    }

    private void CheckDetect()
    {
      CoroutineUtility.WaitUntil(() => detector.isDetected, () => fireTimer.Start());
    }

    private void Fire()
    {
      GameManager.Pool.Summon<EnemyBullet>($"enemy/{poolObject.originalName}/bullet", transform.position, obj =>
      {
        obj.bulletSpeed = bulletSpeedGetter.Invoke();
        obj.damage = damageGetter.Invoke();
        obj.SetTarget();
      });
    }

    private void Update()
    {
      detector.transform.position = transform.position;
    }

    public void OnKilled()
    {
      fireTimer.Stop();
    }

    public void Ready()
    {
      fireTimer.duration = fireDurationGetter.Invoke();
      detector.GetComponent<CircleCollider2D>().radius = detectRangeGetter.Invoke() / 10f;
      CheckDetect();
    }
  }
}
