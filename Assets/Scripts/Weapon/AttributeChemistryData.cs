using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

namespace Weapon
{
  [Serializable]
  public class AttributeChemistryData : IData<Dictionary<Attribute, Dictionary<int, Dictionary<ApplyStatus, float>>>>
  {
    [Serializable]
    public class AttributeItem
    {
      [Serializable]
      public class StatusItem
      {
        [Serializable]
        public class ApplyItem
        {
          public ApplyStatus type;
          public float value;
        }

        public int count;
        public ApplyItem[] apply;
      }

      public Attribute type;
      public StatusItem[] status;
    }

    // public Dictionary<Attribute, Dictionary<string, Dictionary<ApplyStatus, float>>> data;
    public AttributeItem[] data;

    public Dictionary<Attribute, Dictionary<int, Dictionary<ApplyStatus, float>>> ToSimply()
      => data.ToDictionary
      (
        ai => ai.type,
        ai => ai.status.ToDictionary
        (
          si => si.count,
          si => si.apply.ToDictionary
          (
            api => api.type,
            api => api.value
          )
        )
      );
  }
}
