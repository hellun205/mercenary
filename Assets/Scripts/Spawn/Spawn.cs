using System.Linq;
using UnityEngine;
using Util;
using Weapon;

namespace Spawn
{
  public class Spawn
  {
    public SpawnDataSimply data;

    public Spawn(string jsonData)
    {
      data = JsonUtility.FromJson<SpawnData>(jsonData).ToSimply();
    }

    public SpawnData.Enemy.Status GetEnemyDefaultStatus(string name) => data.enemy[name].defStatus;

    public SpawnData.Enemy.Status GetEnemyIncreaseStatus(string name) => data.enemy[name].incStatus;

    public SpawnData.Enemy.Status GetEnemyStatus(string name, int wave)
    {
      var def = GetEnemyDefaultStatus(name);
      var inc = GetEnemyIncreaseStatus(name);

      var res = new SpawnData.Enemy.Status()
      {
        hp = def.hp,
        damage = def.damage,
        moveSpeed = def.moveSpeed,
        dropCoin = def.dropCoin,
      };

      wave.For(_ =>
      {
        res.hp += inc.hp;
        res.damage += inc.damage;
        res.moveSpeed += inc.moveSpeed;
        res.dropCoin += inc.dropCoin;
      });

      return res;
    }

    public SpawnData.Spawns.Spawn[] GetSpawnData(int wave)
      => data.spawn[wave].Select(x => new SpawnData.Spawns.Spawn() { name = x.Key, count = x.Value }).ToArray();
  }
}
