using System;
using Manager;
using Pool;
using UnityEngine;
using Util;
using Weapon;

namespace Enemy
{
  [RequireComponent(typeof(TargetableObject),typeof(PoolObject))]
  public class EnemyController : MonoBehaviour
  {
    [Header("Base Status")]
    public EnemyStatus status;

    private bool isEnabled = true;

    private Transform target;

    [NonSerialized]
    public PoolObject po;

    private void Awake()
    {
      po = GetComponent<PoolObject>();
      po.onGet += () =>
      {
        isEnabled = true;
      };
      po.onReleased += () =>
      {
        var count = 1;
        if (GameManager.Player.status.luck.ApplyPercentage())
          count++;
        
        for (var i = 0; i <count; i ++)
          GameManager.Spawn.Spawn(transform.position, "object/coin");
        
        isEnabled = false;
      };
    }

    private void Start()
    {
      target = GameManager.Player.transform;
    }

    private void Update()
    {
      if (!isEnabled) return;

      transform.rotation = transform.GetRotationOfLookAtObject(target.transform);
      transform.Translate(Vector3.right * (Time.deltaTime * status.moveSpeed));
    }

    private void OnTriggerStay2D(Collider2D other)
    {
      if (!other.CompareTag("Player")) return;
      
      GameManager.Player.Hit(status.damage);
    }
  }
}
