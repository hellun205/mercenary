using System;
using UnityEngine;

namespace UI.Popup
{
  public class IgnorePopup : MonoBehaviour
  {
    private void Awake()
    {
      GetComponent<UsePopup<PopupPanel>>().ignore = true;
    }
  }
}