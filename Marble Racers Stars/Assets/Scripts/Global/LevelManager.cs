using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class LevelManager : MonoBehaviour
{
    [SerializeField] AudioMixer audiomixer = null;
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
        while (!LoadingAnimator.Instance.stepOneAnimation)
            yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneLoadIndex);
        LoadingAnimator.Instance.LoadingSceneWithProgressCurtain(operation);

    }

    protected IEnumerator ProgressLoad(string sceneLoad)
    {
        while (!LoadingAnimator.Instance.stepOneAnimation)
            yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneLoad);
        LoadingAnimator.Instance.LoadingSceneWithProgressCurtain(operation);
    }

    public void Pause(Canvas canvasPause)
    {
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
        canvasPause.enabled = canvasPause.enabled ? false : true; 
        if(PlayerPrefs.GetInt(KeyStorage.SOUND_SETTING_I,0) ==1)
            audiomixer.SetFloat("Volume",Time.timeScale==0?-80f:0);
    }

    public void Pause()
    {
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
        if(PlayerPrefs.GetInt(KeyStorage.SOUND_SETTING_I,0) ==1)
            audiomixer.SetFloat("Volume",Time.timeScale==0?-80f:0);
    }
}
