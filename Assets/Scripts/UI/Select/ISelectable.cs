using UnityEngine;

namespace UI.Select
{
  public interface ISelectable
  {
    public Sprite icon { get;  }
    
    public string description { get; }
    
    public int index { get; set; }
  }

  public class SelectableItem : ISelectable
  {
    public Sprite icon { get; set; }
    public string description { get; set; }
    public int index { get; set; }
  }
}
