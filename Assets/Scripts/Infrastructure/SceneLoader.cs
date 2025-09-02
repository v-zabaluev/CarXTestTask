using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    private ICoroutineRunner _coroutineRunner;

    public SceneLoader(ICoroutineRunner coroutineRunner)
    {
        _coroutineRunner = coroutineRunner;
    }

    public void Load(string sceneName, Action onSceneLoaded = null)
    {
        _coroutineRunner.StartCoroutine(LoadScene(sceneName, onSceneLoaded));
    }

    private IEnumerator LoadScene(string nextSceneName, Action onSceneLoaded = null)
    {
        if (SceneManager.GetActiveScene().name == nextSceneName)
        {
            onSceneLoaded?.Invoke();

            yield break;
        }

        AsyncOperation loadSceneProcess = SceneManager.LoadSceneAsync(nextSceneName);

        while (!loadSceneProcess.isDone)
        {
            yield return null;
        }

        onSceneLoaded?.Invoke();
    }
}