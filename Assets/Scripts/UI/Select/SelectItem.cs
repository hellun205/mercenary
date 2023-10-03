using System;
using UI.Popup;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Select
{
  [RequireComponent(typeof(Outline))]
  public abstract class SelectItem<T> : UsePopup<PopupPanel>, IPointerClickHandler where T : ISelectable
  {
    public override string popupName => m_popupName;
    
    [SerializeField]
    protected Outline outline;

    [SerializeField]
    protected Image icon;

    [SerializeField]
    protected Color defaultColor;
    
    [SerializeField]
    protected Color selectColor;

    [SerializeField]
    protected float defaultBold = 1;
    
    [SerializeField]
    protected float selectBold = 3;

    [SerializeField]
    protected string m_popupName;
    
    public bool isSelected { get; private set; }

    public T value { get; private set; }

    public event Action<SelectItem<T>> onSelected;
    public event Action<SelectItem<T>> onDeselected;
      
    private void Reset()
    {
      outline = GetComponent<Outline>();
    }

    protected override void Awake()
    {
      base.Awake();
      Deselect();
    }

    public void Select()
    {
      isSelected = true;
      outline.effectColor = selectColor;
      outline.effectDistance = new Vector2(selectBold, -selectBold);
      onSelected?.Invoke(this);
    }

    public void Deselect()
    {
      isSelected = false;
      outline.effectColor = defaultColor;
      outline.effectDistance = new Vector2(defaultBold, -defaultBold);
      onDeselected?.Invoke(this);
    }

    public void SetValue(T value, int index)
    {
      this.value = value;
      icon.sprite = value.icon;
      value.index = index;
    }

    public override void OnEntered()
    {
      base.OnEntered();
      popupPanel.ShowPopup(value.description);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      Select();
    }
  }

  public class SelectItem : SelectItem<SelectableItem>
  {
  }
}
