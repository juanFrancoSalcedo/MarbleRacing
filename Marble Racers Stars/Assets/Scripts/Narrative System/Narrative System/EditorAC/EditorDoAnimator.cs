using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;

#if UNITY_EDITOR

[CustomEditor(typeof(AnimationController))]
public class EditorDoAnimator : BaseEditorAnimator
{
    //protected new AnimationController animationController;

    public override void OnInspectorGUI()
    {
        animationController = (AnimationController)target;
        base.OnInspectorGUI();
    }

    protected override void ShowData(AnimationAssistant animationAux)
    {
        animationAux.animationType = (TypeAnimation)EditorGUILayout.EnumPopup("Animation Type", animationAux.animationType);
        ShowTargetPosition(animationAux);
        ShowTargetScale(animationAux);
        ShowTargetRotation(animationAux);
        animationAux.delay = EditorGUILayout.FloatField("Delay Time", animationAux.delay);
        animationAux.timeAnimation = EditorGUILayout.FloatField("Time Animation", animationAux.timeAnimation);
        animationAux.animationCurve = (Ease)EditorGUILayout.EnumPopup("Animation Type", animationAux.animationCurve);
        animationAux.loops = EditorGUILayout.IntField("Loops", animationAux.loops);
        animationAux.playOnAwake = EditorGUILayout.Toggle("Play On Awake", animationAux.playOnAwake);
    }

    protected override void ShowTargetPosition(AnimationAssistant auxArg)
    {
        if (auxArg.animationType == TypeAnimation.Move || auxArg.animationType == TypeAnimation.MoveReturnOrigin || auxArg.animationType == TypeAnimation.MoveLocal
            || auxArg.animationType == TypeAnimation.MoveLocalScaleAT)
        {
            auxArg.targetPosition = EditorGUILayout.Vector3Field("Target Position", auxArg.targetPosition);
        }
    }

    protected override void ShowTargetScale(AnimationAssistant auxArg)
    {
        if (auxArg.animationType == TypeAnimation.Scale || auxArg.animationType == TypeAnimation.ScaleReturnOriginScale
            || auxArg.animationType == TypeAnimation.MoveLocalScaleAT)
        {
            auxArg.targetScale = EditorGUILayout.Vector3Field("Target Scale", auxArg.targetScale);
        }
    }

    protected override void ShowTargetRotation(AnimationAssistant auxArg)
    {
        if (auxArg.animationType == TypeAnimation.Rotate || auxArg.animationType == TypeAnimation.RotateBackOrigin)
        {
            auxArg.targetRotation = EditorGUILayout.Vector3Field("Target Rotation", auxArg.targetRotation);
        }
    }

    protected override void ShowColor(AnimationAssistant auxArg)
    {
        if (auxArg.animationType == TypeAnimation.Rotate || auxArg.animationType == TypeAnimation.RotateBackOrigin)
        {
            auxArg.targetRotation = EditorGUILayout.Vector3Field("Target Rotation", auxArg.targetRotation);
        }
    }
}

#endif
