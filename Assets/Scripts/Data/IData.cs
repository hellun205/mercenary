namespace Data
{
  public interface IData<out T>
  {
    public T ToSimply();
  }
}
