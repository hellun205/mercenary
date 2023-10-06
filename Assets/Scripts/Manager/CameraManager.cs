using Cinemachine;
using UnityEngine;

namespace Manager
{
  public class CameraManager
  {
    public Camera camera { get; set; }

    public CinemachineVirtualCamera virtualCamera { get; set; }
    
    public CinemachineConfiner2D confiner { get; set; }

    public void SetCamera(PolygonCollider2D confinerCollider)
    {
      camera = Object.FindObjectOfType<Camera>();
      virtualCamera = Object.FindObjectOfType<CinemachineVirtualCamera>();
      confiner = virtualCamera.GetComponent<CinemachineConfiner2D>();

      virtualCamera.Follow = GameManager.Player.transform;
      virtualCamera.LookAt = GameManager.Player.transform;
      
      confiner.m_BoundingShape2D = confinerCollider;
    }
  }
}