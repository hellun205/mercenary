using System;
using Manager;
using Pool;
using UnityEngine;
using Weapon;

namespace Coin
{
  [RequireComponent(typeof(PoolObject))]
  public class CoinChest : TargetableObject
  {
    public int coin;

    public override float maxHp => coin;

    private TargetableObject to;
    
    private void Awake()
    {
      to = GetComponent<TargetableObject>();
      to.onDead += ToOnonDead;
    }

    private void ToOnonDead()
    {
      GameManager.Pool.Summon("effect/chest_blood", transform.position);
      for (var i = 0; i < coin; i++)
        GameManager.Pool.Summon("object/coin", transform.position);
    }
  }
}