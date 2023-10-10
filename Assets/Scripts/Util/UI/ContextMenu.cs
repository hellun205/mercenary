using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Util.UI
{
  public class ContextMenu : Selectable
  {
    private Button[] buttons;
    private string[] texts;

    [SerializeField]
    private UIVisibler uiVisibler;

    private Canvas canvas;

    private bool isClicked;

    public static event Action onOpened;

#if UNITY_EDITOR
    protected override void Reset()
    {
      base.Reset();
      uiVisibler = GetComponent<UIVisibler>();
    }
#endif
    

    protected override void Awake()
    {
      base.Awake();
      buttons = transform.GetChilds<Button>();
      canvas = GetComponentInParent<Canvas>();

      texts = buttons.Select(x => x.GetComponentInChildren<TextMeshProUGUI>().text).ToArray();
    }

    private void Update()
    {
      if (!uiVisibler.isVisible) return;

      if
      (
        buttons.All(x => x.gameObject != EventSystem.current.currentSelectedGameObject) &&
        !currentSelectionState.HasFlag(SelectionState.Selected)
      )
        Close();
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
          Close();
          onButtonClick.Invoke(buttons[i1].name);
        });
        var tmp = buttons[i].GetComponentInChildren<TextMeshProUGUI>();

        tmp.text = string.Format(texts[i], format);
      }

      onOpened?.Invoke();
      Select();
      uiVisibler.SetVisible(true);
    }

    public void Close()
    {
      uiVisibler.SetVisible(false);
    }
  }
}
