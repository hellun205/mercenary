using System;
using DG.Tweening;
using UnityEngine;
using Util;

namespace Pool.Extensions
{
  public class Particle : MonoBehaviour, IUsePool
  {
    public PoolObject poolObject { get; set; }

    private ParticleSystem particle;

    private void Awake()
    {
      particle = GetComponent<ParticleSystem>();
    }

    public void OnSummon()
    {
      // if (!particle.isPlaying)
      //   particle.Play();
      // CoroutineUtility.Wait(particle.main.duration, () => particle.Stop());
      CoroutineUtility.Wait(particle.main.duration + 0.2f, () => poolObject.Release());
    }
  }
}
