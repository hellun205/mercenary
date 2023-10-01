using System;
using System.Collections.Generic;
using System.Linq;

namespace Data
{
  public struct SpawnDataSimply
  {
    public struct Enemy
    {
      public struct Status
      {
        public float hp;
        public float damage;
        public float moveSpeed;
        public float dropCoin;
      }

      public Status defaultStatus;
      public Status increaseStatus;

      public Dictionary<SpawnData.Enemy.Detail.StatusTypes, float> detailStatus;
    }

    public struct Spawn
    {
      public int count;
    }

    public Dictionary<string, Enemy> enemies;
    public Dictionary<string, Spawn>[] spawns;
    public float[] waves;
  }

  [Serializable]
  public class SpawnData
    : IData<SpawnData, SpawnDataSimply>,
      ILoadable
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

      [Serializable]
      public class Detail
      {
        public enum StatusTypes
        {
          Range, BulletSpeed, AttackSpeed,
          IncreaseMoveSpeedPerSecond, MaxMoveSpeed
        }

        public StatusTypes status;
        public float value;
      }

      public string name;
      public Status defaultStatus;
      public Status increaseStatus;

      public Detail[] detailStatus;
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

    public SpawnDataSimply ToSimply()
      => new SpawnDataSimply
      {
        enemies = enemyData.ToDictionary
        (
          x => x.name,
          x => new SpawnDataSimply.Enemy
          {
            defaultStatus = new SpawnDataSimply.Enemy.Status
            {
              hp = x.defaultStatus.hp,
              damage = x.defaultStatus.damage,
              moveSpeed = x.defaultStatus.moveSpeed,
              dropCoin = x.defaultStatus.dropCoin
            },
            increaseStatus = new SpawnDataSimply.Enemy.Status
            {
              hp = x.increaseStatus.hp,
              damage = x.increaseStatus.damage,
              moveSpeed = x.increaseStatus.moveSpeed,
              dropCoin = x.increaseStatus.dropCoin
            },
            detailStatus = x.detailStatus.ToDictionary(y => y.status, y => y.value)
          }
        ),
        spawns = spawns.Select
        (
          x => x.spawns.ToDictionary
          (
            y => y.name,
            y => new SpawnDataSimply.Spawn
            {
              count = y.count
            }
          )
        ).ToArray(),
        waves = waveTime
      };

    public SpawnData Parse(SpawnDataSimply simplyData)
    {
      enemyData = simplyData.enemies.Select
      (
        x => new Enemy
        {
          name = x.Key,
          defaultStatus = new Enemy.Status
          {
            hp = x.Value.defaultStatus.hp,
            damage = x.Value.defaultStatus.damage,
            moveSpeed = x.Value.defaultStatus.moveSpeed,
            dropCoin = x.Value.defaultStatus.dropCoin,
          },
          increaseStatus = new Enemy.Status
          {
            hp = x.Value.increaseStatus.hp,
            damage = x.Value.increaseStatus.damage,
            moveSpeed = x.Value.increaseStatus.moveSpeed,
            dropCoin = x.Value.increaseStatus.dropCoin,
          },
          detailStatus = x.Value.detailStatus.Select
          (
            y => new Enemy.Detail
            {
              status = y.Key,
              value = y.Value
            }
          ).ToArray()
        }
      ).ToArray();

      spawns = simplyData.spawns.Select
      (
        x => new Spawns
        {
          spawns = x.Select
          (
            y => new Spawns.Spawn
            {
              name = y.Key,
              count = y.Value.count
            }
          ).ToArray()
        }
      ).ToArray();

      waveTime = simplyData.waves;

      return this;
    }
  }
}
