using System;
using Manager;
using TMPro;
using UnityEngine;

namespace Pool.Extensions
{
  [RequireComponent(typeof(PoolObject))]
  public class PoolIdDisplayer : MonoBehaviour
  {
    private Follower followerText;

    private PoolObject po;

    private bool isEnabled;

    [SerializeField]
    private Color color;

    private void Awake()
    {
      po = GetComponent<PoolObject>();

      po.onGet += () =>
      {
        GameManager.Pool.Summon<FollowingText>("ui/followingtext", po.position, obj =>
        {
          obj.follower.SetTarget(po);
          obj.value = $"{po.index}";
          obj.color = color;
          obj.text.fontSize = 5;
          obj.bgColor = new Color(0f, 0f, 0f, 0.3f);
        });
      };
    }

    private void Update()
    {
      if (!isEnabled) return;
    }
  }
}
