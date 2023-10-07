using Cinemachine;
using Player;
using UnityEngine;

namespace Manager
{
  public class ManagerLoader : MonoBehaviour
  {
    public bool loadPlayer = true;
    public bool loadManager = true;
    public bool loadCamera = true;

    private void Awake()
    {
      if (loadManager && FindObjectOfType<GameManager>() == null)
        Instantiate(Resources.Load("@managers"));
      
      if (loadPlayer && FindObjectOfType<PlayerController>() == null)
        Instantiate(Resources.Load("@player"));

      if (loadCamera && FindObjectOfType<CinemachineVirtualCamera>() == null)
        Instantiate(Resources.Load("@camera"));
      
      Destroy(gameObject);
    }
  }
}