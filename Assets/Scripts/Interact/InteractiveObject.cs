using System;
using System.Collections.Generic;
using UnityEngine;

namespace Interact
{
  public class InteractiveObject : MonoBehaviour
  {
    [Header("Interact Setting")]
    public InteractType type;

    public InteractCaster detectCaster;

    public InteractCondition condition;

    private List<GameObject> interacters = new();

    public event Action<Interacter> onInteract;
    public Func<bool> additionalCondition;

    protected virtual void OnInteract(Interacter caster)
    {
    }

    public void Interact(Interacter interacter)
    {
      if ((condition & interacter.currentCondition) == 0) return;
      if ((detectCaster & interacter.caster) == 0) return;
      if (!AdditionalCondition() || !(additionalCondition?.Invoke() ?? true)) return;

      if (!interacters.Contains(interacter.gameObject))
        interacters.Add(interacter.gameObject);
      else if (type == InteractType.First) return;

      OnInteract(interacter);
      onInteract?.Invoke(interacter);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
      Remove(other.gameObject);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
      Remove(other.gameObject);
    }

    public void Remove(GameObject go)
    {
      if (interacters.Contains(go))
        interacters.Remove(go);
    }

    public virtual bool AdditionalCondition() => true;
  }
}
