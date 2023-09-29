using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

namespace Weapon
{
  [Serializable]
  public class AttributeChemistryData :
    IData<AttributeChemistryData, Dictionary<Attribute, Dictionary<int, Dictionary<ApplyStatus, float>>>>
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

    public AttributeChemistryData Parse
    (
      Dictionary<Attribute, Dictionary<int, Dictionary<ApplyStatus, float>>> simplyData
    )
    {
      data = simplyData.Select(x => new AttributeItem()
      {
        type = x.Key,
        status = x.Value.Select(y => new AttributeItem.StatusItem()
        {
          count = y.Key,
          apply = y.Value.Select(z => new AttributeItem.StatusItem.ApplyItem() { type = z.Key, value = z.Value })
           .ToArray()
        }).ToArray()
      }).ToArray();

      return this;
    }
  }
}
