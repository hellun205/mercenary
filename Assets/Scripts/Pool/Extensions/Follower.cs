using UnityEngine;

namespace Pool.Extensions
{
  public class Follower : UsePool
  {
    private PoolObject target;

    public bool isEnabled => target is not null;

    protected override void OnKilled()
    {
      target = null;
    }

    public void SetTarget(PoolObject obj)
    {
      target = obj;
      target.onReleased += TargetOnReleased;
    }

    private void TargetOnReleased()
    {
      target.onReleased -= TargetOnReleased;
      po.Release();
    }

    private void Update()
    {
      if (!isEnabled) return;

      transform.position = target.position;
    }
  }
}
