using System;
using Manager;
using UnityEngine;

namespace Weapon
{
  public class WeaponController : MonoBehaviour
  {
    public Weapon weaponData;

    public GameObject target;

    private void Update()
    {
      if (target is null)
      {
        var hit = Physics2D.CircleCast(GameManager.Player.transform.position, weaponData.status.range, Vector2.up, 0, LayerMask.GetMask("Enemy"));

        if (hit)
        {
          target = hit.transform.gameObject;
        }
      }
      else
      {
        var rotation = Quaternion.Euler(0, 0,
          Mathf.Atan2(target.transform.position.y, target.transform.position.x) * Mathf.Rad2Deg);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 5f);
      }
    }
  }
}