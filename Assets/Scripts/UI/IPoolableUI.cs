using UnityEngine;

namespace UI
{
  public interface IPoolableUI<T> where T : Component
  {
    public GameObject gameObject => component.gameObject;
    public Transform transform => component.transform;
    public T component { get; }

    public void Ready() => gameObject.SetActive(true);
  }
}
