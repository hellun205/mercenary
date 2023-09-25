namespace UI.Popup
{
  public class ListPopup : PopupPanel
  {
    public PopupPanel[] list;

    public void ShowPopup(string originalValue, params string[] value)
    {
      HidePopup();
      base.ShowPopup();
      valueText.text = originalValue;
      for (var i = 0; i < value.Length; i++)
        list[i].ShowPopup(value[i]);
    }

    public new void HidePopup()
    {
      foreach (var popupPanel in list)
        popupPanel.HidePopup();

      base.HidePopup();
    }
  }
}
