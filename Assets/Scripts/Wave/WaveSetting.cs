using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Wave
{
  [CreateAssetMenu(fileName = "WaveSetting", menuName = "Wave Setting", order = 0)]
  public class WaveSetting : ScriptableObject
  {
    public WaveData defaultData;
    
    [SerializedDictionary("wave", "changes")]
    public SerializedDictionary<int, WaveData> data;

    public WaveData GetData(int index)
    {
      if (data.ContainsKey(index))
      {
        return data[index];
      }
      else
      {
        var find = data.Keys.LastOrDefault(i => i < index);
        return find == default ? defaultData : data[find];
      }
    }
  }
}
