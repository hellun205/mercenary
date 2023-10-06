using System;
using System.Text;
using Data;
using Manager;
using Player;
using UnityEngine;
using Util.Text;

namespace Item
{
  [CreateAssetMenu(fileName = "Item Data", menuName = "Item Data", order = 0)]
  public class ItemData : ScriptableObject, IPossessible
  {
    [SerializeField]
    private string m_itemName;

    [SerializeField]
    private string m_description;

    [SerializeField]
    private Sprite m_icon;

    public IncreaseStatus status => GameManager.Data.data.GetItemStatus(specfiedName);

    public string specfiedName => name;
    public string itemName => $"[아이템] {m_itemName}";

    public Sprite icon => m_icon;

    public bool hasTier => true;
    public int tier => GameManager.Data.data.GetItemTier(specfiedName);

    public string GetDescription(int tier = 0)
    {
      var sb = new StringBuilder();
      sb.Append("보유 시 다음 능력치 증가\n".AddColor(GameManager.GetAttributeColor()))
       .Append(status.GetDescription(false))
       .Append($"{m_description}");
      return sb.ToString();
    }

    public int GetPrice(int tier = 0) => GameManager.Data.data.GetItemPrice(specfiedName);
  }
}
