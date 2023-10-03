namespace Scene
{
  public interface ISceneChangeEventHandler
  {
    public void OnSceneChanged(string before, string after);
  }
}
