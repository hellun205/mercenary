using System.Text;
using Player;
using UnityEngine;

namespace Item
{
  [CreateAssetMenu(fileName = "Item Data", menuName = "Item Data", order = 0)]
  public class ItemData : ScriptableObject
  {
    public string itemName;

    public Sprite icon;

    public int price;

    public PlayerStatus increaseStatus;

    public string description;

    public string GetDescription()
    {
      var sb = new StringBuilder();
      sb.Append(increaseStatus.GetDescription());
      sb.Append($"{description}");
      return sb.ToString();
    }
  }
}
