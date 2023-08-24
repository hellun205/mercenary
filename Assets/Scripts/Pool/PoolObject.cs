using Manager;
using UnityEngine;

namespace Pool
{
  public sealed class PoolObject : MonoBehaviour
  {
    [Header("Pool Object")]
    public int index;
    public string type;

    public delegate void PoolObjectEventListener();
    public event PoolObjectEventListener onGet;
    public event PoolObjectEventListener onReleased;

    public void OnGet()
    {
      onGet?.Invoke();
    }

    public void OnReleased()
    {
      onReleased?.Invoke();
    }

    public void Release() => GameManager.Pool.pools[type].Release(this);
  }
}