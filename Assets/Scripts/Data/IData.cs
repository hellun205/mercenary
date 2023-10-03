namespace Data
{
  public interface IData<TSource, TSimplyData>
  {
    public TSimplyData ToSimply();

    public TSource Parse(TSimplyData simplyData);
  }

  public interface ILoadable
  {
    
  }
}
