using UnityEngine;
using UnityEngine.UI;
using Util.UI;

namespace UI.DragNDrop
{
  public class DraggingObject : MonoBehaviour
  {
    [SerializeField]
    private Image targetImage;

    public bool isDragging { get; private set; }

    public RectTransform rectTransform { get; private set; }

    public void StartDrag(Sprite icon)
    {
      targetImage.sprite = icon;
      isDragging = true;
      gameObject.SetVisible(true);
    }

    public void EndDrag()
    {
      isDragging = false;
      gameObject.SetVisible(false);
    }

    private void Awake()
    {
      rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
      gameObject.SetVisible(false);
    }
  }
}
