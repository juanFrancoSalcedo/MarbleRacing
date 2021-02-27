using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;


#if UNITY_EDITOR
[CustomEditor(typeof(AnimationUIController))]
public class EditorDoUIAnimator : BaseEditorAnimator
{
    //protected new AnimationUIController animationController;
    public override void OnInspectorGUI()
    {
        animationController = (AnimationUIController)target;
        base.OnInspectorGUI();
    }

    protected override void ShowData(AnimationAssistant animationAux)
    {
        animationAux.animationType = (TypeAnimation)EditorGUILayout.EnumPopup("Animation Type", animationAux.animationType);
        ShowTargetPosition(animationAux);
        ShowTargetScale(animationAux);
        ShowTargetSprite(animationAux);
        ShowTargetRotation(animationAux);
        ShowColor(animationAux);
        animationAux.delay = EditorGUILayout.FloatField("Delay Time", animationAux.delay);
        animationAux.timeAnimation = EditorGUILayout.FloatField("Time Animation", animationAux.timeAnimation);
        animationAux.animationCurve = (Ease)EditorGUILayout.EnumPopup("Animation Type", animationAux.animationCurve);
        animationAux.loops = EditorGUILayout.IntField("Loops", animationAux.loops);
        animationAux.playOnAwake = EditorGUILayout.Toggle("Play On Awake", animationAux.playOnAwake);
    }

    protected override void ShowTargetPosition(AnimationAssistant auxArg)
    {
        if (auxArg.animationType == TypeAnimation.Move || auxArg.animationType == TypeAnimation.MoveReturnOrigin || auxArg.animationType == TypeAnimation.MoveLocal)
        {
            auxArg.targetPosition = EditorGUILayout.Vector3Field("Target Position", auxArg.targetPosition);
        }
    }

    protected override void ShowTargetScale(AnimationAssistant auxArg)
    {
        if (auxArg.animationType == TypeAnimation.Scale ||
            auxArg.animationType == TypeAnimation.ScaleReturnOriginScale ||
            auxArg.animationType == TypeAnimation.FadeOutScaleAT ||
            auxArg.animationType == TypeAnimation.FadeInScaleAT)
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
        if (auxArg.animationType == TypeAnimation.ColorChange)
        {
            auxArg.colorTarget = EditorGUILayout.ColorField("Color Target",auxArg.colorTarget);
        }
    }

    private void ShowTargetSprite(AnimationAssistant auxArg)
    {
        if (auxArg.animationType == TypeAnimation.SwitchSprite)
        {
            SerializedProperty objSer;
            objSer = serializedObject.FindProperty("listAux");
            SerializedProperty spriteShift;
            spriteShift = objSer.GetArrayElementAtIndex(animationController.listAux.IndexOf(auxArg));
            EditorGUILayout.PropertyField(spriteShift.FindPropertyRelative("spriteShift"), new GUIContent("SpriteShift"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif
