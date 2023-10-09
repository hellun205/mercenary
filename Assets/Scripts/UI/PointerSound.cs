using Sound;
using UnityEngine;
using UnityEngine.EventSystems;
using Util;

namespace UI
{
  [RequireComponent(typeof(UseSound))]
  public class PointerSound : MonoBehaviour, IPointerDownHandler
  {
    public void OnPointerDown(PointerEventData eventData)
    {
      this.PlaySound();
    }
  }
}
