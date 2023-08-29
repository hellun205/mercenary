using Manager;
using Pool;
using UnityEngine;
using Util;

namespace Spawn
{
  public class SpawnManager : MonoBehaviourSingleTon<SpawnManager>
  {
    public bool spawn;
    public int spawnCount;
    public float spawnDelay;
    public string[] spawnTarget;

    private float time;

    public PoolObject SpawnRandomPos(string targetPrefab)
      => SpawnRandomPos<PoolObject>(targetPrefab);

    public T SpawnRandomPos<T>(string targetPrefab) where T : Component
    {
      return Spawn<T>(GameManager.Map.GetRandom(), targetPrefab);
    }

    public PoolObject Spawn(Vector2 position, string targetPrefab)
      => Spawn<PoolObject>(position, targetPrefab);

    public T Spawn<T>(Vector2 position, string targetPrefab) where T : Component
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
      var go = GameManager.Pool.Summon<T>(targetPrefab, position);
      return go;
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
