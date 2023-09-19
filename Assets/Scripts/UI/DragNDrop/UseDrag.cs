using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Util;

namespace UI.DragNDrop
{
  public abstract class UseDrag<T> : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    where T : DragRequest
  {
    public event Action<T> onStartRequest;

    public T data { get; private set; }

    public Func<T> getter { get; set; }

    public Func<bool> condition = () => true;

    public bool lastCondition { get; private set; }
    
    public DraggingObject draggingObject { get; set; }
    
    private Canvas canvas;

    protected virtual void Awake()
    {
      canvas = transform.GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
      lastCondition = condition.Invoke();
      if (!lastCondition) return;
      
      data = getter.Invoke();
      draggingObject.StartDrag(data.draggingImage);
      onStartRequest?.Invoke(data);
    }

    public void OnDrag(PointerEventData eventData)
    {
      if (!draggingObject.isDragging) return;
      draggingObject.rectTransform.anchoredPosition = canvas.ScreenToCanvasPosition(Input.mousePosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
      draggingObject.EndDrag();
    }
  }
}