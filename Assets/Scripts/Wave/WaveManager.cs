using System;
using System.Collections.Generic;
using Data;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Util.UI;
using Weapon;
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

    public SpawnData.Spawns.Spawn[] spawns { get; private set; }

    private List<Timer> spawnTimers = new List<Timer>();
    private Dictionary<string, int> leftCount = new Dictionary<string, int>();

    public bool state { get; private set; }

    // private readonly string[] uiNames = { "$coin_display", "$hp", "$timer", "$wave", "$menu_btn" };

    private void Awake()
    {
      waveText = GameManager.UI.Find<TextMeshProUGUI>("$wave");
      timerText = GameManager.UI.Find<TextMeshProUGUI>("$timer");
      storePanel = GameManager.UI.Find<Image>("$store");
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
    }

    private void SetUIEnabled(bool value)
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
        $"{Math.Max(0, GameManager.Data.data.GetWaveTime(currentWave) - Mathf.FloorToInt(sender.elapsedTime))}초";
    }

    public void StartWave(int index)
    {
      currentWave = index;
      StartWave();
    }

    public void StartWave()
    {
      SetUIEnabled(true);
      spawnTimers.Clear();
      leftCount.Clear();
      spawns = GameManager.Data.data.GetSpawnData(currentWave);
      waveTimer.duration = GameManager.Data.data.GetWaveTime(currentWave);
      try
      {
        onWaveStart?.Invoke();
      }
      catch(Exception ex)
      {
        Debug.Log(ex.Message);
      }
      

      foreach (var spawn in spawns)
      {
        var timer = new Timer();

        timer.duration = (GameManager.Data.data.GetWaveTime(currentWave) - spawn.delay) *
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
        timer.onEnd += t => { t.Start(); };
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
        GameManager.UI.Find<Camera>($"$player_display_camera").transform.position =
          GameManager.Player.transform.position.Setter(z: -5);
        Time.timeScale = 0f;
        storePanel.SetVisible(true, 0.1f);
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
