using System;
using TMPro;
using UnityEngine;

namespace Pool.Extensions
{
  public class Damage : UsePool
  {
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
        text.text = $"{_value}";
      }
    }  
    
    protected override void Awake()
    {
      anim = GetComponent<Animator>();
      base.Awake();
    }

    protected override void OnSummon()
    {
      anim.SetTrigger("lift");
    }
  }
}
