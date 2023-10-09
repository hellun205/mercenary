using System;
using Manager;
using Sound;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.DragNDrop
{
  public abstract class UseDrop<T> : MonoBehaviour, IDropHandler where T : DragRequest
  {
    public event Action<T> onGetRequest;

    public void OnDrop(PointerEventData eventData)
    {
      if (!eventData.pointerDrag.TryGetComponent<UseDrag<T>>(out var pointerDrag) || !pointerDrag.lastCondition) return;

      onGetRequest?.Invoke(pointerDrag.data);
      pointerDrag.draggingObject.EndDrag();
      GameManager.Sound.Play(SoundType.SFX_UI, "sfx/ui/drop");
    }
  }
}