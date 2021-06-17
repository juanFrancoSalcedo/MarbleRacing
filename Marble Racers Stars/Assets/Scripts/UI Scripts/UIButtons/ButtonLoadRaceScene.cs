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
    [SerializeField] [ConditionalField(nameof(inmediateLoading), false)] private TextMeshProUGUI buttonText = null;
    [SerializeField] [ConditionalField(nameof(inmediateLoading), false)] private LeagueManager leagueCalculated = null;
    string sceneLoadIndex;

    void Start()
    {
        if (inmediateLoading)
        {
            Invoke("SelectScene",1.3f);
            return;
        }
        PrepareScene();
        if(buttonComponent != null)
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
        if (leagueCalculated != null)
        {
            if (leagueCalculated.Liga.date >= leagueCalculated.Liga.listPrix.Count)
            {
                PlayerPrefs.SetInt(KeyStorage.GIFT_CLAIMED_I, 1);
                buttonText.text = "Get Gift";
                sceneLoadIndex = Constants.sceneAward;
            }
            else
            {
                buttonText.text = "Next Race";
                sceneLoadIndex = allCups.NextRace();
            }
        }
        else
        {
            if (buttonText != null)
                buttonText.text = "Next Race";

            if (PlayerPrefs.GetInt(KeyStorage.GIFT_CLAIMED_I, 0) == 1 && inmediateLoading)
                sceneLoadIndex = Constants.sceneAward;
            else
                sceneLoadIndex = allCups.NextRace();
        }
        if (string.IsNullOrEmpty(sceneLoadIndex)) sceneLoadIndex = "(T)Hut On The Hill";
    }

    void ProgressLoad()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneLoadIndex);
        LoadingAnimator.Instance.LoadingSceneWithProgressCurtain(operation);
    }
}
