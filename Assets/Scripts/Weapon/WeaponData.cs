using System;
using System.Text;
using Data;
using Item;
using Manager;
using Player;
using UnityEngine;
using UnityEngine.Serialization;
using Util.Text;

namespace Weapon
{
  [CreateAssetMenu(menuName = "Weapon Data")]
  public class WeaponData : ScriptableObject, IPossessible
  {
    [Header("Weapon Desc"), SerializeField]
    private string _name;

    public Attribute attribute => GameManager.Data.data.GetWeaponAttribute(specfiedName);

    [SerializeField]
    private Sprite m_icon;

    [Multiline]
    public string descriptions;

    public WeaponStatus[] status => GameManager.Data.data.GetWeaponStatus(specfiedName);

    public IncreaseStatus[] increaseStatus => GameManager.Data.data.GetWeaponIncreaseStatus(specfiedName);

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
    public Sprite icon => m_icon;

    public string GetDescription(int tier)
    {
      var sb = new StringBuilder();
      sb.Append("무기 능력치\n".AddColor(GameManager.GetAttributeColor()))
       .Append(status[tier].GetDescription())
       .Append($"{string.Format(descriptions, status[tier].bulletCount)}");
      return sb.ToString();
    }

    public string GetDescriptionWithAdditionalStatus(int tier, IncreaseStatus additional)
    {
      var sb = new StringBuilder();
      sb.Append(status[tier].GetDescription(additional));
      sb.Append($"{descriptions}");
      return sb.ToString();
    }

    public int GetPrice(int tier)
      => Convert.ToInt32(GameManager.Data.data.GetWeaponStatusData(specfiedName, tier, WeaponStatusItem.Price));
  }
}
