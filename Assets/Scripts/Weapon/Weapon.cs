using UnityEngine;

namespace Weapon
{
  public abstract class Weapon : ScriptableObject
  {
    public Sprite icon;

    public Sprite sprite;

    [Multiline]
    public string descriptions;

    public IncreaseStatus status;
  }
}