using System;

namespace Weapon
{
  public enum ApplyStatus
  {
    BleedingDamage,
    CriticalPercent,
    MeleeDamage,
    RangedDamage,
    Knockback,
    Armor,
    Range,
    AttackSpeed,
    ExplosionRange
  }

  public static class ApplyStatusUtility
  {
    public static string GetText(this ApplyStatus applyStatus)
      => applyStatus switch
      {
        ApplyStatus.BleedingDamage  => "출혈 데미지",
        ApplyStatus.CriticalPercent => "치명타율",
        ApplyStatus.MeleeDamage     => "근거리 피해량",
        ApplyStatus.RangedDamage    => "원거리 피해량",
        ApplyStatus.Knockback       => "넉백",
        ApplyStatus.Armor           => "방어력",
        ApplyStatus.Range           => "범위",
        ApplyStatus.AttackSpeed     => "공격 속도",
        ApplyStatus.ExplosionRange  => "폭발 범위",
        _                           => throw new ArgumentOutOfRangeException()
      };

    public static float GetValue(this ApplyStatus applyStatus, float value)
      => applyStatus switch
      {
        ApplyStatus.Armor => value * 100f,
        ApplyStatus.AttackSpeed => value * 100f,
        ApplyStatus.CriticalPercent => value * 100f,
        _ => value,
      };
  }
}
