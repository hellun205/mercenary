using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Window.Contents
{
  public enum AskBoxResult
  {
    Cancel, Yes, No
  }

  public class AskBox : WindowContent<AskBoxResult>
  {
    public override WindowType type => WindowType.AskBox;

    [Header("Child Objects"), SerializeField]
    private Button yesButton;
    [SerializeField]
    private Button noButton;

    [SerializeField]
    private TextMeshProUGUI messageTextBox;

    [Header("Ask Box"), SerializeField]
    private string m_message;

    [SerializeField]
    private string m_yesText;
    
    [SerializeField]
    private string m_noText;

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
      yesButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = m_yesText;
      noButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = m_noText;
    }

    protected override void Awake()
    {
      base.Awake();
      yesButton.onClick.AddListener(() => Return(AskBoxResult.Yes));
      noButton.onClick.AddListener(() => Return(AskBoxResult.No));
      window.onCloseButtonClick += () => Return(AskBoxResult.Cancel, false);
    }
  }
}
