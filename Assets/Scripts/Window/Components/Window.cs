using System;
using Manager;
using UnityEngine;
using UnityEngine.UI;
using Window.Contents;

namespace Window.Components
{
  public class Window : MonoBehaviour
  {
    [Header("Child Objects"), SerializeField]
    private WindowTitle titleObject;

    [SerializeField]
    private Button closeButton;

    [Header("Window"), SerializeField]
    private string m_title;

    public IWindowContent content { get; private set; }
      
    public string title
    {
      get => m_title;
      set
      {
        m_title = value;
        titleObject.text = value;
      }
    }

    public event Action onCloseButtonClick;
    public event Action onClose;
    
    private void OnValidate()
    {
      title = m_title;
    }

    private void Awake()
    {
      content = (IWindowContent) transform.GetComponentInChildren(typeof(IWindowContent));
      
      closeButton.onClick.AddListener(() =>
      {
        onCloseButtonClick?.Invoke();
        Close();
      });
    }

    public void Close()
    {
      onClose?.Invoke();
      GameManager.Window.Close(content.type, this);
    }
  }
}
