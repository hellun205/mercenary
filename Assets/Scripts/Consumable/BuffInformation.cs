using Manager;
using UI;
using UnityEngine;

namespace Consumable
{
  public class BuffInformation : PoolableWrapper<BuffInformationItem, BuffInformationItem>
  {
    protected override void Awake()
    {
      base.Awake();
      instantiateFunc = () => GameManager.Prefabs.Get<BuffInformationItem>("buff_item");
    }

    public BuffInformationItem Add(string title, Sprite icon, string description)
    {
      var obj = Get();
      obj.component.title = title;
      obj.component.icon = icon;
      obj.component.description = description;
      
      return obj.component;
    }

    public void Remove(BuffInformationItem item)
      => Release(item);
  }
}