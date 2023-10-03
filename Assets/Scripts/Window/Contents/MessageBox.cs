using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Window.Contents
{
  public enum MessageBoxResult
  {
    Cancel,
    Confirm,
  }
  
  public class MessageBox : WindowContent<MessageBoxResult>
  {
    public override WindowType type => WindowType.MessageBox;

    [Header("Child Objects"), SerializeField]
    private Button confirmButton;

    [SerializeField]
    private TextMeshProUGUI messageTextBox;

    [Header("Message Box"),SerializeField]
    private string m_message;

    [SerializeField]
    private string m_confirmText;

    public string message
    {
      get => m_message;
      set
      {
        m_message = value;
        messageTextBox.text = value;
      }
    }

    private void OnValidate()
    {
      message = m_message;
      confirmButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = m_confirmText;
    }

    protected override void Awake()
    {
      base.Awake();
      confirmButton.onClick.AddListener(() => Return(MessageBoxResult.Confirm));
      window.onCloseButtonClick += () => Return(MessageBoxResult.Cancel, false);
    }
  }
}
