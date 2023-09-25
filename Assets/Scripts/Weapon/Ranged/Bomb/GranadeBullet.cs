using UnityEngine;
using Util;

namespace Weapon.Ranged.Bomb
{
  public class GranadeBullet : ExplosionBullet
  {
    [SerializeField]
    private Timer moveTimer = new();
    
    public Timer explosionTimer = new();
    
    private Vector2 startPosition;

    protected override void Awake()
    {
      base.Awake();
      explosionTimer.onEnd += OnExplosionTimerEnd;
      moveTimer.onBeforeStart += OnMoveTimerStart;
      moveTimer.onTick += OnMoveTimerTick;
    }

    public override void OnKilled()
    {
      base.OnKilled();
      moveTimer.Stop();
      explosionTimer.Stop();
    }

    public override void OnStart()
    {
      base.OnStart();
      moveTimer.Start();
      explosionTimer.Start();
    }
    
    private void OnExplosionTimerEnd(Timer sender)
    {
      Fire();
    }
    
    private void OnMoveTimerStart(Timer sender)
    {
      startPosition = transform.position;
    }

    private void OnMoveTimerTick(Timer sender)
    {
      transform.position = Vector2.Lerp(startPosition, targetPosition, sender.value);
    }

    protected override void Move()
    {
    }
  }
}