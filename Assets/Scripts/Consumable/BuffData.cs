using System;
using Player;
using UnityEngine;
using Util;

namespace Consumable
{
  public class BuffData
  {
    public string name { get; }
    public Sprite icon { get; }
    public IncreaseStatus status { get; }
    public float duration { get; }
    public float elapsedTime => timer.elapsedTime;
    public Timer timer { get; private set; }
    public Action<BuffData> onEnd { get; set; }
    public Action<BuffData> onTick { get; set; }

    public BuffData(string name, IncreaseStatus status, Sprite icon, float duration)
    {
      this.name = name;
      this.status = status;
      this.icon = icon;
      this.duration = duration;

      timer = new Timer
      {
        duration = duration
      };

      timer.onEnd += _ => onEnd?.Invoke(this);
    }

    public BuffData(ConsumableItem consumableItem)
      : this
      (
        consumableItem.itemName,
        consumableItem.GetStatus(),
        consumableItem.icon,
        consumableItem.GetDuration()
      )
    {
    }

    public void StartTimer() => timer.Start();
  }
}