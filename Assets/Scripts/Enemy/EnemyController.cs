using Manager;
using Pool.Extensions;
using UnityEngine;
using Util;
using Weapon;

namespace Enemy
{
  [RequireComponent(typeof(TargetableObject))]
  public class EnemyController : UsePool
  {
    [Header("Base Status")]
    public EnemyStatus status;

    private bool isEnabled = true;

    private Transform target;

    private TargetableObject to;

    protected override void OnSummon()
    {
      isEnabled = true;
    }

    protected override void Awake()
    {
      to = GetComponent<TargetableObject>();
      base.Awake();
    }

    protected override void OnKilled()
    {
      var count = 1;
      if (GameManager.Player.status.luck.ApplyPercentage())
        count++;

      for (var i = 0; i < count; i++)
        GameManager.Spawn.Spawn(transform.position, "object/coin");

      isEnabled = false;
    }

    private void Start()
    {
      target = GameManager.Player.transform;
    }

    private void Update()
    {
      if (!isEnabled || !to.canTarget) return;

      transform.rotation = transform.GetRotationOfLookAtObject(target.transform);
      transform.Translate(Vector3.right * (Time.deltaTime * status.moveSpeed));
    }

    private void OnTriggerStay2D(Collider2D other)
    {
      if (!other.CompareTag("Player") || !to.canTarget) return;

      GameManager.Player.Hit(status.damage, po.index);
    }
  }
}
