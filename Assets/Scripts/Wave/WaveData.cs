using System;

namespace Wave
{
  [Serializable]
  public struct WaveData
  {
    public float waveTime;
    public float delay;
    public int count;
    public string[] enemy;
  }
}
