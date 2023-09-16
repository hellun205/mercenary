using System;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Util;

namespace Store.Status
{
  public class StatusItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
  {
    [Header("Bind")]
    [SerializeField]
    private Image statIconImage;

    [SerializeField]
    private TextMeshProUGUI statNameText;

    [SerializeField]
    private TextMeshProUGUI statValueText;

    [Header("Value")]
    public Sprite statIcon;

    public string statName;
    public float statValue;

    [Multiline]
    public string description;

    private static Popup popup;
    private Canvas canvas;

    private void Awake()
    {
      popup ??= GameManager.UI.Find<Popup>("$popup_stat");
      canvas = GetComponentInParent<Canvas>();
    }

    private void OnValidate()
    {
      Refresh();
    }

    public void Refresh()
    {
      statIconImage.sprite = statIcon;
      statNameText.text = statName;
      statValueText.text = statValue.GetViaValue();
    }


    public void OnPointerExit(PointerEventData eventData)
    {
      popup.HidePopup();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
      Move(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      Move(eventData);
      popup.ShowPopup(statName, description);
    }

    public void Move(PointerEventData eventData)
    {
      popup.rectTransform.anchoredPosition = canvas.ScreenToCanvasPosition(eventData.position);
    }
  }
}
