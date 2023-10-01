using System;
using Pool;
using UnityEngine;

namespace Weapon.Ranged.Bomb
{
  [RequireComponent(typeof(PoolObject))]
  public class ExplosionEffectController : MonoBehaviour
  {
    [NonSerialized]
    public PoolObject po;
    
    private Animator anim;

    [SerializeField]
    private string animName;

    private void Awake()
    {
      po = GetComponent<PoolObject>();
      anim = GetComponent<Animator>();

      po.onGet += PoolOnGet;
      po.onReleased += PoolOnRelease;
    }

    private void PoolOnRelease()
    {
    }

    private void PoolOnGet()
    {
      anim.Play(animName);
    }

    public void SetRange(float range)
    {
      transform.localScale = new Vector3(range, range);
    }
  }
}