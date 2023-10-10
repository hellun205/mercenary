using System;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scene
{
  public enum GameOverReason
  {
    Dead,
  }

  public class GameOverScene : MonoBehaviour
  {
    private void Start()
    {
      GameManager.UI.Find<Button>("$gameover_restart_btn").onClick.AddListener(Restart);
      GameManager.UI.Find<Button>("$gameover_mainmenu_btn").onClick.AddListener(GoToMain);
      GameManager.UI.Find<Button>("$gameover_exit_btn").onClick.AddListener(ExitGame);
      var title = GameManager.UI.Find<TextMeshProUGUI>("$title");
      title.text = string.Format(title.text, GetReason(), GetDesc());
    }

    private void ExitGame()
    {
      GameManager.Manager.AskExit();
    }

    private void GoToMain()
    {
      GameManager.Manager.AskGotoMain();
    }

    private void Restart()
    {
      GameManager.Manager.AskRestart();
    }

    private string GetReason()
    {
      return GameManager.Manager.gameOverReason switch
      {
        GameOverReason.Dead => "<color=red>죽었습니다!</color>",
        _                   => throw new ArgumentOutOfRangeException()
      };
    }

    private string GetDesc()
    {
      return GameManager.Manager.gameOverReason switch
      {
        GameOverReason.Dead => $"웨이브 {GameManager.Wave.currentWave + 1} 까지 생존하였습니다.",
        _                   => throw new ArgumentOutOfRangeException()
      };
    }
  }
}
