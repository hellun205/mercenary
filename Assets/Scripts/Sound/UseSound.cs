using System;
using Manager;
using UnityEngine;

namespace Sound
{
  public class UseSound : MonoBehaviour
  {
    public SoundType type;
    public string soundName;

    [SerializeField]
    private bool playOnStart;
    
    public void PlaySound()
    {
      GameManager.Sound.Play(type, soundName);
    }

    private void Start()
    {
      if (playOnStart) PlaySound();
    }
  }
}
