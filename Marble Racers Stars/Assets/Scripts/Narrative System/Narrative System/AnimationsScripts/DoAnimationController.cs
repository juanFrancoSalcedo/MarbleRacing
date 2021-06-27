using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public abstract class DoAnimationController : MonoBehaviour
{
    public Vector3 originPosition { get; set; }
    public Vector3 originScale { get; set; }
    public Vector3 originRotation { get; set; }
    protected int currentAnimation = 0;
    public int CurrentAnimation { get { return currentAnimation; } private set { } }
    [HideInInspector]
    public List<AnimationAssistant> listAux = new List<AnimationAssistant>();
    [Header("~~~~Events~~~~~")]
    [SerializeField] protected bool useTimeScale = true;
    [SerializeField] private bool inLoop = false;
    [SerializeField] private bool restoreOnEnd = false;
    [SerializeField] protected bool restoreOnDisable = false;
    [SerializeField] protected bool rewindOnDisable = false;
    public UnityEvent OnStartedCallBack;
    public UnityEvent OnEndedCallBack;
    public System.Action OnCompleted;

    public abstract void ActiveAnimation();
    public void ActiveAnimation(int newIndex)
    {
        currentAnimation = newIndex;
        ActiveAnimation();
    }

    public void RewindAndActiveAnimation()
    {
        Rewind();
        ActiveAnimation();
    }

    protected void OnEnable()
    {
        if (listAux.Count == 0) listAux.Add(new AnimationAssistant());

        if (listAux[currentAnimation].playOnAwake && currentAnimation == 0) ActiveAnimation();
    }

    protected void OnDisable()
    {
        if (rewindOnDisable)
        {
            currentAnimation = 0;
            transform.DOKill();
        }

        if (restoreOnEnd)
        {
            RestorePosition();
            RestoreScale();
            RestoreRotation();
        }
    }

    public void Rewind()
    {
        currentAnimation = 0;
        transform.DOKill();
    }

    public void StopAnimations() => transform.DOKill();

    public List<AnimationAssistant> GetList()
    {
        return listAux;
    }

    protected void PlusAnimationIndex()
    {
        currentAnimation++;

        if (currentAnimation == listAux.Count)
        {
            currentAnimation = 0;
            OnCompleted?.Invoke();
            OnEndedCallBack?.Invoke();
            if (restoreOnEnd)
            {
                RestorePosition();
                RestoreScale();
                RestoreRotation();
            }

            if (inLoop)
            {
                ActiveAnimation();
            }
        }
        else
        {
            if (listAux[currentAnimation].playOnAwake) ActiveAnimation();
        }
    }

    protected void CallBacks()
    {
        PlusAnimationIndex();
    }

    protected void RestoreScale()
    {
        transform.localScale = originScale;
    }

    protected virtual void RestorePosition()
    {
        transform.position = originPosition;
    }
    protected virtual void RestoreRotation()
    {
        transform.rotation = Quaternion.Euler(originRotation);
    }

    public void SetInloop(bool arg1)
    {
        inLoop = arg1;
    }
}

[System.Serializable]
public class AnimationAssistant
{
    public TypeAnimation animationType;
    public Vector3 targetPosition;
    public Vector3 targetScale;
    public Vector3 targetRotation;
    public float timeAnimation = 0.3F;
    public float delay;
    public float coldTime;
    public Ease animationCurve;
    public bool playOnAwake;
    public Transform worldPoint;
    public Color colorTarget;
    public bool display;
    public int loops;
    public Sprite spriteShift;
    public float pixelMultiplier;
    public void DisplayAnimationAux() => display = !display;
}

public enum TypeAnimation
{
    Move,
    MoveLocal,
    MoveReturnOrigin,
    MoveScaleAT,
    MoveWorldPoint,
    MoveWorldPointScale,
    Scale,
    ScaleReturnOriginScale,
    FadeOut,
    FadeIn,
    FadeInScaleAT,
    FadeOutScaleAT,
    SwitchSprite,
    ChangeSprite,
    ColorChange,
    Rotate,
    RotateBackOrigin,
    UIMoveToPoint,
    UIMoveScaleToPoint,
    MoveLocalScaleAT,
    RotateScaleAT,
    MoveLocalFadeInAT,
    SizeDelta,
    PixelPerUnitMultiplier
}