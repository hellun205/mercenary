using System;
using Interact;
using UnityEngine;

namespace Weapon
{
  public class AttackableObject : Interacter
  {
    [Header("Attack")]
    public float damage;

    public float bleeding;

    public static int bleedingCount = 3;

    public bool isCritical;

    public float knockBack;

    public float multipleDamage;

    private void Reset()
    {
      caster = InteractCaster.Player;
      currentCondition = InteractCondition.Attack;
    }
  }
}
