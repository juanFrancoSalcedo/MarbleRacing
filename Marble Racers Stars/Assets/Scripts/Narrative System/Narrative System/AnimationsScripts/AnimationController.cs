using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class AnimationController : DoAnimationController
{
    private SpriteRenderer _spriteRender;

    private new void OnEnable()
    {
        base.OnEnable();

        if (GetComponent<SpriteRenderer>())
        {
            _spriteRender = GetComponent<SpriteRenderer>();
        }

        originPosition = transform.position;
        originScale = transform.localScale;
    }

    protected new void OnDisable()
    {
        base.OnDisable();
        _spriteRender.DOKill();
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
                transform.DOMove(listAux[currentAnimation].targetPosition, listAux[currentAnimation].timeAnimation, false).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).OnComplete(CallBacks).
                    SetLoops(listAux[currentAnimation].loops);
                break;

            case TypeAnimation.Scale:
                transform.DOScale(listAux[currentAnimation].targetScale, listAux[currentAnimation].timeAnimation).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).OnComplete(CallBacks).
                    SetLoops(listAux[currentAnimation].loops);
                break;

            case TypeAnimation.MoveLocal:
                transform.DOLocalMove(listAux[currentAnimation].targetPosition, listAux[currentAnimation].timeAnimation).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).OnComplete(CallBacks).
                    SetLoops(listAux[currentAnimation].loops);
                break;

            case TypeAnimation.MoveLocalScaleAT:

                transform.DOScale(listAux[currentAnimation].targetScale, listAux[currentAnimation].timeAnimation).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).
                    SetLoops(listAux[currentAnimation].loops);

                transform.DOLocalMove(listAux[currentAnimation].targetPosition, listAux[currentAnimation].timeAnimation).
                    SetEase(listAux[currentAnimation].animationCurve).SetDelay(listAux[currentAnimation].delay).OnComplete(CallBacks).
                    SetLoops(listAux[currentAnimation].loops);
                break;
        }
    }
}
