using UnityEngine;

namespace Pool.Extensions
{
  [RequireComponent(typeof(Follower))]
  public class FollowingText : Text
  {
    public Follower follower;

    private void Awake()
    {
      follower = GetComponent<Follower>();
    }
  }
}
