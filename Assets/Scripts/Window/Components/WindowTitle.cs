using System;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Util;

namespace Window.Components
{
  public class WindowTitle : MonoBehaviour, IBeginDragHandler, IDragHandler
  {
    [SerializeField]
    private TextMeshProUGUI titleText;

    private Window window;
    private Vector2 startPos;
    private Vector2 downPos;
    private Canvas canvas;

    public string text
    {
      get => m_text;
      set
      {
        m_text = value;
        titleText.text = value;
      }
    }

    [SerializeField]
    private string m_text;

    private void Awake()
    {
      window = transform.GetComponentInParent<Window>();
      canvas = transform.GetComponentInParent<Canvas>();
    }

    private void OnValidate()
    {
      text = m_text;
    }

    public void OnDrag(PointerEventData eventData)
    {
      var rectTransform = ((RectTransform) window.transform);

      Vector2 cur = canvas.ScreenToCanvasPosition(eventData.position);
      Vector2 down = canvas.ScreenToCanvasPosition(downPos);
      rectTransform.anchoredPosition = startPos + cur - down;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
      startPos = (((RectTransform) window.transform).anchoredPosition);
      downPos = eventData.position;
    }
  }
}
