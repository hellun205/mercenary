using Manager;
using UnityEngine;

namespace Pool.Extensions
{
  public class PoolIdDisplayer : UsePool
  {
    private Follower followerText;

    [SerializeField]
    private Color color;

    protected override void OnSummon()
    {
      GameManager.Pool.Summon<FollowingText>("ui/followingtext", po.position, obj =>
      {
        obj.follower.SetTarget(po);
        obj.value = $"<size=5>{po.index}</size>";
        obj.color = color;
        obj.bgColor = new Color(0f, 0f, 0f, 0.3f);
      });
    }
    
  }
}
