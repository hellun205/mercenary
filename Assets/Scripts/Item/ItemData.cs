using System;
using System.Text;
using Data;
using Manager;
using Player;
using UnityEngine;

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

    [NonSerialized]
    public IncreaseStatus[] status;

    public string specfiedName => name;
    public string itemName => $"[아이템] {m_itemName}";

    public Sprite icon => m_icon;

    public int price { get; private set; }

    public string GetDescription(int tier)
    {
      var sb = new StringBuilder();
      sb.Append(status[tier].GetDescription());
      sb.Append($"{m_description}");
      return sb.ToString();
    }

    public void Refresh()
    {
      price = GameManager.Data.data.GetItemPrice(specfiedName);
      status = GameManager.Data.data.GetItemStatus(specfiedName);
    }
  }
}
