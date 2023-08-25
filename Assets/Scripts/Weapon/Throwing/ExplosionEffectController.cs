using Pool;
using UnityEngine;

namespace Weapon.Throwing
{
  [RequireComponent(typeof(PoolObject))]
  public class ExplosionEffectController : MonoBehaviour
  {
    private PoolObject po;
    private Animator anim;

    [SerializeField]
    private string animName;
    [SerializeField]
    private string noneName = "None";

    private void Awake()
    {
      po = GetComponent<PoolObject>();
      anim = GetComponent<Animator>();

      po.onGet += PoolOnGet;
      po.onReleased += PoolOnRelease;
    }

    private void PoolOnRelease()
    {
      // anim.Play(noneName);
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