using System;
using System.Collections;
using Manager;
using TMPro;
using UnityEngine;
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

    private Coroutiner timerCrt;

    protected override void Awake()
    {
      base.Awake();
      waveText = GameManager.UI.Find<TextMeshProUGUI>("$wave");
      timerText = GameManager.UI.Find<TextMeshProUGUI>("$timer");
      timerCrt = new Coroutiner(TimerRoutine);
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
      Debug.Log($"ended wave: {currentWave}.");
      Utils.Wait(3f, () => NextWave());
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
