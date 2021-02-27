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
            //sceneLoadIndex = _scenes.NextRace();
            Invoke("SelectScene",1.3f);
            return;
        }
        PrepareScene();
        GetComponent<Button>().onClick.AddListener(SelectScene);
    }
    
    void PrepareScene()
    {
        if (leagueCalculated != null)
        {
            if (leagueCalculated.liga.date >= leagueCalculated.liga.listPrix.Count)
            {
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
            sceneLoadIndex = allCups.NextRace();
        }

        if(string.IsNullOrEmpty(sceneLoadIndex)) sceneLoadIndex = "(T)Hut On The Hill";
    }
    
    void SelectScene()
    {
        PrepareScene();
        Time.timeScale = 1f;
        //print(sceneLoadIndex);
        StartCoroutine(ProgressLoad());
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
