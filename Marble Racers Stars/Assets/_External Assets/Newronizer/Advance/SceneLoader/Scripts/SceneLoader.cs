using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Newronizer.SceneLoader
{
    public class SceneLoader : Singleton<SceneLoader>
    {
        [SerializeField] private UnityEvent callBackStartLoad;
        [SerializeField] private UnityEvent callBackLoaded;
        [SerializeField] GameObject panelLoading;
        PauseHandler pauseHandler = null;
        protected override void Awake()
        {
            base.Awake();
            pauseHandler = new PauseHandler();
        }

        public int CurrentSceneIndex => SceneManager.GetActiveScene().buildIndex;
        public string CurrentSceneName => SceneManager.GetActiveScene().name;
        public void LoadFirstScene() => LoadScene(0);
        public void LoadNextScene() => LoadScene(CurrentSceneIndex + 1);
        public void LoadPreviousScene() => LoadScene(CurrentSceneIndex - 1);
        public void ReloadScene() => LoadScene(CurrentSceneIndex);
        public void LoadScene(int sceneIndex) => StartCoroutine(CallLoadScene(sceneIndex));
        public void LoadScene(string sceneName) => StartCoroutine(CallLoadScene(sceneName));

        #region Coroutines

        IEnumerator CallLoadScene(int sceneIndex)
        {
            var scenePath = SceneUtility.GetScenePathByBuildIndex(sceneIndex);
            yield return CallLoadScene(GetSceneNameByPath(scenePath));
        }

        IEnumerator CallLoadScene(string sceneName)
        {
            yield return LoadingProcess(sceneName);
        }

        IEnumerator LoadingProcess(string sceneName)
        {
            callBackStartLoad?.Invoke();
            panelLoading.SetActive(true);
            AsyncOperation progress = SceneManager.LoadSceneAsync(sceneName);
            if (progress.progress<1f)
            {
                panelLoading.SetActive(true);
                yield return null;
            }
            panelLoading.SetActive(false);
            callBackLoaded?.Invoke();
        }

        public void PauseTime() => Time.timeScale = Time.timeScale == 1 ? 0 : 1;
        public void SetPauseTime(float newTime) => Time.timeScale = newTime;
        #endregion

        #region Static Methods
        public static string GetSceneNameByPath(string path)
        {
            var pathRoutes = path.Split('/');
            var sceneNameFormat = pathRoutes[pathRoutes.Length - 1];
            var format = ".unity";
            return sceneNameFormat.Remove(sceneNameFormat.Length - format.Length);
        }

        public static string GetCurrentSceneName() => SceneManager.GetActiveScene().name;
        #endregion

        public void Quit() => Application.Quit();
        public void Pause(bool pause)=> pauseHandler.Pause(pause);
        public void Pause(float time) => pauseHandler.Pause(time);
        public void Pause() => pauseHandler.Pause();
    }



}