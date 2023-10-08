using UnityEngine;

namespace Util.UI
{
  public class TabPage : MonoBehaviour
  {
    private UIVisibler[] pages;  

    [SerializeField]
    private string defaultEnable;

    public string current { get; private set; }
    
    private void Awake()
    {
      pages = transform.GetChilds<UIVisibler>();
    }

    private void Start()
    {
      SetEnable(defaultEnable);
    }

    public void SetEnable(string pageName)
    {
      current = null;
      foreach (var page in pages)
      {
        var logical = page.name == pageName;
        page.SetVisible(logical);
        
        if (logical)
        {
          current = page.name;
        }
      }
      this.RebuildLayout();
    }
  }
}
