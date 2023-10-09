using System;
using Manager;
using UnityEngine;
using Util;

namespace Sound
{
  public class UseSound : MonoBehaviour
  {
    public SoundType type;
    public string[] sounds;

    [SerializeField]
    protected bool playOnStart;
    
    public void PlaySound()
    {
      GameManager.Sound.Play(type, sounds.GetRandom());
    }

    private void Start()
    {
      if (playOnStart) PlaySound();
    }
  }
}
