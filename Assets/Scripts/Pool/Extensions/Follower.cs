using UnityEngine;

namespace Pool.Extensions
{
  public class Follower :MonoBehaviour , IUsePool
  {
    public PoolObject poolObject { get; set; }
    
    private PoolObject target;

    public bool isEnabled => target is not null;

    public void OnKilled()
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
      poolObject.Release();
    }

    private void Update()
    {
      if (!isEnabled) return;

      transform.position = target.position;
    }
  }
}
