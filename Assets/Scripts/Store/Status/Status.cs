using System;
using System.Collections.Generic;
using System.Linq;
using Manager;
using UnityEngine;

namespace Store.Status
{
  public class Status : MonoBehaviour
  {
    [SerializeField]
    private Transform content;

    public Dictionary<string, StatusItem> items = new ();
    
    private void Awake()
    {
      items = content.GetComponentsInChildren<StatusItem>().ToDictionary(x => x.name, x => x);
    }

    private void Start()
    {
      Refresh();
    }

    public void SetValue(string key, float value)
    {
      var item = items[key];

      item.statValue = value;
      item.Refresh();
    }

    public void Refresh()
    {
      var status = GameManager.Player.GetStatus();
      
      SetValue("max_hp", status.maxHp);
      SetValue("hp_regeneration", status.regeneration);
      SetValue("hp_drain", status.drainHp);
      SetValue("melee_damage", status.meleeDamage);
      SetValue("ranged_damage", status.rangedDamage);
      SetValue("critical_percentage", status.criticalPercent);
      SetValue("bleeding_damage", status.bleedingDamage);
      SetValue("attack_speed", status.attackSpeed);
      SetValue("weapon_range", status.range);
      SetValue("armor", status.armor);
      SetValue("knockback", status.knockback);
      SetValue("move_speed", status.moveSpeed);
      SetValue("luck", status.luck);
    }
  }
}
