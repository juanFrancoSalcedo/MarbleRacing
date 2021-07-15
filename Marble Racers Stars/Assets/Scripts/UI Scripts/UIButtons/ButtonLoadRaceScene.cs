using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using LeagueSYS;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MyBox;
using DG.Tweening;

public class ButtonLoadRaceScene : BaseButtonComponent
{
    [SerializeField] bool inmediateLoading = false;
    [SerializeField] private Cups allCups = null;
    [SerializeField] [ConditionalField(nameof(inmediateLoading), true)] private TextMeshProUGUI buttonText = null;
    string sceneLoadIndex;
    TracksInfo infoLastTrack = null;

    void Start()
    {
        if (inmediateLoading)
        {
            LeagueManager.CreateLeague(allCups,DataController.Instance.allMarbles);
            Invoke("SelectScene",1.3f);
            return;
        }
        PrepareScene();
        if (buttonComponent != null)
            buttonComponent.onClick.AddListener(SelectScene);
    }

    void SelectScene()
    {
        PrepareScene();
        Time.timeScale = 1f;
        ProgressLoad();
    }

    void PrepareScene()
    {
        //print(PlayerPrefs.GetInt(KeyStorage.GIFT_CLAIMED_I));
        if (SceneManager.GetActiveScene().name.Contains("(T)"))
        {
            if (LeagueManager.LeagueRunning.date >= LeagueManager.LeagueRunning.listPrix.Count)
            {
                PlayerPrefs.SetInt(KeyStorage.GIFT_CLAIMED_I, 1);
                buttonText.text = "Get Award";
                sceneLoadIndex = Constants.sceneAward;
                infoLastTrack = null;
            }
            else
            {
                buttonText.text = "Next Race";
                infoLastTrack = allCups.NextRace();
                sceneLoadIndex = infoLastTrack.NameTrack;
            }
        }
        else
        {
            if (buttonText != null)
                buttonText.text = "Next Race";
            if (PlayerPrefs.GetInt(KeyStorage.GIFT_CLAIMED_I, 0) == 1 && inmediateLoading)
            { 
                sceneLoadIndex = Constants.sceneAward;
                infoLastTrack = null;
            }
            else
            {
                infoLastTrack = allCups.NextRace();
                sceneLoadIndex = infoLastTrack.NameTrack;
            }
        }
        if (string.IsNullOrEmpty(sceneLoadIndex)) 
        {
            infoLastTrack = allCups.NextRace();
            sceneLoadIndex = infoLastTrack.NameTrack;
        }
    }


    void ProgressLoad()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneLoadIndex);
        if (infoLastTrack == null)
            infoLastTrack = allCups.NextRace();
        LoadingAnimator.Instance.LoadingSceneWithProgressCurtain(operation,infoLastTrack.LogoTrack);
    }
}
