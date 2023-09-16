using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
  public class Slide : MonoBehaviour
  {
    private List<RectTransform> pages = new();

    public int currentPage;

    private RectTransform rectTransform;

    [SerializeField]
    private float width;

    [SerializeField]
    private RectTransform content;

    [SerializeField]
    private Timer animationTimer = new Timer();

    private float startX;
    private float endX;

    private void Awake()
    {
      rectTransform = GetComponent<RectTransform>();
      for (int i = 0; i < content.childCount; i++)
        pages.Add(content.GetChild(i).GetComponent<RectTransform>());

      animationTimer.onTick += OnTimerTick;
      SlidePageWithoutAnimation(currentPage);
    }

    private void OnTimerTick(Timer sender)
    {
      content.anchoredPosition = new Vector2(Mathf.Lerp(startX, endX, sender.value), 0f);
    }

    public void SlideNextPage()
    {
      SlidePage(currentPage + 1);
    }

    public void SlidePreviousPage()
    {
      SlidePage(currentPage - 1);
    }

    public void SlidePage(int index)
    {
      if (index >= pages.Count || index <= -1 || animationTimer.isPlaying) return;

      Set(index);
      animationTimer.Start();
    }

    public void SlidePageWithoutAnimation(int index)
    {
      Set(index);
      content.anchoredPosition = new Vector2(endX, 0f);
    }

    private void Set(int index)
    {
      currentPage = index;
      startX = content.anchoredPosition.x;
      endX = index * -width;
    }
  }
}
