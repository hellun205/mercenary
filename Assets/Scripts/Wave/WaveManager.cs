using System;
using System.Collections.Generic;
using Data;
using Manager;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Util.UI;
using Weapon;
using Window;
using Window.Contents;
using Random = UnityEngine.Random;

namespace Wave
{
  public class WaveManager : MonoBehaviour
  {
    public int currentWave { get; private set; }

    private TextMeshProUGUI timerText;
    private TextMeshProUGUI waveText;
    private Image storePanel;

    private Timer waveTimer = new();

    public event Action onWaveEnd;
    public event Action onWaveStart;
    public event Action onStoreOpen;

    private int tutorialWave;

    public SpawnData.Spawns.Spawn[] spawns { get; private set; }

    private List<Timer> spawnTimers = new List<Timer>();
    private Dictionary<string, int> leftCount = new Dictionary<string, int>();

    public bool state { get; private set; }

    // private readonly string[] uiNames = { "$coin_display", "$hp", "$timer", "$wave", "$menu_btn" };

    private TextMeshProUGUI healBtnText;
    private Button healBtn;

    private int healPrice;
    private int healAmount;

    private ProgressBar hpBar;

    private void Awake()
    {
      waveText = GameManager.UI.Find<TextMeshProUGUI>("$wave");
      timerText = GameManager.UI.Find<TextMeshProUGUI>("$timer");
      storePanel = GameManager.UI.Find<Image>("$store");
      healBtn = GameManager.UI.Find<Button>("$heal_btn");
      healBtnText = healBtn.GetComponentInChildren<TextMeshProUGUI>();
      hpBar = GameManager.UI.Find<ProgressBar>("$hp_bar");
      healBtn.onClick.AddListener(AskHeal);
      waveTimer.onTick += OnTimerTick;
      waveTimer.onEnd += OnTimerEnd;
      GameManager.UI.Find<Button>("$btn_nextwave").onClick.AddListener(() =>
      {
        Utils.UnPause();
        NextWave();
        storePanel.SetVisible(false, 0.1f);
      });

      SetUIEnabled(false);
      storePanel.SetVisible(false);

      var pages = GameManager.UI.Find<TabPage>("$store_tab_pages");
      GameManager.UI.Find<RadioButtonList>("$store_tab_buttons").onChanged += select => pages.SetEnable(select);
      var inventories = GameManager.UI.Find<TabPage>("$inventory_tab_pages");
      GameManager.UI.Find<RadioButtonList>("$inventory_tab_buttons").onChanged +=
        select => inventories.SetEnable(select);
    }

    private void Update()
    {
      if (GameManager.Player == null) return;
      healAmount = Mathf.CeilToInt(GameManager.Player.currentStatus.maxHp - GameManager.Player.status.hp);
      healPrice = healAmount / 2;
      healBtnText.text = healAmount > 0 ? $"체력 회복\n${healPrice}" : "체력이 최대입니다.";
      healBtn.interactable = healAmount > 0;
      hpBar.maxValue = GameManager.Player.currentStatus.maxHp;
      hpBar.value = GameManager.Player.status.hp;
    }

    private void AskHeal()
    {
      // var askBox = GameManager.Window.Open(WindowType.AskBox).GetContent<AskBox>();
      // askBox.window.title = "체력 회복";
      // askBox.message = $"${healPrice}를 지불하고 체력을 모두 회복하시겠습니까?";
      // askBox.onReturn = res =>
      // {
      //   if (res == AskBoxResult.Yes)
      //   {
      if (GameManager.Manager.coin.value >= healPrice)
      {
        GameManager.Manager.coin.value -= healPrice;
        GameManager.Player.Heal(healAmount);
        GameManager.Broadcast.Say("체력을 모두 회복하였습니다.");
      }
      else
      {
        GameManager.Broadcast.Say("돈이 부족합니다.");
      }
      //   }
      // };
    }

    public void SetUIEnabled(bool value)
    {
      // foreach (var uiName in uiNames)
      GameManager.UI.Find($"$game_display").SetVisible(value, 0.3f);
    }

    private void OnTimerEnd(Timer sender)
    {
      EndWave();
    }

    private void OnTimerTick(Timer sender)
    {
      timerText.text =
        $"{Math.Max(0, sender.duration - Mathf.FloorToInt(sender.elapsedTime))}초";
    }

    public void StartWave(int index)
    {
      currentWave = index;
      tutorialWave = index;
      StartWave();
    }

    public void StartWave()
    {
      SetUIEnabled(true);

      spawnTimers.Clear();
      leftCount.Clear();

      if (GameManager.isTutorial)
      {
        switch (tutorialWave)
        {
          case 0:
            spawns = new[]
            {
              new SpawnData.Spawns.Spawn
              {
                name = "normal",
                count = 15,
                range = 10,
                delay = 0,
                simultaneousSpawnCount = 1
              }
            };
            waveTimer.duration = 10f;
            break;

          case 1:
            spawns = new[]
            {
              new SpawnData.Spawns.Spawn
              {
                name = "normal",
                count = 30,
                range = 10,
                delay = 0,
                simultaneousSpawnCount = 2
              }
            };
            waveTimer.duration = 20f;
            break;
        }
      }
      else
      {
        spawns = GameManager.Data.data.GetSpawnData(currentWave);
        waveTimer.duration = GameManager.Data.data.GetWaveTime(currentWave);
      }

      try
      {
        onWaveStart?.Invoke();
      }
      catch (Exception ex)
      {
        Debug.Log(ex.Message);
      }

      foreach (var spawn in spawns)
      {
        var timer = new Timer();

        timer.duration = (GameManager.Data.data.GetWaveTime(currentWave) - spawn.delay - 1.4f) *
          spawn.simultaneousSpawnCount / spawn.count;

        timer.onStart += t =>
        {
          var spawnPosition = GameManager.Map.GetRandom();
          for (var i = 0; i < spawn.simultaneousSpawnCount; i++)
          {
            if (leftCount[spawn.name] <= 0) break;
            var pos = Random.insideUnitCircle * (spawn.range / 10);
            GameManager.Spawn.Spawn(spawnPosition + pos, $"enemy/{spawn.name}");
            leftCount[spawn.name]--;
          }
        };
        timer.onEnd += t => t.Start();
        spawnTimers.Add(timer);
        leftCount.Add(spawn.name, spawn.count);

        CoroutineUtility.Wait(spawn.delay, () => timer.Start());
      }

      waveText.text = $"웨이브 {currentWave + 1}";

      state = true;
      waveTimer.Start();
    }

    public void EndWave(bool start = true)
    {
      if
      (
        (start && GameManager.isTutorial && tutorialWave == 1) ||
        (start && GameManager.Data.data.spawns.waves.Length - 1 <= currentWave)
      )
      {
        tutorialWave = 0;
        GameManager.Manager.GameClear(false);
        return;
      }

      if (GameManager.isTutorial)
        tutorialWave++;

      if (!state) return;
      foreach (var spawnTimer in spawnTimers)
        spawnTimer.Stop();
      spawnTimers.Clear();
      KillEnemies();
      state = false;
      onWaveEnd?.Invoke();
      SetUIEnabled(false);
      waveTimer.Stop();

      if (!start) return;
      CoroutineUtility.Start((new WaitForSeconds(1.5f), () =>
      {
        GameManager.UI.Find<Camera>("$player_display_camera").transform.position =
          GameManager.Player.transform.position.Setter(z: -5);
        GameManager.UI.Find<Camera>("$partner1_display_camera").transform.position =
          GameManager.Player.partners[0].transform.position.Setter(z: -5);
        GameManager.UI.Find<Camera>("$partner2_display_camera").transform.position =
          GameManager.Player.partners[1].transform.position.Setter(z: -5);

        var btn = GameManager.UI.Find<Button>("$btn_nextwave");
        btn.GetComponentInChildren<TextMeshProUGUI>().text = string.Format
        (
          "다음 웨이브\n({0}/{1})",
          currentWave + 2,
          GameManager.isTutorial ? 2 : GameManager.Data.data.spawns.waves.Length
        );

        Time.timeScale = 0f;
        storePanel.SetVisible(true, 0.1f);
        onStoreOpen?.Invoke();
      }));
    }

    public void NextWave()
    {
      currentWave++;
      StartWave();
    }

    private void KillEnemies()
    {
      var objs = FindObjectsOfType<TargetableObject>();
      foreach (var obj in objs)
      {
        obj.Kill(false);
      }
    }
  }
}