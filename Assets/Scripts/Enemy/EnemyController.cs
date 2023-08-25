using System;
using Manager;
using Pool;
using Spawn;
using UnityEngine;
using Util;
using Weapon;

namespace Enemy
{
  [RequireComponent(typeof(TargetableObject))]
  [RequireComponent(typeof(PoolObject))]
  public class EnemyController : MonoBehaviour
  {
    [Header("Base Status")]
    public EnemyStatus status;
    
    // [Header("Variable Status")]
    
    public bool isEnabled = true;

    private Transform target;

    private PoolObject po;

    private void Awake()
    {
      po = GetComponent<PoolObject>();
      po.onGet += () =>
      {
        isEnabled = true;
      };
      po.onReleased += () =>
      {
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
  }
}
