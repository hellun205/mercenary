using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AYellowpaper.SerializedCollections;
using Manager;
using Player;
using UnityEngine;
using Util;
using Util.Text;

namespace Weapon
{
  [CreateAssetMenu(fileName = "Attribute Chemistry", menuName = "Weapon/Attribute Chemistry", order = 0)]
  public class AttributeChemistry : ScriptableObject
  {
    public SerializedDictionary<Attribute, SerializedDictionary<string, SerializedDictionary<ApplyStatus, float>>> data;

    public IncreaseStatus GetIncrease(Attribute attribute, int count)
    {
      var d = data[attribute];
      var c = d.Keys.LastOrDefault(x => int.Parse(x) <= count);

      if (c == default)
        return new IncreaseStatus();
      else
      {
        var res = new IncreaseStatus();

        foreach (var (status, value) in d[c])
        {
          switch (status)
          {
            case ApplyStatus.Armor:
              res.armor += value;
              break;

            case ApplyStatus.Knockback:
              res.knockback += value;
              break;

            case ApplyStatus.Range:
              res.range += value;
              break;

            case ApplyStatus.AttackSpeed:
              res.attackSpeedPercent += value;
              break;

            case ApplyStatus.CriticalPercent:
              res.criticalPercent += value;
              break;

            case ApplyStatus.RangedDamage:
              res.rangedDamage += value;
              break;

            case ApplyStatus.MeleeDamage:
              res.meleeDamage += value;
              break;

            case ApplyStatus.BleedingDamage:
              res.bleedingDamage += value;
              break;

            case ApplyStatus.ExplosionRange:
              res.explosionRange += value;
              break;

            default:
              throw new ArgumentOutOfRangeException();
          }
        }

        return res;
      }
    }

    public string GetDescription(Attribute attribute)
    {
      var sb = new StringBuilder();
      var d = data[attribute];
      GameManager.Player.GetChemistryStatus(out var counts);

      var i = counts.ContainsKey(attribute) ? d.Keys.LastOrDefault(x => int.Parse(x) <= counts[attribute]) : "0";

      sb.Append
        (
          attribute.GetText()
           .SetSizePercent(1.25f)
           .AddColor(new Color32(72, 156, 255, 255))
           .SetLineHeight(1.3f)
        )
       .Append("\n");

      foreach (var (count, value) in d)
      {
        var sb2 = new StringBuilder();

        foreach (var (status, f) in value)
          sb2.Append(status.GetText())
           .Append(' ')
           .Append(status.GetValue(f).GetViaValue())
           .Append("\n");

        sb2.Remove(sb2.ToString().LastIndexOf("\n"), 1);
        sb.Append
          (
            $"({count}){sb2.ToString().SetIndent(0.2f)}"
             .AddColor(i != default && i == count ? Color.white : Color.gray)
          )
         .Append("\n");
      }

      return sb.ToString();
    }

    public string[] GetDescriptions(Attribute attributeFlags)
    {
      var res = new List<string>();
      //
      // if ((attributeFlags & Attribute.Bluntness) != 0)
      //   res.Add(GetDescription(Attribute.Bluntness));
      // if ((attributeFlags & Attribute.Explosion) != 0)
      //   res.Add(GetDescription(Attribute.Explosion));
      // if ((attributeFlags & Attribute.Sharpness) != 0)
      //   res.Add(GetDescription(Attribute.Sharpness));
      // if ((attributeFlags & Attribute.AutomaticFire) != 0)
      //   res.Add(GetDescription(Attribute.AutomaticFire));
      // if ((attributeFlags & Attribute.Gun) != 0)
      //   res.Add(GetDescription(Attribute.Gun));
      // if ((attributeFlags & Attribute.Heavy) != 0)
      //   res.Add(GetDescription(Attribute.Heavy));
      // if ((attributeFlags & Attribute.Lazer) != 0)
      //   res.Add(GetDescription(Attribute.Lazer));
      // if ((attributeFlags & Attribute.Spear) != 0)
      //   res.Add(GetDescription(Attribute.Spear));
      // if ((attributeFlags & Attribute.Sword) != 0)
      //   res.Add(GetDescription(Attribute.Sword));

      foreach (var attribute in attributeFlags.GetFlags())
      {
        if (attribute == 0) continue;
        res.Add(GetDescription(attribute));
      }

      return res.ToArray();
    }
  }
}
