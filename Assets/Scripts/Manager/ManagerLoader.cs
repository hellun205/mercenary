using System;
using Cinemachine;
using Player;
using UnityEngine;
using Util;

namespace Manager
{
  public class ManagerLoader : MonoBehaviour
  {
    public bool loadPlayer = true;
    public bool loadManager = true;
    public bool loadCamera = true;

    private void Awake()
    {
      if (GameObject.Find("@game") == null)
      {
        var o = Instantiate(Resources.Load("@game"));
        o.name = "@game";
      }
      
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