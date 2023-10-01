using UnityEngine;

namespace Item
{
  public interface IPossessible
  {
    public string specfiedName { get; }
    public string itemName { get; }

    public Sprite icon { get; }
    
    public int price { get; }

    public string GetDescription(int tier);
  }
}
