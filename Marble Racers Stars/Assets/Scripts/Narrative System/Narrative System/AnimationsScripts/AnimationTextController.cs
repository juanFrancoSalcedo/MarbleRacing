using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;

public class AnimationTextController : DoAnimationController
{
    private RectTransform rectTransform;
    public TextMeshProUGUI textComponent { get; set; }
    private string textNarrativeBuffer;

    private new void OnEnable()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
        originPosition = rectTransform.anchoredPosition;
        originScale = rectTransform.localScale;
        base.OnEnable();
    }

    protected new void OnDisable()
    {
        base.OnDisable();
        rectTransform.DOKill();
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
                rectTransform.DOMove(listAux[currentAnimation].targetPosition, listAux[currentAnimation].timeAnimation, false).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).
                    SetLoops(listAux[currentAnimation].loops).OnComplete(CallBacks);
                break;

            case TypeAnimation.MoveLocal:
                rectTransform.DOLocalMove(listAux[currentAnimation].targetPosition, listAux[currentAnimation].timeAnimation, false).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).
                    SetLoops(listAux[currentAnimation].loops).OnComplete(CallBacks);
                break;

            case TypeAnimation.Scale:
                rectTransform.DOScale(listAux[currentAnimation].targetScale, listAux[currentAnimation].timeAnimation).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).
                    SetLoops(listAux[currentAnimation].loops).OnComplete(CallBacks);
                break;

            case TypeAnimation.FadeOut:
                textComponent.DOFade(1, 0);
                textComponent.DOFade(0, listAux[currentAnimation].timeAnimation).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).
                    SetLoops(listAux[currentAnimation].loops).OnComplete(CallBacks);
                break;
        }
    }
    
}