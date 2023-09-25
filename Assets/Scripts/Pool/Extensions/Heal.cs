using TMPro;
using UnityEngine;

namespace Pool.Extensions
{
  public class Heal : MonoBehaviour, IUsePool
  {
    public PoolObject poolObject { get; set; }
    
    private Animator anim;

    [SerializeField]
    private TextMeshProUGUI text;

    private int _value;

    public int value
    {
      get => _value;
      set
      {
        _value = value;
        text.text = $"+{_value}";
      }
    }

    private void Awake()
    {
      anim = GetComponent<Animator>();
    }

    public void OnSummon()
    {
      anim.SetTrigger("lift");
    }
  }
}
