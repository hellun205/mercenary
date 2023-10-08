using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Button = UnityEngine.UI.Button;

namespace Util
{
  public class InputKeyButton : Button
  {
    private TextMeshProUGUI text;

    private KeyCode? _key;

    public KeyCode? key
    {
      get => _key;
      set
      {
        _key = value;
        text.text = _key?.GetKeyString() ?? "";
        onKeyChanged?.Invoke(_key);
      }
    }

    public event Action<KeyCode?> onKeyChanged; 

    private KeyCode[] keyCodes;

    protected override void Awake()
    {
      base.Awake();
      text = GetComponentInChildren<TextMeshProUGUI>();
      keyCodes = Enum.GetValues(typeof(KeyCode)).OfType<KeyCode>().ToArray();
    }

    private void Update()
    {
      if (currentSelectionState != SelectionState.Selected) return;

      if (Input.anyKeyDown)
      {
        key = keyCodes.First(Input.GetKeyDown);
      }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
      base.OnPointerClick(eventData);
      if (eventData.button == PointerEventData.InputButton.Right)
        key = null;
    }
  }
}
