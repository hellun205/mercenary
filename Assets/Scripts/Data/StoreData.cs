using System;

namespace Data
{
  [Serializable]
  public class StoreData
    : IData<StoreData, (int[] refreshPrices, float[] tierProbabilities, float[]additionalProbabilitiesOfWaves)>,
      ILoadable
  {
    public int[] refreshPrices;
    public float[] tierProbabilities;
    public float[] additionalProbabilitiesOfWaves;

    public (int[] refreshPrices, float[] tierProbabilities, float[] additionalProbabilitiesOfWaves) ToSimply()
      => (refreshPrices, tierProbabilities, additionalProbabilitiesOfWaves);

    public StoreData Parse((int[] refreshPrices, float[] tierProbabilities, float[] additionalProbabilitiesOfWaves) simplyData)
    {
      refreshPrices = simplyData.refreshPrices;
      tierProbabilities = simplyData.tierProbabilities;
      additionalProbabilitiesOfWaves = simplyData.additionalProbabilitiesOfWaves;

      return this;
    }
  }
}