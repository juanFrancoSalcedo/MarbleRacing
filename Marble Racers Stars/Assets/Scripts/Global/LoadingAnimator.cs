using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingAnimator : MonoBehaviour
{

    [SerializeField] Image imageTransition;
    private static LoadingAnimator _instance;
    public bool finishedProcess { get; set; }
    public bool stepOneAnimation { get; set; }
    public static LoadingAnimator Instance 
    {
        get 
        {
            if (_instance == null)
            {
                _instance = Object.FindObjectOfType<LoadingAnimator>();
                if (_instance == null)
                    Debug.LogError("Singleton Instance caused:  not found on scene");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (ReferenceEquals(Instance, this))
            DontDestroyOnLoad(this);
        else
            Destroy(gameObject);
    }

    public void AnimationInit()
    {
        finishedProcess = false;
        imageTransition.rectTransform.DOAnchorPos(Vector3.zero, 0.5f).
            OnComplete(delegate {stepOneAnimation = true; }).SetUpdate(true);
    }

    public void AnimationInit(float delayNextAnimation)
    {
        finishedProcess = false;
        imageTransition.rectTransform.DOAnchorPos(Vector3.zero, 0.5f).
            OnComplete(delegate { stepOneAnimation = true; Invoke("AnimationOut",delayNextAnimation); })
            .SetUpdate(true);
    }

    public void AnimationOut()
    {
        imageTransition.rectTransform.DOAnchorPos(new Vector3(3000, 0, 0), 0.4f).SetDelay(0.6f).SetUpdate(true).
            OnComplete(delegate { imageTransition.rectTransform.anchoredPosition = new Vector3(-3000, 0, 0); });
    }
}
