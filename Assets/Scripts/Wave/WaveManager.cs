using System;
using System.Collections;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Weapon;

namespace Wave
{
  public class WaveManager : MonoBehaviour
  {
    public WaveSetting currentSetting;

    public int currentWave;

    private TextMeshProUGUI timerText;
    private TextMeshProUGUI waveText;
    private Image storePanel;

    private Timer waveTimer = new();

    public event Action onWaveEnd;
    public event Action onWaveStart;

    private WaveData waveData;
    
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
    }

    private void OnTimerEnd(Timer sender)
    {
      EndWave();
    }

    private void OnTimerTick(Timer sender)
    {
      timerText.text = $"{Math.Max(0, waveData.waveTime - Mathf.FloorToInt(sender.elapsedTime))}초";
    }

    public void StartWave()
    {
      waveData = currentSetting.GetData(currentWave);
      waveTimer.duration = waveData.waveTime;
      GameManager.Spawn.spawnCount = waveData.count;
      GameManager.Spawn.spawnDelay = waveData.delay;
      GameManager.Spawn.spawnTarget = waveData.enemy;
      GameManager.Spawn.spawn = true;
      waveText.text = $"웨이브 {currentWave + 1}";
      onWaveStart?.Invoke();

      state = true;
      waveTimer.Start();
    }

    public void EndWave()
    {
      GameManager.Spawn.spawn = false;
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
      // StartWave();
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
