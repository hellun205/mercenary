namespace Scene
{
  public interface IBeforeSceneChangeEventHandler
  {
    public void OnBeforeSceneChange(string before, string after);
  }
}
