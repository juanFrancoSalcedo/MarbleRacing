using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;


#if UNITY_EDITOR

[CustomEditor(typeof(CinematicAssistant))]
public class EditorNarrativeAssistant : Editor
{
    CinematicAssistant assitantController;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        assitantController = (CinematicAssistant)target;
        if (assitantController.control == null)
        {
            EditorGUILayout.HelpBox("Assigns the controller "+assitantController.transform.parent.name, MessageType.Warning);
        }
        SelectInfoToShow();
        GUI.color = Color.yellow;
        if (GUILayout.Button("Back To Control"))
        {
            assitantController.BackController();
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(assitantController);
            //EditorSceneManager.MarkSceneDirty(animationController.gameObject.scene);
        }
    }

    private void SelectInfoToShow()
    {
        switch (assitantController.missionType)
        {
            case TypeMisionTutorial.animationCompleted:

                SerializedProperty doAnimator;
                doAnimator = serializedObject.FindProperty("animatorControl");
                EditorGUILayout.PropertyField(doAnimator, new GUIContent("Animator Controller"));
                serializedObject.ApplyModifiedProperties();

                if (GUILayout.Button("[Add Animation]"))
                {
                    assitantController.animationsMission.Add(new AnimationAssistant());
                }
                ShowAnimationList();
                break;

            case TypeMisionTutorial.buttonClicked:
                SerializedProperty buttonUI;
                buttonUI = serializedObject.FindProperty("buttonMision");
                EditorGUILayout.PropertyField(buttonUI,new GUIContent("Button Mission"));
                serializedObject.ApplyModifiedProperties();
                break;

            case TypeMisionTutorial.textCompleted:
                assitantController.textToShow = EditorGUILayout.TextArea(assitantController.textToShow,GUILayout.MinHeight(40));
                SerializedProperty textAnimator;
                textAnimator = serializedObject.FindProperty("typingObject");
                EditorGUILayout.PropertyField(textAnimator, new GUIContent("Typing Animator"));
                serializedObject.ApplyModifiedProperties();
                break;

            case TypeMisionTutorial.textEquals:
                
                break;

            case TypeMisionTutorial.dialogue:
                assitantController.textToShow = EditorGUILayout.TextArea(assitantController.textToShow, GUILayout.MinHeight(40));
                assitantController.characterSide = (SideCharacter)EditorGUILayout.EnumPopup("Side Character", assitantController.characterSide);
                assitantController.languageDialogue = (LanguageCharacter)EditorGUILayout.EnumPopup("Language Fantas", assitantController.languageDialogue);
                assitantController.characterApeear = EditorGUILayout.Toggle("Character Appears", assitantController.characterApeear);

                SerializedProperty scriptable;
                scriptable = serializedObject.FindProperty("characterStats");
                EditorGUILayout.PropertyField(scriptable, new GUIContent("Character Settings"));
                serializedObject.ApplyModifiedProperties();

                break;

            //case TypeMisionTutorial.chapterReader:

            //    SerializedProperty chapterRead;
            //    chapterRead = serializedObject.FindProperty("chapterReader");
            //    EditorGUILayout.PropertyField(chapterRead, new GUIContent("Chapter Checker"));
            //    serializedObject.ApplyModifiedProperties();
            //    break;
        }
    }

    private void ShowAnimationList()
    {
        foreach (AnimationAssistant aux in assitantController.animationsMission)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("" + assitantController.animationsMission.IndexOf(aux) + "[" + aux.animationType.ToString() + "]"))
            {
                aux.DisplayAnimationAux();
            }

            if (GUILayout.Button("<", GUILayout.Width(30)))
            {
                Organizer<AnimationAssistant>.MoveIndexOfAList(aux, assitantController.animationsMission, true);
            }

            if (GUILayout.Button(">", GUILayout.Width(30)))
            {
                Organizer<AnimationAssistant>.MoveIndexOfAList(aux, assitantController.animationsMission, true);
            }
            GUILayout.EndHorizontal();

            if (aux.display)
            {
                ShowDataAnimation(aux);
            }
        }
    }
    
    private void ShowDataAnimation(AnimationAssistant animationAux)
    {
        animationAux.animationType = (TypeAnimation)EditorGUILayout.EnumPopup("Animation Type", animationAux.animationType);
        ShowTargetPosition(animationAux);
        ShowTargetScale(animationAux);
        animationAux.delay = EditorGUILayout.FloatField("Delay Time", animationAux.delay);
        animationAux.timeAnimation = EditorGUILayout.FloatField("Time Animation", animationAux.timeAnimation);
        animationAux.animationCurve = (Ease)EditorGUILayout.EnumPopup("Animation Type", animationAux.animationCurve);
        animationAux.loops = EditorGUILayout.IntField("Loops", animationAux.loops);
        animationAux.playOnAwake = EditorGUILayout.Toggle("Play On Awake", animationAux.playOnAwake);
    }

    private  void ShowTargetPosition(AnimationAssistant auxArg)
    {
        if (auxArg.animationType == TypeAnimation.Move || auxArg.animationType == TypeAnimation.MoveReturnOrigin || auxArg.animationType == TypeAnimation.MoveLocal)
        {
            auxArg.targetPosition = EditorGUILayout.Vector3Field("Target Position", auxArg.targetPosition);
        }
    }

    private void ShowTargetScale(AnimationAssistant auxArg)
    {
        if (auxArg.animationType == TypeAnimation.Scale || auxArg.animationType == TypeAnimation.ScaleReturnOriginScale)
        {
            auxArg.targetScale = EditorGUILayout.Vector3Field("Target Scale", auxArg.targetScale);
        }
    }
}

#endif
