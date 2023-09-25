using UnityEngine;

namespace Util
{
  public class Yield
  {
    private readonly YieldInstruction baseYieldInstruction;
    private readonly CustomYieldInstruction customYieldInstruction;

    public object value => (object)baseYieldInstruction ?? customYieldInstruction;

    public Yield(YieldInstruction yield)
    {
      baseYieldInstruction = yield;
    }

    public Yield(CustomYieldInstruction yield)
    {
      customYieldInstruction = yield;
    }
    
    public static implicit operator Yield(YieldInstruction yield) => new (yield);
    public static implicit operator Yield(CustomYieldInstruction yield) => new (yield);
  }
}
