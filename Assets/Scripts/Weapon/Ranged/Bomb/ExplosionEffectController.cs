using System;
using Pool;
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
    }

    public void SetRange(float range)
    {
      transform.localScale = new Vector3(range, range);
    }
  }
}
