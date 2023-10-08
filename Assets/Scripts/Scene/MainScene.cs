using System;
using System.Collections.Generic;
using System.Linq;
using Manager;
using Sound;
using Transition;
using UI;
using UI.Select;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Util.Text;
using Weapon;

namespace Scene
{
  public class SelectableWeaponData : ISelectable
  {
    public Sprite icon { get; set; }
    public string description { get; set; }
    public int index { get; set; }
    public string weaponName { get; set; }
  }

  public class MainScene : MonoBehaviour
  {
    private Slide slide;
    private WeaponSelectPanel weaponSelect;
    private SelectPanel stageSelect;

    private Button weaponNext;
    private Button stageNext;

    private void Start()
    {
      slide = GameManager.UI.Find<Slide>("$main_slide");
      weaponSelect = GameManager.UI.Find<WeaponSelectPanel>("$weapons");
      stageSelect = GameManager.UI.Find<SelectPanel>("$stages");
      GameManager.UI.FindAll<Button>("$main_next", b => b.onClick.AddListener(slide.SlideNextPage));
      GameManager.UI.FindAll<Button>("$main_back", b => b.onClick.AddListener(slide.SlidePreviousPage));
      weaponNext = GameManager.UI.Find("$weapon_next").GetComponentInChildren<Button>();
      stageNext = GameManager.UI.Find("$stage_next").GetComponentInChildren<Button>();
      GameManager.UI.Find<Button>("$game_start_btn").onClick.AddListener(OnGameStart);
      GameManager.UI.Find<Button>("$game_exit_btn").onClick.AddListener(GameManager.Manager.AskExit);

      weaponSelect.AddItems(GameManager.WeaponData.items.Values.Select
        (
          x => new SelectableWeaponData
          {
            weaponName = x.specfiedName,
            description =
              $"{x.itemName.SetSizePercent(1.3f).AddBold().SetLineHeight(1.5f)}" +
              $"\n{x.attribute.GetTexts().SetSizePercent(1.15f).AddColor(new Color32(107, 102, 255, 255))}" +
              $"\n{x.GetDescription(0)}",
            icon = x.icon
          }
        ).ToArray()
      );

      for (var i = 0; i < 5; i++)
      {
        stageSelect.AddItem(new SelectableItem
        {
          icon = GameManager.Sprites.Get($"{i + 1}"),
          description = ""
        });
      }
    }

    private void Update()
    {
      weaponNext.interactable = weaponSelect.selectedItem is not null;
      stageNext.interactable = stageSelect.selectedItem is not null;
    }

    private void OnGameStart()
    {
      GameManager.Manager.StartStage(stageSelect.selectedItem.value.index,weaponSelect.selectedItem.value.weaponName);
    }
  }
}
