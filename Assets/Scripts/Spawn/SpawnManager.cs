using System;
using Manager;
using Pool;
using UnityEngine;
using Util;

namespace Spawn
{
  public class SpawnManager : MonoBehaviour
  {
    public bool spawn;
    public int spawnCount;
    public float spawnDelay;
    public string[] spawnTarget;

    private float time;

    public event Action onSpawned;

    public void SpawnRandomPos(string targetPrefab, Action<PoolObject> setter = null)
      => SpawnRandomPos<PoolObject>(targetPrefab);

    public void SpawnRandomPos<T>(string targetPrefab, Action<T> setter = null) where T : Component
      => Spawn(GameManager.Map.GetRandom(), targetPrefab, setter);

    public void Spawn(Vector2 position, string targetPrefab, Action<PoolObject> setter = null)
      => Spawn<PoolObject>(position, targetPrefab, setter);

    public void Spawn<T>(Vector2 position, string targetPrefab, Action<T> setter = null) where T : Component
    {
      // Debug.Log(position);
      // if (
      //   position.x < GameManager.Map.start.position.x ||
      //   position.x > GameManager.Map.end.position.x ||
      //   position.y < GameManager.Map.start.position.y ||
      //   position.y > GameManager.Map.end.position.y
      // )
      //   throw new Exception("The position are out of bounds on the map.");

      // var go = Instantiate(GameManager.Prefab.Get(targetPrefab), position, Quaternion.identity);

      var warning = GameManager.Pool.Summon("ui/warning", position);
      warning.GetComponent<Animator>().SetFloat("speed", 2f);

      1.4f.Wait(() =>
      {
        warning.Release();
        if (GameManager.Wave.state)
        {
          GameManager.Pool.Summon(targetPrefab, position, setter);
          onSpawned?.Invoke();
        }
      });
    }

    private void Update()
    {
      if (spawn)
      {
        time += Time.deltaTime;

        if (time >= spawnDelay)
        {
          time = 0;

          for (var i = 0; i < spawnCount; i++)
          {
            SpawnRandomPos(spawnTarget.GetRandom());
          }
        }
      }
      else
      {
        time = 0;
      }
    }

    public void StartSpawn(int count, float delay, params string[] target)
    {
      spawnTarget = target;
      spawnCount = count;
      spawnDelay = delay;
      spawn = true;
    }

    [ContextMenu("Stop Spawn")]
    public void StopSpawn()
    {
      spawn = false;
    }
  }
}