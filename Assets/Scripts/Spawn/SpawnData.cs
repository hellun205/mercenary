using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Spawn
{
  [Serializable]
  public class SpawnData : IData<SpawnData, SpawnDataSimply>
  {
    [Serializable]
    public class Enemy
    {
      [Serializable]
      public class Status
      {
        public float hp;
        public float damage;
        public float moveSpeed;
        public float dropCoin;
      }

      public string name;
      public Status defaultStatus;
      public Status increaseStatus;
    }

    [Serializable]
    public class Spawns
    {
      [Serializable]
      public class Spawn
      {
        public string name;
        public int count;
      }

      public Spawn[] spawns;
    }

    public Enemy[] enemyData;

    public Spawns[] spawns;

    public float[] waveTime;

    public SpawnDataSimply ToSimply() =>
      new SpawnDataSimply()
      {
        enemy = enemyData.ToDictionary(x => x.name, x => (x.defaultStatus, x.increaseStatus)),
        spawn = spawns.Select(x => x.spawns.ToDictionary(y => y.name, y => y.count)).ToArray(),
        waveTime = (float[]) waveTime.Clone()
      };

    public SpawnData Parse(SpawnDataSimply simplyData)
    {
      enemyData = simplyData.enemy.Select
      (
        x => new Enemy()
        {
          name = x.Key,
          defaultStatus = x.Value.defStatus,
          increaseStatus = x.Value.incStatus
        }
      ).ToArray();

      spawns = simplyData.spawn.Select
      (
        x => new Spawns() { spawns = x.Select(y => new Spawns.Spawn() { name = y.Key, count = y.Value }).ToArray() }
      ).ToArray();

      waveTime = (float[]) simplyData.waveTime.Clone();

      return this;
    }
  }
}
