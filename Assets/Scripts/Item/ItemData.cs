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

    public string GetDescription()
    {
      return "DESCRIPTION NOT IMPLEMENT!";
    }
  }
}
