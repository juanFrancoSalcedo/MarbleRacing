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
        if(image) spriteOriginal = image.sprite;
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
                    SetLoops(listAux[currentAnimation].loops).SetUpdate(!useTimeScale); ;

                rectTransform.DOScale(listAux[currentAnimation].targetScale, listAux[currentAnimation].timeAnimation).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).
                    SetLoops(listAux[currentAnimation].loops).OnComplete(CallBacks).SetUpdate(!useTimeScale);

                break;

            case TypeAnimation.SwitchSprite:
                if (ReferenceEquals(image.sprite, spriteOriginal))
                {
                    image.DOColor(image.color, listAux[currentAnimation].timeAnimation).SetDelay(listAux[currentAnimation].delay).
                    OnComplete(delegate { image.sprite = listAux[currentAnimation].spriteShift; CallBacks(); });
                }
                else
                {
                    image.DOColor(image.color, listAux[currentAnimation].timeAnimation).SetDelay(listAux[currentAnimation].delay).
                    OnComplete(delegate { image.sprite = spriteOriginal; CallBacks(); });
                }
                break;

            case TypeAnimation.ChangeSprite:
                image.DOColor(image.color, listAux[currentAnimation].timeAnimation).SetDelay(listAux[currentAnimation].delay).
                    OnComplete( delegate { image.sprite = listAux[currentAnimation].spriteShift; CallBacks();});
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

            case TypeAnimation.Rotate:
                rectTransform.DORotate(listAux[currentAnimation].targetRotation, listAux[currentAnimation].timeAnimation).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).
                    SetLoops(listAux[currentAnimation].loops).OnComplete(CallBacks).SetUpdate(!useTimeScale);
                break;

            case TypeAnimation.RotateScaleAT:

                rectTransform.DORotate(listAux[currentAnimation].targetRotation, listAux[currentAnimation].timeAnimation).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).
                    SetLoops(listAux[currentAnimation].loops).SetUpdate(!useTimeScale); ;

                rectTransform.DOScale(listAux[currentAnimation].targetScale, listAux[currentAnimation].timeAnimation).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).
                    SetLoops(listAux[currentAnimation].loops).OnComplete(CallBacks).SetUpdate(!useTimeScale);

                break;

            case TypeAnimation.MoveLocalScaleAT:

                rectTransform.DOLocalMove(listAux[currentAnimation].targetPosition, listAux[currentAnimation].timeAnimation).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).
                    SetLoops(listAux[currentAnimation].loops).SetUpdate(!useTimeScale); ;

                rectTransform.DOScale(listAux[currentAnimation].targetScale, listAux[currentAnimation].timeAnimation).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).
                    SetLoops(listAux[currentAnimation].loops).OnComplete(CallBacks).SetUpdate(!useTimeScale);

                break;

            case TypeAnimation.MoveScaleAT:

                rectTransform.DOMove(listAux[currentAnimation].targetPosition, listAux[currentAnimation].timeAnimation).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).
                    SetLoops(listAux[currentAnimation].loops).SetUpdate(!useTimeScale); ;

                rectTransform.DOScale(listAux[currentAnimation].targetScale, listAux[currentAnimation].timeAnimation).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).
                    SetLoops(listAux[currentAnimation].loops).OnComplete(CallBacks).SetUpdate(!useTimeScale);

                break;

            case TypeAnimation.MoveLocalFadeInAT:

                image.DOFade(1, listAux[currentAnimation].timeAnimation).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).
                    SetLoops(listAux[currentAnimation].loops).SetUpdate(!useTimeScale); ;

                rectTransform.DOLocalMove(listAux[currentAnimation].targetPosition, listAux[currentAnimation].timeAnimation).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).
                    SetLoops(listAux[currentAnimation].loops).OnComplete(CallBacks).SetUpdate(!useTimeScale);

                break;

            case TypeAnimation.SizeDelta:

                rectTransform.DOSizeDelta(listAux[currentAnimation].targetScale, listAux[currentAnimation].timeAnimation).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).
                    SetLoops(listAux[currentAnimation].loops).SetUpdate(!useTimeScale);

                rectTransform.DOAnchorPos(listAux[currentAnimation].targetPosition, listAux[currentAnimation].timeAnimation).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).
                    SetLoops(listAux[currentAnimation].loops).OnComplete(CallBacks).SetUpdate(!useTimeScale); ;

                break;

            case TypeAnimation.PixelPerUnitMultiplier:
                DOTween.Kill(image.pixelsPerUnitMultiplier);
                DOTween.To(() => this.image.pixelsPerUnitMultiplier, juu => this.image.pixelsPerUnitMultiplier = juu, listAux[currentAnimation].pixelMultiplier,
                    listAux[currentAnimation].timeAnimation).SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).
                    SetLoops(listAux[currentAnimation].loops).SetUpdate(!useTimeScale).OnComplete(CallBacks).OnUpdate(image.SetAllDirty);
                        //.OnComplete(()=> { CallBacks(); StopCoroutine(UpdatePixelPerUnit()); }).SetUpdate(!useTimeScale).
                        //OnStart(()=>StartCoroutine(UpdatePixelPerUnit())).OnUpdate(image.SetAllDirty);
                break;
                // 2090*
        }
    }

    private IEnumerator UpdatePixelPerUnit() 
    {
        while (gameObject.activeInHierarchy)
        {
            //();
            yield return new WaitForEndOfFrame();
        }
    }
}

