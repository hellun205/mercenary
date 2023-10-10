using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Manager;
using Transition;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;
using Object = UnityEngine.Object;

namespace Scene
{
  public class SceneLoader
  {
    public static bool isLoading;

    private Action callbackOnComplete;

    private Action callbackOnStartIn;

    private Action callbackOnEndIn;

    private Action callbackOnStartOut;

    private Action callbackOnEndOut;

    private Coroutine loadCrt;

    private TransitionOption inTransition = Transitions.IN;

    private TransitionOption outTransition = Transitions.OUT;

    private bool isSmooth = false;

    private bool isPause = false;

    private string beforeSceneName;

    private string afterSceneName;

    public Coroutine coroutine;

    public SceneLoader(string afterSceneName)
    {
      beforeSceneName = SceneManager.GetActiveScene().name;
      this.afterSceneName = afterSceneName;
    }

    public SceneLoader In(TransitionOption value)
    {
      inTransition = value;
      return this;
    }

    public SceneLoader In(string type, float speed = 1f, float delay = 0f)
    {
      inTransition = new TransitionOption(type, speed, delay);
      return this;
    }

    public SceneLoader Out(TransitionOption value)
    {
      outTransition = value;
      return this;
    }

    public SceneLoader Out(string type, float speed = 1f, float delay = 0f)
    {
      outTransition = new TransitionOption(type, speed, delay);
      return this;
    }

    public SceneLoader OnComplete(Action value)
    {
      callbackOnComplete = value;
      return this;
    }

    public SceneLoader OnEndIn(Action value)
    {
      callbackOnEndIn = value;
      return this;
    }

    public SceneLoader OnEndOut(Action value)
    {
      callbackOnEndOut = value;
      return this;
    }

    public SceneLoader OnStartIn(Action value)
    {
      callbackOnStartIn = value;
      return this;
    }

    public SceneLoader OnStartOut(Action value)
    {
      callbackOnStartOut = value;
      return this;
    }

    public SceneLoader PauseOnTransitioning(bool smooth = true)
    {
      isPause = true;
      isSmooth = smooth;

      return this;
    }

    public SceneLoader Load()
    {
      if (isLoading) return this;
      SceneManager.activeSceneChanged += SceneManagerOnactiveSceneChanged;
      coroutine = GameManager.CoroutineObject.StartCoroutine(LoadRoutine());
      return this;
    }

    private void SceneManagerOnactiveSceneChanged
    (
      UnityEngine.SceneManagement.Scene arg0,
      UnityEngine.SceneManagement.Scene arg1
    )
    {
      HandleSceneChanged();
    }

    private IEnumerator LoadRoutine()
    {
      isLoading = true;
      var load = SceneManager.LoadSceneAsync(afterSceneName);
      load.allowSceneActivation = false;

      yield return new WaitForSecondsRealtime(outTransition.delay);
      callbackOnStartOut?.Invoke();
      if (isPause) Utils.Pause(isSmooth, outTransition.speed * 0.9f);
      GameManager.Transition.Play(outTransition.type, outTransition.speed);

      yield return new WaitForSecondsRealtime(outTransition.speed);
      callbackOnEndOut?.Invoke();
      HandleBeforeSceneChanged();

      yield return new WaitUntil(() => load.progress >= 0.9f);
      load.allowSceneActivation = true;

      yield return new WaitForSecondsRealtime(inTransition.delay);
      callbackOnStartIn?.Invoke();
      if (isPause) Utils.UnPause(isSmooth, inTransition.speed * 0.9f);
      GameManager.Transition.Play(inTransition.type, inTransition.speed);

      yield return new WaitForSecondsRealtime(inTransition.speed);
      callbackOnEndIn?.Invoke();

      isLoading = false;
      callbackOnComplete?.Invoke();
      SceneManager.activeSceneChanged -= SceneManagerOnactiveSceneChanged;
    }

    public void HandleSceneChanged()
    {
      var p = typeof(GameManager).GetProperties(BindingFlags.Public | BindingFlags.Instance |
                                                BindingFlags.Static | BindingFlags.DeclaredOnly);

      foreach (var property in p.Select(x => x.GetValue(GameManager.Manager)).OfType<ISceneChangeEventHandler>())
        property.OnSceneChanged(beforeSceneName, afterSceneName);
    }
    
    public void HandleBeforeSceneChanged()
    {
      var p = typeof(GameManager).GetProperties(BindingFlags.Public | BindingFlags.Instance |
                                                BindingFlags.Static | BindingFlags.DeclaredOnly);

      foreach (var property in p.Select(x => x.GetValue(GameManager.Manager)).OfType<IBeforeSceneChangeEventHandler>())
        property.OnBeforeSceneChange(beforeSceneName, afterSceneName);
    }
  }
}
