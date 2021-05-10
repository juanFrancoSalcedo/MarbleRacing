using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using LeagueSYS;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MyBox;
using DG.Tweening;

public class ButtonLoadRaceScene : MonoBehaviour
{
    [SerializeField] bool inmediateLoading;
    [SerializeField] private Cups allCups;
    [SerializeField] [ConditionalField(nameof(inmediateLoading), false)] private TextMeshProUGUI buttonText;
    [SerializeField] [ConditionalField(nameof(inmediateLoading), false)] private LeagueManager leagueCalculated;
    string sceneLoadIndex;

    void Start()
    {
        if (inmediateLoading)
        {
            Invoke("SelectScene",1.3f);
            return;
        }
        PrepareScene();
        if(GetComponent<Button>())
            GetComponent<Button>().onClick.AddListener(SelectScene);
    }
  
    void SelectScene()
    {
        PrepareScene();
        Time.timeScale = 1f;
        StartCoroutine(ProgressLoad());
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

    IEnumerator ProgressLoad()
    {
        LoadingAnimator.Instance.AnimationInit();
        while (!LoadingAnimator.Instance.stepOneAnimation)
            yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneLoadIndex);
        operation.completed += delegate { LoadingAnimator.Instance.AnimationOut(); };
    }
}
