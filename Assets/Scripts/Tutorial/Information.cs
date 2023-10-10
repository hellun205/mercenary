using System;
using System.Collections;
using System.Text;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tutorial
{
  public class Information : MonoBehaviour
  {
    [SerializeField]
    private TextMeshProUGUI textTmp;

    private Animator anim;

    public const float speechTime = 0.04f;

    private StringBuilder sb = new StringBuilder();

    private Action fn;

    private bool skipable;

    public Highlight highlight;

    public bool isActive { get; private set; }

    [SerializeField]
    private Button nextButton;

    private void Awake()
    {
      anim = GetComponent<Animator>();
    }

    private void Start()
    {
      nextButton.onClick.AddListener(() =>
      {
        if (isActive)
        {
          if (!skipable) return;
          skipable = false;
          anim.SetBool("open", false);
          nextButton.gameObject.SetActive(false);
          highlight.StopHighlighting();
        }
      });

      nextButton.gameObject.SetActive(false);
    }

    public string text { get; private set; }

    public void StartWrite(string text, Action callback)
    {
      sb.Clear();
      textTmp.text = "";
      isActive = true;
      this.text = text;
      skipable = false;
      anim.SetBool("open", true);
      fn = callback;
      nextButton.gameObject.SetActive(true);
    }

    private void EndAnimation()
    {
      StartCoroutine(WriteRoutine());
    }

    private IEnumerator WriteRoutine()
    {
      var chars = text.ToCharArray();

      foreach (var c in chars)
      {
        sb.Append(c);
        textTmp.text = sb.ToString();
        yield return new WaitForSecondsRealtime(speechTime);
      }

      skipable = true;
      highlight.StartHighlighting();
    }

    private void EndCloseAnimation()
    {
      fn?.Invoke();
    }
  }
}