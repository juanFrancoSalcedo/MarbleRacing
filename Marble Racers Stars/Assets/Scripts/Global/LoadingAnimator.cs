using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MyBox;
using System.Threading.Tasks;
using System.Threading;
using System;

public class LoadingAnimator : Singleton<LoadingAnimator>
{

    [SerializeField] Image imageTransition = null;
    [SerializeField] Image logoTransition = null;
    private Sprite defaultSprite;
    public bool stepOneAnimation { get; set; } = false;
    private bool m_levelWasLoaded = false;

    private void Awake()
    {
        defaultSprite = logoTransition.sprite;
        if (ReferenceEquals(Instance, this))
            DontDestroyOnLoad(this);
        else
            Destroy(gameObject);
        //print("curtain");
    }

    public async void LoadingSceneWithProgressCurtain(AsyncOperation operation, Sprite logoTrack)
    {
        await ChangeLogo(logoTrack);
        await AnimationInit();
        m_levelWasLoaded = false;
        while (!m_levelWasLoaded)
        {
            if(operation.progress>=0.8f)
                break;
        }
        await Task.Delay(200);
        Task task = (MarbleSelector.Instance != null)? MarbleSelector.Instance.InstanciateAllItems():TestProgressAlternative();
        await Task.WhenAll(task);
        AnimationOut();
    }
    //public async void LoadingSceneWithProgressCurtain(AsyncOperation operation)
    //{
    //    await AnimationInit();
    //    m_levelWasLoaded = false;
    //    while (!m_levelWasLoaded)
    //    {
    //        if (operation.progress >= 0.8f)
    //            break;
    //    }
    //    await Task.Delay(200);
    //    Task task = (MarbleSelector.Instance != null) ? MarbleSelector.Instance.InstanciateAllItems() : TestProgressAlternative();
    //    await Task.WhenAll(task);
    //    AnimationOut();
    //}
    private async Task ChangeLogo(Sprite logo) => logoTransition.sprite  = await Task.FromResult<Sprite>((logo != null)?logo:defaultSprite);

    private async Task<bool> TestProgressAlternative()
    {
        await Task.Delay(200);
        return true;
    }


    public async Task<bool> AnimationInit()
    {
        imageTransition.rectTransform.DOAnchorPos(Vector3.zero, 0.5f).
            OnComplete(delegate {stepOneAnimation = true;}).SetUpdate(true);
        await Task.Delay(TimeSpan.FromSeconds(0.6f));
        return true;
    }

    public void AnimationInit(float delayNextAnimation)
    {
        imageTransition.rectTransform.DOAnchorPos(Vector3.zero, 0.5f).
            OnComplete(delegate { stepOneAnimation = true; Invoke("AnimationOut",delayNextAnimation); })
            .SetUpdate(true);
    }

    public void AnimationOut()
    {
        imageTransition.rectTransform.DOAnchorPos(new Vector3(3000, 0, 0), 0.4f).SetDelay(0.6f).SetUpdate(true).
            OnComplete(delegate { imageTransition.rectTransform.anchoredPosition = new Vector3(-3000, 0, 0); });
    }

    private void OnLevelWasLoaded(int level)
    {
        m_levelWasLoaded = true;
    }
}
