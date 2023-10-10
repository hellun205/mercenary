using Manager;
using UnityEngine;
using Util;

namespace Player.Partner
{
  public class PartnerAnimation : MonoBehaviour
  {
    [SerializeField]
    private Transform flipObject;

    private Animator anim;

    public SpriteRenderer sr;

    private void Awake()
    {
      anim = GetComponent<Animator>();
    }

    private void Update()
    {
      var flip = GameManager.Player.movement.flip.localScale.x.ToNormalize();
      var scale = flipObject.localScale;
      flipObject.localScale = scale.Setter(x: Mathf.Abs(scale.x) * flip);

      anim.SetBool("walking", GameManager.Player.movement.isWalking);
    }
  }
}