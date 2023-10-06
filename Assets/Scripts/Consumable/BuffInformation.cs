using Manager;
using UI;
using UnityEngine;

namespace Consumable
{
  public class BuffInformation : PoolableWrapper<BuffInformationItem>
  {
    protected override void Awake()
    {
      base.Awake();
      instantiateFunc = () => GameManager.Prefabs.Get<BuffInformationItem>("buff_item");
    }

    public BuffInformationItem Add(Sprite icon, string description)
    {
      return Get(item =>
      {
        item.iconImage.sprite = icon;
        item.description = description;
      });
    }

    public void Remove(BuffInformationItem item)
      => Release(item);
  }
}