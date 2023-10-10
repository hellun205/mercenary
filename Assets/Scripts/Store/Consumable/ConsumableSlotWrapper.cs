using System;
using UnityEngine;

namespace Store.Consumable
{
  public class ConsumableSlotWrapper : MonoBehaviour
  {
    public ConsumableSlot[] slots { get; private set; }
    
    private void Awake()
    {
      slots = GetComponentsInChildren<ConsumableSlot>();

      for (var i = 0; i < slots.Length; i++)
      {
        slots[i].index = i;
        slots[i].transform.SetAsLastSibling();
      }
    }
  }
}