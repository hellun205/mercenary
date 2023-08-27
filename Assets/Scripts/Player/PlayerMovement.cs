using Manager;
using UnityEngine;

namespace Player
{
  public class PlayerMovement : MonoBehaviour
  {
    public Vector2 currentMoveAmount = Vector2.zero;

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
      transform.Translate(currentMoveAmount * (GameManager.Player.status.moveSpeed * Time.fixedDeltaTime));
    }
  }
}