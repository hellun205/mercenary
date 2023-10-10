using Cinemachine;
using UnityEngine;

namespace Manager
{
  public class CameraManager
  {
    public Camera camera { get; private set; }

    public CinemachineVirtualCamera virtualCamera { get; private set; }
    
    public CinemachineConfiner2D confiner { get; private set; }
    public CinemachineFollowZoom followZoom  { get; private set; }
    
    public float defaultZoom { get; private set; }

    public void SetCamera(PolygonCollider2D confinerCollider)
    {
      camera = Object.FindObjectOfType<Camera>();
      virtualCamera = Object.FindObjectOfType<CinemachineVirtualCamera>();
      confiner = virtualCamera.GetComponent<CinemachineConfiner2D>();
      followZoom = virtualCamera.GetComponent<CinemachineFollowZoom>();
      defaultZoom = followZoom.m_Width;
      
      SetTarget(GameManager.Player.transform);
      
      confiner.m_BoundingShape2D = confinerCollider;
    }

    public void SetTarget(Transform target)
    {
      virtualCamera.Follow = target;
      virtualCamera.LookAt = target;
    }

    public void SetZoom(float? zoom = null)
    {
      followZoom.m_Width = zoom ?? defaultZoom;
    }
  }
}