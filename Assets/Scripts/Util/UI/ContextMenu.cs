using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Util.UI
{
  public class ContextMenu : Selectable
  {
    private Button[] buttons;

    [SerializeField]
    private UIVisibler uiVisibler;

    private Canvas canvas;

    private bool isClicked;

    protected override void Reset()
    {
      base.Reset();
      uiVisibler = GetComponent<UIVisibler>();
    }

    protected override void Awake()
    {
      base.Awake();
      buttons = transform.GetChilds<Button>();
      canvas = GetComponentInParent<Canvas>();
    }

    private void Update()
    {
      if (!uiVisibler.isVisible) return;
      if (currentSelectionState != SelectionState.Selected)
        uiVisibler.SetVisible(false);
      else
        uiVisibler.SetVisible(true);
    }

    public void Open(Vector2 position, object[] format, Action<string> onButtonClick)
    {
      ((RectTransform) transform).anchoredPosition = canvas.ScreenToCanvasPosition(position);
      isClicked = false;
      for (var i = 0; i < buttons.Length; i++)
      {
        buttons[i].onClick.RemoveAllListeners();
        var i1 = i;
        buttons[i].onClick.AddListener(() =>
        {
          if (isClicked) return;
          isClicked = true;
          onButtonClick.Invoke(buttons[i1].name);
        });
        var tmp = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = string.Format(tmp.text, format);
      }

      Select();
    }
  }
}
