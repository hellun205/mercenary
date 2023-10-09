using System;
using Manager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Util.UI
{
  public interface IUseContextMenu : IPointerClickHandler
  {
    public string contextMenuName { get; }

    public object[] contextMenuFormats { get; }
    public Action<string> contextMenuFunction { get; }

    public bool contextMenuCondition => true;

    public Vector2 contextMenuPosition => Input.mousePosition;

    public PointerEventData.InputButton button => PointerEventData.InputButton.Right;

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
      if (!contextMenuCondition || eventData.button != button) return;

      GameManager.UI.Find<ContextMenu>(contextMenuName).Open
      (
        contextMenuPosition,
        (object[])contextMenuFormats.Clone(),
        contextMenuFunction
      );
    }
  }
}
