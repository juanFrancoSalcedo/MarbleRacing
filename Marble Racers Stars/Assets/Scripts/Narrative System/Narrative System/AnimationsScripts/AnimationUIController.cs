using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class AnimationUIController : DoAnimationController
{
    private RectTransform rectTransform;
    private Image image;
    private Sprite spriteOriginal;
    
    private new void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        originPosition = rectTransform.anchoredPosition;
        originScale = rectTransform.localScale;
        if (image) spriteOriginal = image.sprite;
        base.OnEnable();
    }

    protected new void OnDisable()
    {
        base.OnDisable();
        rectTransform.DOKill();
        image.DOKill();
    }

    protected override void RestorePosition()
    {
        rectTransform.anchoredPosition = originPosition;
    }

    public override void ActiveAnimation()
    {
        if (currentAnimation == 0)
        {
            OnStartedCallBack?.Invoke();
        }

        Sequence sequence = DOTween.Sequence();
        
        switch (listAux[currentAnimation].animationType)
        {
            case TypeAnimation.Move:
                rectTransform.DOAnchorPos(listAux[currentAnimation].targetPosition, listAux[currentAnimation].timeAnimation, false).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).
                    SetLoops(listAux[currentAnimation].loops).OnComplete(CallBacks).SetUpdate(!useTimeScale);
                break;

            case TypeAnimation.Scale:
                sequence.Append(rectTransform.DOScale(listAux[currentAnimation].targetScale, listAux[currentAnimation].timeAnimation).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay)).
                    SetLoops(listAux[currentAnimation].loops).OnComplete(CallBacks).SetUpdate(!useTimeScale);
                break;

            case TypeAnimation.FadeOutScaleAT:

                image.DOFade(0, listAux[currentAnimation].timeAnimation).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).
                    SetLoops(listAux[currentAnimation].loops).SetUpdate(!useTimeScale);

                rectTransform.DOScale(listAux[currentAnimation].targetScale, listAux[currentAnimation].timeAnimation).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).
                    SetLoops(listAux[currentAnimation].loops).OnComplete(CallBacks).SetUpdate(!useTimeScale);

                break;

            case TypeAnimation.FadeInScaleAT:

                image.DOFade(1, listAux[currentAnimation].timeAnimation).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).
                    SetLoops(listAux[currentAnimation].loops).SetUpdate(!useTimeScale);

                rectTransform.DOScale(listAux[currentAnimation].targetScale, listAux[currentAnimation].timeAnimation).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).
                    SetLoops(listAux[currentAnimation].loops).OnComplete(CallBacks).SetUpdate(!useTimeScale);

                break;

            case TypeAnimation.SwitchSprite:
                if (ReferenceEquals(image.sprite,spriteOriginal))
                image.sprite = listAux[currentAnimation].spriteShift;
                else
                image.sprite = spriteOriginal;
                CallBacks();
                break;

            case TypeAnimation.FadeIn:
                image.DOFade(1, listAux[currentAnimation].timeAnimation).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).
                    SetLoops(listAux[currentAnimation].loops).OnComplete(CallBacks).SetUpdate(!useTimeScale);
                break;

            case TypeAnimation.FadeOut:
                image.DOFade(0, listAux[currentAnimation].timeAnimation).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).
                    SetLoops(listAux[currentAnimation].loops).OnComplete(CallBacks).SetUpdate(!useTimeScale);
                break;

            case TypeAnimation.ColorChange:
                image.DOColor(listAux[currentAnimation].colorTarget,listAux[currentAnimation].timeAnimation).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).
                    SetLoops(listAux[currentAnimation].loops).OnComplete(CallBacks).SetUpdate(!useTimeScale);
                break;
        }
    }
    
}

