using System;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
  [Serializable]
  public class AttributeChemistryData
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
  }
}
