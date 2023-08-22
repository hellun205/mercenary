namespace Manager
{
  public class SingleTon<T> where T: SingleTon<T>, new()
  {
    private static T _instance;

    public static T Instance => _instance ??= new T();
  }
}