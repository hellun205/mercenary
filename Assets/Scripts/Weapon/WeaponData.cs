using System.Text;
using Item;
using Manager;
using UnityEngine;
using UnityEngine.Serialization;

namespace Weapon
{
  public abstract class WeaponData : ScriptableObject, IPossessible
  {
    [Header("Weapon Desc"), SerializeField]
    private string _name;

    public Attribute attribute;

    [SerializeField]
    private Sprite m_icon;

    [Multiline]
    public string descriptions;

    public WeaponStatus status;

    [SerializeField]
    public int m_price;
    
    [FormerlySerializedAs("needFlip")]
    [Header("Sprite Setting")]
    public bool needFlipY;

    public bool needFlipX;

    public bool rotate = true;

    public string itemName => $"[무기] {_name}";
    public string description => GetDescription();
    public Sprite icon => m_icon;
    public int price => m_price;
    
    public (string name, int tier) information
    {
      get
      {
        var split = name.Split('.');
        return (split[0], int.Parse(split[1]));
      }
    }
    
    private string GetDescription()
    {
      var sb = new StringBuilder();
      sb.Append(status.GetDescription());
      sb.Append($"{descriptions}");
      return sb.ToString();
    }
  }
}