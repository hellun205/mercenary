using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Manager;
using Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Util.UI;

namespace Window.Contents
{
  public struct SettingValue
  {
    public Resolution resolution { get; set; }
    public FullScreenMode fullScreenMode { get; set; }
    public Dictionary<SoundType, float> volumes { get; set; }
    public Dictionary<string, KeyCode?[]> keys { get; set; }

    public void Apply()
    {
      Screen.SetResolution(resolution.width, resolution.height, fullScreenMode, resolution.refreshRateRatio);
      foreach (var (type, volume) in volumes)
        GameManager.Sound.SetVolume(type, volume <= -35 ? -80 : volume);
      GameManager.Data.SetKeySetting(keys);
      GameManager.Data.SetVolumeSetting(volumes);
    }
  }

  public class SettingWindow : WindowContent<bool>
  {
    public override WindowType type => WindowType.Setting;

    // Child Objects
    [Header("Setting Window"), SerializeField]
    private TabPage tabPage;

    [SerializeField]
    private RadioButtonList tabPageButtons;

    [Header("Screen"), SerializeField]
    private TMP_Dropdown resolutionDropDown;

    [SerializeField]
    private Toggle fullScreenToggle;

    [SerializeField]
    private Button applyButton;

    [SerializeField]
    private Button cancelButton;

    [SerializeField]
    private SerializedDictionary<SoundType, Slider> soundSliders;

    [SerializeField]
    private Transform keys;

    // Variables
    private List<Resolution> resolutions;

    private Dictionary<SoundType, float> tmpVolumes;

    public SettingValue settingValue;

    private Dictionary<string, InputKeyButton[]> keyButtons = new();

    protected override void Awake()
    {
      base.Awake();
      tabPageButtons.onChanged += tabName => tabPage.SetEnable(tabName);

      // Screen Setting
      resolutions = Screen.resolutions.ToList();

      resolutionDropDown.onValueChanged.AddListener(x => settingValue.resolution = resolutions[x]);
      resolutionDropDown.options = resolutions.Select
      (
        x => new TMP_Dropdown.OptionData { text = $"{x.width}x{x.height} ({Math.Floor(x.refreshRateRatio.value)}hz)" }
      ).ToList();
      resolutionDropDown.RefreshShownValue();
      fullScreenToggle.onValueChanged.AddListener
      (
        isFull => settingValue.fullScreenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed
      );

      // Sound Setting
      tmpVolumes = Enum.GetValues(typeof(SoundType)).OfType<SoundType>().ToDictionary(x => x, _ => 0f);
      foreach (var (soundType, slider) in soundSliders)
        slider.onValueChanged.AddListener(volume => tmpVolumes[soundType] = volume);

      applyButton.onClick.AddListener(OnApplyButtonClick);
      cancelButton.onClick.AddListener(() => Return(false));

      // Key Setting
      var items = new List<Transform>();
      for (var i = 0; i < keys.childCount; i++)
        items.Add(keys.GetChild(i));

      foreach (var item in items)
        keyButtons.Add(item.name, item.GetComponentsInChildren<InputKeyButton>());

      settingValue.keys = keyButtons.ToDictionary
      (
        x => x.Key,
        x => x.Value.Select(y => y.key).ToArray()
      );

      foreach (var (key, buttons) in keyButtons)
      {
        for (var i = 0; i < buttons.Length; i++)
        {
          var i1 = i;
          buttons[i].onKeyChanged += keyCode => settingValue.keys[key][i1] = keyCode;
        }
      }
    }

    public void SetValuesToCurrent()
    {
      var dict = new Dictionary<string, KeyCode?[]>();
      foreach (var (uniqueKey, keyCodes) in GameManager.Data.data.keys)
      {
        var list = new List<KeyCode?>();
        for (var i = 0; i < 2; i++)
          list.Add(keyCodes.Length > i ? keyCodes[i] : null);

        dict.Add(uniqueKey, list.ToArray());
      }

      SetValues(new SettingValue
      {
        resolution = Screen.currentResolution,
        fullScreenMode = FullScreenMode.FullScreenWindow,
        volumes = Enum.GetValues(typeof(SoundType))
         .OfType<SoundType>()
         .ToDictionary
          (
            x => x,
            x => GameManager.Sound.GetVolume(x)
          ),

        keys = dict
      });
    }

    private void OnApplyButtonClick()
    {
      var askBox = GameManager.Window.Open(WindowType.AskBox).GetContent<AskBox>();
      askBox.window.title = "설정";
      askBox.message = "설정을 적용하시겠습니까?";
      askBox.onReturn = res =>
      {
        if (res == AskBoxResult.Yes)
        {
          settingValue.volumes = tmpVolumes;
          settingValue.Apply();
        }
      };
    }

    public void SetValues(SettingValue values)
    {
      settingValue = values;
      resolutionDropDown.value = resolutions.FindIndex(x => x.Equals(settingValue.resolution));
      fullScreenToggle.isOn = settingValue.fullScreenMode.Equals(FullScreenMode.FullScreenWindow);
      foreach (var (soundType, volume) in settingValue.volumes)
        soundSliders[soundType].value = volume;
      foreach (var (uniqueKey, keyCodes) in values.keys)
        for (var i = 0; i < keyButtons[uniqueKey].Length; i++)
          keyButtons[uniqueKey][i].key = keyCodes[i];
    }

    private void Start()
    {
      SetValuesToCurrent();
    }
  }
}
