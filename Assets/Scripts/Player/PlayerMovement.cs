using System;
using Manager;
using UnityEngine;
using Util;

namespace Player
{
  public class PlayerMovement : MonoBehaviour
  {
    public Vector2 currentMoveAmount = Vector2.zero;

    private Animator anim;
    
    public Transform flip;

    public bool isWalking => currentMoveAmount != Vector2.zero;

    private void Awake()
    {
      anim = GetComponent<Animator>();
    }

    private void Update()
    {
      float v = 0f, h = 0f;

      GameManager.Key.KeyMap(GetKeyType.Press,
        (Keys.PlayerMovementUp, () => v = 1f),
        (Keys.PlayerMovementDown, () => v = -1f),
        (Keys.PlayerMovementLeft, () => h = -1f),
        (Keys.PlayerMovementRight, () => h = 1f));

      currentMoveAmount = new Vector2(h, v).normalized;

      var ls = flip.localScale;
      if (h > 0)
        flip.localScale = ls.Setter(x: Mathf.Abs(ls.x));
      else if (h < 0)
        flip.localScale = ls.Setter(x: -Mathf.Abs(ls.x));

      anim.SetBool("walking", isWalking);
    }

    private void FixedUpdate()
    {
      // rigid.MovePosition(transform.position + (Vector3)currentMoveAmount *
      //   (baseMoveSpeed * (GameManager.Player.status.moveSpeed + 1) * Time.fixedDeltaTime));

      transform.Translate(currentMoveAmount * (GameManager.Player.currentStatus.moveSpeed * Time.fixedDeltaTime));
    }
  }
}
