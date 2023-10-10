using System;
using System.Collections.Generic;
using System.Linq;
using Sound;

namespace Data
{
  [Serializable]
  public class VolumeData : IData<VolumeData,Dictionary<SoundType, float>>, ILoadable
  {
    [Serializable]
    public class Volume
    {
      public SoundType type;
      public float volume;
    }

    public Volume[] data;

    public Dictionary<SoundType, float> ToSimply()
      => data.ToDictionary(x => x.type, x => x.volume);

    public VolumeData Parse(Dictionary<SoundType, float> simplyData)
    {
      data = simplyData.Select(x => new Volume { type = x.Key, volume = x.Value }).ToArray();
      return this;
    }
  }
}
