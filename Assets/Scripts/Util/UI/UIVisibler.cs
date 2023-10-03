using System;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Util.UI
{
  [RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
  public class UIVisibler : MonoBehaviour
  {
    [SerializeField]
    private CanvasGroup canvasGroup;

    private Timer timer = new Timer();
    private int multiple;

    private void Reset()
    {
      canvasGroup = GetComponent<CanvasGroup>();
      SetVisible(true);
    }

    private void Awake()
    {
      timer.isUnscaled = true;
      timer.onTick += TimerOnTick;
    }

    private void TimerOnTick(Timer sender)
    {
      canvasGroup.alpha = sender.value * multiple;
    }

    public void SetVisible(bool visible, float? duration = null, TimerType type = TimerType.Normal)
    {
      canvasGroup.interactable = visible;
      canvasGroup.blocksRaycasts = visible;
      
      if (duration.HasValue)
      {
        timer.duration = duration.Value;
        timer.type = type;
        multiple = visible ? 1 : -1;
        timer.Start();
      }
      else
        canvasGroup.alpha = visible ? 1f : 0f;
        
    }
  }

  public static class UIVisiblerUtility
  {
    public static void SetVisible
    (
      this Object obj,
      bool visible,
      float? duration = null,
      TimerType type = TimerType.Normal
    )
      => obj.GetComponent<UIVisibler>().SetVisible(visible, duration, type);
  }
}
