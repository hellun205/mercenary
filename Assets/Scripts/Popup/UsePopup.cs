using Manager;
using UnityEngine;
using UnityEngine.EventSystems;
using Util;

namespace Popup
{
  public abstract class UsePopup<T> : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler where T : PopupPanel
  {
    public bool isPopup { get; private set; }
    public T popupPanel { get; private set; }
    public abstract string popupName { get; }

    public Canvas canvas { get; private set; }

    protected virtual void Awake()
    {
      popupPanel = (T)GameManager.UI.Find<PopupPanel>(popupName);
      canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      popupPanel.rectTransform.anchoredPosition = canvas.ScreenToCanvasPosition(eventData.position);
      isPopup = true;
      OnEntered();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      isPopup = false;
      OnExited();
    }

    protected virtual void Update()
    {
      if (!isPopup) return;

      var start = popupPanel.rectTransform.anchoredPosition;
      var end = canvas.ScreenToCanvasPosition(Input.mousePosition);

      popupPanel.rectTransform.anchoredPosition = Vector2.Lerp(start, end, Time.unscaledDeltaTime * 10f);
    }

    public virtual void OnEntered() =>
      popupPanel.ShowPopup("", "");

    public virtual void OnExited() => 
      popupPanel.HidePopup();
  }
}
