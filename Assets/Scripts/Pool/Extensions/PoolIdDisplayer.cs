using Manager;
using UnityEngine;

namespace Pool.Extensions
{
  public class PoolIdDisplayer : MonoBehaviour, IUsePool
  {
    public PoolObject poolObject { get; set; }

    private Follower followerText;

    [SerializeField]
    private Color color;

    public void OnSummon()
    {
      GameManager.Pool.Summon<FollowingText>("ui/followingtext", poolObject.position, obj =>
      {
        obj.follower.SetTarget(poolObject);
        obj.value = $"<size=5>{poolObject.index}</size>";
        obj.color = color;
        obj.bgColor = new Color(0f, 0f, 0f, 0.3f);
      });
    }
  }
}
