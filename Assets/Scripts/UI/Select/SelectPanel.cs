using System.Collections.Generic;
using System.Linq;
using Manager;
using UnityEngine;

namespace UI.Select
{
  public abstract class SelectPanel<T, TUi> : MonoBehaviour where T : ISelectable where TUi : SelectItem<T>
  {
    public List<TUi> items { get; private set; } = new();

    [SerializeField]
    protected string itemPrefabName;

    public SelectItem<T> selectedItem => items.SingleOrDefault(x => x.isSelected);

    public void AddItem(T item)
    {
      var obj = Instantiate(GameManager.Prefabs.Get<TUi>(itemPrefabName), transform);
      obj.SetValue(item, items.Count);
      obj.onSelected += OnSelected;
      items.Add(obj);
    }

    public void AddItems(params T[] item)
    {
      foreach (var selectable in item)
        AddItem(selectable);
    }

    public void Clear()
    {
      foreach (var selectItem in items)
        Destroy(selectItem.gameObject);
      items.Clear();
    }

    private void OnSelected(SelectItem<T> sender)
    {
      foreach (var item in items.Where(x => x != sender && x.isSelected))
        item.Deselect();
    }
  }

  public class SelectPanel : SelectPanel<SelectableItem, SelectItem<SelectableItem>>
  {
    
  }
}
