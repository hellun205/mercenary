using System.Collections.Generic;

namespace Spawn
{
  public class SpawnDataSimply
  {
    public Dictionary<string, (SpawnData.Enemy.Status defStatus, SpawnData.Enemy.Status incStatus)> enemy;

    public Dictionary<string, int>[] spawn;

    public float[] waveTime;
  }
}
