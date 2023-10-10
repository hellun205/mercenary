using System;
using Interact;
using UnityEngine;

namespace Weapon
{
  public class AttackableObject : Interacter
  {
    public float damage { get; set; }

    public float bleeding { get; set; }

    public const int bleedingCount = 3;

    public bool isCritical { get; set; }

    public float knockBack { get; set; }

    public float multipleDamage { get; set; }
    public bool canAttack = true;

    private void Reset()
    {
      caster = InteractCaster.Player;
      currentCondition = InteractCondition.Attack;
    }
  }
}
