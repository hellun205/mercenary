using System;
using Interact;
using UnityEngine;

namespace Weapon
{
  public class AttackableObject : Interacter
  {
    [Header("Attack")]
    public float damage;

    private void Reset()
    {
      caster = InteractCaster.Player;
      currentCondition = InteractCondition.Attack;
    }
  }
}
