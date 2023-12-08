using UnityEngine;

namespace Newronizer.SceneLoader
{
    public class PauseHandler
    {
        public static event System.Action<bool> OnPaused;

        public void Pause(bool pause)
        {
            Time.timeScale = (pause) ? 0 : 1;
            OnPaused?.Invoke(IsPaused);
        }

        public void Pause(float time)
        {
            Time.timeScale = time;
            OnPaused?.Invoke(IsPaused);
        }

        public void Pause()
        {
            Time.timeScale = IsPaused ?1:0;
            OnPaused?.Invoke(IsPaused);
        }

        public bool IsPaused => Time.time == 0;
    }
}