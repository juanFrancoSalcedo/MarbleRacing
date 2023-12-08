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
    [SerializeField] [ConditionalField(nameof(inmediateLoading), true)] private TextMeshProUGUI buttonText = null;
    string sceneToLoad;
    TracksInfo infoLastTrack = null;

    void Start()
    {
        if (inmediateLoading)
        {
            if (LeagueManager.IsNullLeagueData())
            { 
                //LeagueManager.CreateLeague(DataController.Instance.allCups, DataController.Instance.allMarbles);
            }
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
                sceneToLoad = Constants.sceneAward;
                infoLastTrack = null;
            }
            else
            {
                buttonText.text = "Next Race";
                infoLastTrack = DataController.Instance.allCups.NextRace();
                sceneToLoad = infoLastTrack.NameTrack;
            }
        }
        else
        {
            if (buttonText != null)
                buttonText.text = "Next Race";
            if (PlayerPrefs.GetInt(KeyStorage.GIFT_CLAIMED_I, 0) == 1 && inmediateLoading)
            { 
                sceneToLoad = Constants.sceneAward;
                infoLastTrack = null;
            }
            else
            {
                infoLastTrack = DataController.Instance.allCups.NextRace();
                print("king("+ infoLastTrack+")");
                sceneToLoad = infoLastTrack.NameTrack;
            }
        }
        if (string.IsNullOrEmpty(sceneToLoad)) 
        {
            print("Polemica");
            infoLastTrack = DataController.Instance.allCups.NextRace();
            sceneToLoad = infoLastTrack.NameTrack;
        }
    }


    void ProgressLoad()
    {
        print(sceneToLoad);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        if (infoLastTrack == null)
            infoLastTrack = DataController.Instance.allCups.NextRace();
        LoadingAnimator.Instance.LoadingSceneWithProgressCurtain(operation,infoLastTrack.LogoTrack);
    }
}
