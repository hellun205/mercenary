using System;
using System.Text;
using Data;
using Item;
using Manager;
using UnityEngine;
using UnityEngine.Serialization;

namespace Weapon
{
  [CreateAssetMenu(menuName = "Weapon Data")]
  public class WeaponData : ScriptableObject, IPossessible
  {
    [Header("Weapon Desc"), SerializeField]
    private string _name;

    [NonSerialized]
    public Attribute attribute;

    [SerializeField]
    private Sprite m_icon;

    [Multiline]
    public string descriptions;

    [NonSerialized]
    public WeaponStatus[] status;

    [Header("Sprite Setting")]
    public bool needFlipY;

    public bool needFlipX;

    public bool rotate = true;

    [Header("Ranged")]
    public string bullet = "bullet";

    [Header("Explosion")]
    public bool fireOnContact;
    public bool isFixedTargetPosition;
    public string effectObj = "effect_explosion";
    public float explosionDelay;

    public string specfiedName => name;
    public string itemName => $"[무기] {_name}";
    public string description => GetDescription(0);
    public Sprite icon => m_icon;
    public int price => status[0].price;
    public bool hasTier => false;
    
    public int tier { get; set; }

    public string GetDescription(int tier)
    {
      var sb = new StringBuilder();
      sb.Append(status[tier].GetDescription());
      sb.Append($"{descriptions}");
      return sb.ToString();
    }

    public void Refresh()
    {
      attribute = GameManager.Data.data.GetWeaponAttribute(specfiedName);
      status = GameManager.Data.data.GetWeaponStatus(specfiedName);
    }
  }
}
