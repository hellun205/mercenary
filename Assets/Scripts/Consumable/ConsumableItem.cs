using System;
using System.Text;
using Data;
using Item;
using Manager;
using Player;
using Store;
using UnityEngine;
using Util.Text;

namespace Consumable
{
  [CreateAssetMenu(fileName = "Consumable Item", menuName = "Consumable Item", order = 0)]
  public class ConsumableItem : ScriptableObject, IPossessible, IPurchasable
  {
    [SerializeField]
    private string _name;

    [SerializeField]
    private Sprite _icon;

    [SerializeField, Multiline]
    private string _descrption;

    public string specfiedName => name;
    public string itemName => _name;
    public int price => GetPrice();
    public string description => GetDescription();
    public string addtive => string.Empty;
    public Sprite icon => _icon;
    public Color color => GameManager.GetTierColor(0);

    public string displayName => _name;

    public IncreaseStatus GetStatus()
      => GameManager.Data.data.GetConsumableStatus(specfiedName);

    public float GetValueOfDetailStatus(ConsumableApplyStatus statusType)
      => GameManager.Data.data.GetConsumableStatus(specfiedName).GetValue(statusType.GetFieldName());

    public float GetDuration()
      => Convert.ToSingle(GameManager.Data.data.GetConsumableStatusData(specfiedName, ConsumableApplyStatus.Duration));

    public string GetDescription(int tier = 0)
    {
      var sb = new StringBuilder();
      var duration = GetDuration();
      var durText = duration > 0
        ? $"{duration.ToString().AddColor(Color.yellow)}초 동안"
        : "즉시".AddColor(Color.yellow);

      sb.Append(
        $"사용 시 {durText}".AddColor(GameManager.GetAttributeColor()));


      sb.Append("\n")
       .Append(GetStatus().GetDescription())
       .Append("\n")
       .Append(_descrption);

      return sb.ToString();
    }

    public int GetPrice(int tier = 0)
      => Convert.ToInt32(GameManager.Data.data.GetConsumableStatusData(specfiedName, ConsumableApplyStatus.Price));
  }
}
