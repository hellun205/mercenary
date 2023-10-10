using System;
using DG.Tweening;
using UI.Popup;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
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
    protected Image panel;
    
    [SerializeField]
    protected Image icon;

    [SerializeField]
    protected Color defaultColor;
    [SerializeField]
    protected Color defaultOutlineColor;
    
    [SerializeField]
    protected Color selectColor;
    
    [SerializeField]
    protected Color selectOutlineColor;

    [SerializeField]
    protected float defaultOutlineBold = 1;
    
    [SerializeField]
    protected float selectOutlineBold = 3;

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
      outline.DOColor(selectOutlineColor, 0.2f);
      panel.DOColor(selectColor, 0.2f);
      outline.effectDistance = new Vector2(selectOutlineBold, -selectOutlineBold);
      onSelected?.Invoke(this);
    }

    public void Deselect()
    {
      isSelected = false;
      outline.DOColor(defaultOutlineColor, 0.2f);
      panel.DOColor(defaultColor, 0.2f);
      outline.effectDistance = new Vector2(defaultOutlineBold, -defaultOutlineBold);
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
