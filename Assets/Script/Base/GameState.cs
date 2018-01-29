using UnityEngine;
using System.Collections;
using System;

public class GameState : Singleton<GameState>
{
    public void OnUpdate()
    {
        WindowManager.instance.OnUpdate();
        ResourceCenter.instance.OnUpdate();
    }

    public void OnInit()
    {
        Main.m_mono.StartCoroutine(LoadSceneAsync(Setup.UISceneName, delegate()
        {
            Logger.Debug("Scene was Loaded");
            Global.instance.Init();
            WindowFactory.instance.CreateWindow(WindowType.MainWin, delegate
            {
                EventManager.instance.NotifyUIEvent(UIEventType.TypeOpenMainView, "this is Event param");
            });
        }));
    }

    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    IEnumerator LoadSceneAsync(string sceneName, EventManager.voidHandle OnLoad = null)
    {
        AsyncOperation asyncOp = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        yield return asyncOp;
        if (OnLoad != null)
        {
            OnLoad();
        }
    }
    public void PreLoadWindow()
    {
        WindowFactory.instance.CreateWindow(WindowType.MessageWindow, null, false);
        WindowFactory.instance.CreateWindow(WindowType.FloatMessageWindow, null, false);
    }
}
