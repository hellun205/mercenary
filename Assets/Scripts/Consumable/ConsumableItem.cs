using System;
using Data;
using Item;
using Manager;
using Player;
using UnityEngine;
using Util.Text;

namespace Consumable
{
  [CreateAssetMenu(fileName = "Consumable Item", menuName = "Consumable Item", order = 0)]
  public class ConsumableItem : ScriptableObject, IPossessible
  {
    [SerializeField]
    private string _name;

    [SerializeField]
    private Sprite _icon;

    public string specfiedName => name;
    public string itemName => _name;
    public Sprite icon => _icon;
    public bool hasTier => false;
    public int tier => 0;

    public IncreaseStatus GetStatus()
      => GameManager.Data.data.GetConsumableStatus(specfiedName);

    public float GetDuration()
      => Convert.ToSingle(GameManager.Data.data.GetConsumableStatusData(specfiedName, ConsumableApplyStatus.Duration));

    public string GetDescription(int tier = 0)
      => $"{"사용 시".AddColor(GameManager.GetAttributeColor())}\n{GetStatus().GetDescription()}";

    public int GetPrice(int tier = 0)
      => Convert.ToInt32(GameManager.Data.data.GetConsumableStatusData(specfiedName, ConsumableApplyStatus.Price));
  }
}