using System.Text;
using Item;
using Manager;
using UnityEngine;
using UnityEngine.Serialization;

namespace Weapon
{
  public abstract class Weapon : ScriptableObject, IPossessible
  {
    [Header("Weapon Desc"), SerializeField]
    private string _name;
    
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
    
    private string GetDescription()
    {
      var sb = new StringBuilder();
      sb.Append(status.GetDescription());
      sb.Append($"{descriptions}");
      return sb.ToString();
    }
  }
}