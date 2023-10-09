using System;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scene
{
  public class GameClearScene : MonoBehaviour
  {
    private void Start()
    {
      GameManager.UI.Find<Button>("$gameclear_restart_btn").onClick.AddListener(Restart);
      GameManager.UI.Find<Button>("$gameclear_mainmenu_btn").onClick.AddListener(GoToMain);
      GameManager.UI.Find<Button>("$gameclear_exit_btn").onClick.AddListener(ExitGame);
      var title = GameManager.UI.Find<TextMeshProUGUI>("$title");
      var stage = GameManager.Manager.currentStage;
      var stageText = stage == 0 ? "튜토리얼" : $"{stage}";
      title.text = string.Format(title.text, stageText);
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
  }
}
