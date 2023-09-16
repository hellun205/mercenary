using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;

namespace UI
{
  public class Slide : MonoBehaviour
  {
    private List<RectTransform> pages;

    public int currentPage;

    private RectTransform rectTransform;

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
      pages = content.GetComponentsInChildren<RectTransform>().ToList();
      width = rectTransform.sizeDelta.x;
      animationTimer.onTick += OnTimerTick;
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
      if (index == pages.Count - 1 || index == -1 || animationTimer.isPlaying) return;

      currentPage = index;
      startX = content.anchoredPosition.x;
      endX = index * -width;
      animationTimer.Start();
    }
  }
}
