using System;
using Manager;
using Spawn;
using UnityEngine;
using Util;
using Weapon;

namespace Enemy
{
  [RequireComponent(typeof(TargetableObject))]
  public class EnemyController : SpawnableObject
  {
    public float moveSpeed = 2f;

    private Transform target;

    public bool isEnabled = true;

    private void Start()
    {
      target = GameManager.Player.transform;
    }

    private void Update()
    {
      if (!isEnabled) return;

      transform.rotation = transform.GetRotationOfLookAtObject(target.transform);
      transform.Translate(Vector3.right * (Time.deltaTime * moveSpeed));
    }
  }
}
