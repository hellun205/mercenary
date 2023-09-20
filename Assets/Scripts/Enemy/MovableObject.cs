using System;
using Manager;
using UnityEngine;
using Util;

namespace Enemy
{
  public class MovableObject : MonoBehaviour
  {
    public bool canMove { get; set; } = false;
    public Func<float> moveSpeed { get; set; } 
    
    private void Update()
    {
      if (!canMove) return;

      transform.rotation = transform.GetRotationOfLookAtObject(GameManager.Player.transform);
      transform.Translate(Vector3.right * (Time.deltaTime * moveSpeed.Invoke()));
    }

  }
}