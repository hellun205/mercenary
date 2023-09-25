using System;
using Manager;
using UnityEngine;

namespace Player
{
  public class PlayerMovement : MonoBehaviour
  {
    public Vector2 currentMoveAmount = Vector2.zero;

    public float baseMoveSpeed = 4;

    private Rigidbody2D rigid;

    private void Awake()
    {
      rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
      float v = 0f, h = 0f;

      GameManager.Key.KeyMap(KeyType.Press,
        (Keys.PlayerMovementUp, () => v = 1f),
        (Keys.PlayerMovementDown, () => v = -1f),
        (Keys.PlayerMovementLeft, () => h = -1f),
        (Keys.PlayerMovementRight, () => h = 1f));

      currentMoveAmount = new Vector2(h, v).normalized;
    }

    private void FixedUpdate()
    {
      // rigid.MovePosition(transform.position + (Vector3)currentMoveAmount *
      //   (baseMoveSpeed * (GameManager.Player.status.moveSpeed + 1) * Time.fixedDeltaTime));
      transform.Translate(currentMoveAmount * (baseMoveSpeed * (GameManager.Player.status.moveSpeed + 1) * Time.fixedDeltaTime));
    }
  }
}