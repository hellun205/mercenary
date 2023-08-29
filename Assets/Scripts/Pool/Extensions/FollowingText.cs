using UnityEngine;

namespace Pool.Extensions
{
  [RequireComponent(typeof(Follower))]
  public class FollowingText : Text
  {
    public Follower follower;

    protected override void Awake()
    {
      base.Awake();
      
      follower = GetComponent<Follower>();
    }
  }
}
