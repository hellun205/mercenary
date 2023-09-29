using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using Spawn;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Weapon;

namespace Wave
{
  public class WaveManager : MonoBehaviour
  {
    public int currentWave;

    private TextMeshProUGUI timerText;
    private TextMeshProUGUI waveText;
    private Image storePanel;

    private Timer waveTimer = new();

    public event Action onWaveEnd;
    public event Action onWaveStart;

    public SpawnData.Spawns.Spawn[] spawns { get; private set; }
    public float[] times { get; private set; }

    private List<Timer> spawnTimers = new List<Timer>();
    
    public bool state { get; private set; }

    private void Awake()
    {
      waveText = GameManager.UI.Find<TextMeshProUGUI>("$wave");
      timerText = GameManager.UI.Find<TextMeshProUGUI>("$timer");
      storePanel = GameManager.UI.Find<Image>("$store", obj => obj.gameObject.SetActive(false));
      waveTimer.onTick += OnTimerTick;
      waveTimer.onEnd += OnTimerEnd;
      GameManager.UI.Find<Button>("$btn_nextwave").onClick.AddListener(() =>
      {
        Time.timeScale = 1f;
        storePanel.gameObject.SetActive(false);
        NextWave();
      });
      times = GameManager.Manager.spawn.data.waveTime;
    }

    private void OnTimerEnd(Timer sender)
    {
      EndWave();
    }

    private void OnTimerTick(Timer sender)
    {
      timerText.text = $"{Math.Max(0, times[currentWave] - Mathf.FloorToInt(sender.elapsedTime))}초";
    }

    public void StartWave()
    {
      spawnTimers.Clear();
      spawns = GameManager.Manager.spawn.GetSpawnData(currentWave);
      waveTimer.duration = times[currentWave];
      onWaveStart?.Invoke();
      for (int i = 0; i < spawns.Length; i++)
      {
        var timer = new Timer();
        timer.duration = times[currentWave] / spawns[i].count + 1;
        var i1 = i;
        timer.onEnd += t =>
        {
          GameManager.Spawn.SpawnRandomPos($"enemy/{spawns[i1].name}");
          t.Start();  
        };
        timer.Start();
        spawnTimers.Add(timer);
      }
      waveText.text = $"웨이브 {currentWave + 1}";
      
      state = true;
      waveTimer.Start();
    }

    public void EndWave()
    {
      foreach (var spawnTimer in spawnTimers)
        spawnTimer.Stop();
      spawnTimers.Clear();
      KillEnemies();
      state = false;
      onWaveEnd?.Invoke();

      CoroutineUtility.Start((new WaitForSeconds(1.5f), () =>
      {
        Time.timeScale = 0f;
        storePanel.gameObject.SetActive(true);
      }));
    }

    public void NextWave()
    {
      currentWave++;
      StartWave();
    }

    private void Start()
    {
      
    }

    private void KillEnemies()
    {
      var objs = FindObjectsOfType<TargetableObject>();
      foreach (var obj in objs)
      {
        obj.Kill();
      }
    }
  }
}
