using System;
using TMPro;
using UnityEngine;

namespace Store
{
  public class Popup : MonoBehaviour
  {
    [NonSerialized]
    public RectTransform rectTransform;
    
    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    private void Awake()
    {
      rectTransform = GetComponent<RectTransform>();
      gameObject.SetActive(false);
    }

    public void ShowPopup(string name, string desc)
    {
      nameText.text = name;
      descriptionText.text = desc;
      gameObject.SetActive(true);
    }

    public void HidePopup()
    {
      gameObject.SetActive(false);
    }
  }
}
