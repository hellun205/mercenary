using UnityEngine;

namespace Store
{
  public interface IPurchasable
  {
    public int price { get; }
    public string name { get; }
    public string description { get; }
    public string addtive { get; }
    public Sprite icon { get; }
    public Color color { get; }
  }
}
