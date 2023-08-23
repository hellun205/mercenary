using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;

namespace Manager
{
  public class UIManager : MonoBehaviourSingleTon<UIManager>
  {
    public List<TextMeshProUGUI> items;

    protected override void Awake()
    {
      base.Awake();
      items = FindObjectsOfType<TextMeshProUGUI>().Where(tmp => tmp.name.Contains('$')).ToList();
    }

    public TextMeshProUGUI Find(string name, Action<TextMeshProUGUI> setter = null)
    {
      var obj = items.Find(tmp => tmp.name == name);

      if (obj is null)
        throw new Exception($"invalid text: \"{name}\"");
      
      setter?.Invoke(obj);
      return obj;
    }
  }
}