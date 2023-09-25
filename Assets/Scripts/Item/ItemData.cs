﻿using System.Text;
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

    [SerializeField]
    private int m_price;

    public IncreaseStatus increaseStatus;

    public string itemName => $"[아이템] {m_itemName}";
    public string description => GetDescription();

    public Sprite icon => m_icon;

    public int price => m_price;

    private string GetDescription()
    {
      var sb = new StringBuilder();
      sb.Append(increaseStatus.GetDescription());
      sb.Append($"{m_description}");
      return sb.ToString();
    }
  }
}
