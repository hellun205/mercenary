using System;
using UnityEngine;

namespace Tutorial
{
  public class Highlight : MonoBehaviour
  {
    private Animator anim;

    private void Awake()
    {
      anim = GetComponent<Animator>();
    }

    public void StartHighlighting()
    {
      anim.SetBool("highlight", true);
    }

    public void StopHighlighting()
    {
      anim.SetBool("highlight", false);
    }
  }
}
