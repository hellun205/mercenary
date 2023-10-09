using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using Util;

namespace Manager
{
  public class Broadcast
  {
    public TextMeshProUGUI broadcastText => GameManager.UI.Find<TextMeshProUGUI>("$broadcast");

    private TweenerCore<Color, Color, ColorOptions> tweenerCore;
    private Coroutine wait;

    public void Say(string message)
    {
      tweenerCore.Kill();
      if (wait != null)
        GameManager.CoroutineObject.StopCoroutine(wait);
      broadcastText.text = message;
      broadcastText.color = broadcastText.color.Setter(a: 1f);
      wait = 0.5f.WaitUnscaled(() => tweenerCore = broadcastText.DOFade(0f, 1f).SetUpdate(true));
    }

    public void Say(string format, params object[] formats)
    {
      try
      {
        Say(message: string.Format(format, formats));
      }
      catch
      {
        Say(message: format);
      }
    }
  }
}
