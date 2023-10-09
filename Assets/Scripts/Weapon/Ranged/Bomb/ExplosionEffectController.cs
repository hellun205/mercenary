using System;
using Manager;
using Pool;
using Sound;
using UnityEngine;

namespace Weapon.Ranged.Bomb
{
  public class ExplosionEffectController : MonoBehaviour, IUsePool
  {
    public PoolObject poolObject { get; set; }

    private Animator anim;

    [SerializeField]
    private string animName;

    private void Awake()
    {
      anim = GetComponent<Animator>();
    }

    public void OnSummon()
    {
      anim.Play(animName);
      GameManager.Sound.Play(SoundType.SFX_Weapon, "sfx/weapon/explosion");
    }

    public void SetRange(float range)
    {
      transform.localScale = new Vector3(range, range);
    }
  }
}
