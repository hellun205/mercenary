using System;
using UnityEngine;
using UnityEngine.UI;

namespace Util.UI
{
  public class RadioButtonList : MonoBehaviour
  {
    public Color enableColor;
    public Color disableColor;

    private Button[] buttons;

    [SerializeField]
    private string defaultEnable;

    public string current { get; private set; }

    public event Action<string> onChanged;
    
    private void Awake()
    {
      buttons = GetComponentsInChildren<Button>();

      foreach (var button in buttons)
        button.onClick.AddListener(() => SetEnable(button.name));
    }

    private void Start()
    {
      SetEnable(defaultEnable);
    }

    public void SetEnable(string buttonName)
    {
      current = null;
      foreach (var button in buttons)
      {
        var logical = button.name == buttonName;
        button.GetComponent<Image>().color = logical ? enableColor : disableColor;
        
        if (logical)
        {
          current = button.name;
          
        }
      }
      this.RebuildLayout(10);
      
      onChanged?.Invoke(current);  
    }
  }
}
