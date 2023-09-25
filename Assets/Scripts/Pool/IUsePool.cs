namespace Pool
{
  public interface IUsePool
  {
    public PoolObject poolObject { get; set; }

    public void OnKilled()
    {
    }

    public void OnSummon()
    {
    }
  }
}
