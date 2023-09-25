using UnityEngine;

namespace Item
{
  public interface IPossessible
  {
    public string itemName { get; }
    public string description { get; }

    public Sprite icon { get; }
    
    public int price { get; }
  }
}
