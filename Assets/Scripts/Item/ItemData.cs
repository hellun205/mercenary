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

    public IncreaseStatus status { get; private set; }

    public string specfiedName => name;
    public string itemName => $"[아이템] {m_itemName}";

    public Sprite icon => m_icon;

    public bool hasTier => true;
    public int tier { get; set; }

    public string GetDescription(int tier = 0)
    {
      var sb = new StringBuilder();
      sb.Append(status.GetDescription());
      sb.Append($"{m_description}");
      return sb.ToString();
    }

    public int GetPrice(int tier = 0) => GameManager.Data.data.GetItemPrice(specfiedName);

    public void Refresh()
    {
      status = GameManager.Data.data.GetItemStatus(specfiedName);
      tier = GameManager.Data.data.GetItemTier(specfiedName);
    }
  }
}
