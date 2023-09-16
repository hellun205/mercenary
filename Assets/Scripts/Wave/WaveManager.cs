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
  public class WaveManager : MonoBehaviourSingleTon<WaveManager>
  {
    public WaveSetting currentSetting;

    public int currentWave;

    public float time;

    private TextMeshProUGUI timerText;
    private TextMeshProUGUI waveText;
    private Image storePanel;

    private Coroutiner timerCrt;

    protected override void Awake()
    {
      base.Awake();
      waveText = GameManager.UI.Find<TextMeshProUGUI>("$wave");
      timerText = GameManager.UI.Find<TextMeshProUGUI>("$timer");
      storePanel = GameManager.UI.Find<Image>("$store", obj => obj.gameObject.SetActive(false));
      timerCrt = new Coroutiner(TimerRoutine);
      GameManager.UI.Find<Button>("$btn_nextwave").onClick.AddListener(() =>
      {
        Time.timeScale = 1f;
        storePanel.gameObject.SetActive(false);
        NextWave();
      });
    }

    private IEnumerator TimerRoutine()
    {
      while (time > 0)
      {
        time -= Time.deltaTime;
        timerText.text = $"{Math.Max(0, Mathf.CeilToInt(time))}초";
        yield return new WaitForEndOfFrame();
      }

      EndWave();
    }

    public void StartWave()
    {
      var waveData = currentSetting.GetData(currentWave);
      time = waveData.waveTime;
      GameManager.Spawn.spawnCount = waveData.count;
      GameManager.Spawn.spawnDelay = waveData.delay;
      GameManager.Spawn.spawnTarget = waveData.enemy;
      GameManager.Spawn.spawn = true;
      waveText.text = $"웨이브 {currentWave + 1}";

      timerCrt.Start();
    }

    public void EndWave()
    {
      timerCrt.Stop();
      GameManager.Spawn.spawn = false;
      KillEnemies();

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
      StartWave();
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
