using UnityEngine;

namespace Manager
{
  public class DontDestroyObject : MonoBehaviour
  {
    private void Awake()
    {
      DontDestroyOnLoad(gameObject);
    }
  }
}
