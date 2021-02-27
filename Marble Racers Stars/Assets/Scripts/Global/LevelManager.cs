using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void Replay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadNewMarbleLevel()
    {
        StartCoroutine(ProgressLoad(Constants.sceneNewMarble));
    }

    public void LoadAwardLevel()
    {
        StartCoroutine(ProgressLoad(Constants.sceneAward));
    }

    public void LoadCupSelectionLevel()
    {
        StartCoroutine(ProgressLoad(Constants.sceneCups));
    }

    public void LoadSpecificScene(int indexScene)
    {
        StartCoroutine(ProgressLoad(indexScene));
    }

    protected IEnumerator ProgressLoad(int sceneLoadIndex) 
    {
        LoadingAnimator.Instance.AnimationInit();
        while (!LoadingAnimator.Instance.stepOneAnimation)
            yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneLoadIndex);
        operation.completed += delegate { LoadingAnimator.Instance.AnimationOut(); };
    }

    protected IEnumerator ProgressLoad(string sceneLoad)
    {
        LoadingAnimator.Instance.AnimationInit();
        while (!LoadingAnimator.Instance.stepOneAnimation)
            yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneLoad);
        operation.completed += delegate { LoadingAnimator.Instance.AnimationOut(); };
    }

    public void Pause(Canvas canvasPause)
    {
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
        canvasPause.enabled = (canvasPause.enabled) ? false : true; 
    }

    public void Pause()
    {
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
    }
}
