using Store;
using UnityEngine;

namespace Item
{
  public interface IPossessible
  {
    public string specfiedName { get; }
    public string itemName { get; }

    public Sprite icon { get; }
    
    public bool hasTier { get; }
    
    public int tier { get; }

    public string GetDescription(int tier = 0);

    public int GetPrice(int tier = 0);
  }
}
