using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//using UnityEditor.SceneManagement;

#if UNITY_EDITOR
public abstract class BaseEditorAnimator : Editor
{
    public DoAnimationController animationController { get; set; }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        animationController = (DoAnimationController)target;

        if (GUILayout.Button("[Add Animation]"))
        {
            animationController.GetList().Add(new AnimationAssistant());
        }

        foreach (AnimationAssistant aux in animationController.GetList())
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("" + animationController.GetList().IndexOf(aux) + "[" + aux.animationType.ToString() + "]"))
            {
                aux.DisplayAnimationAux();
            }

            GUI.color = Color.red;
            if (GUILayout.Button("-", GUILayout.Width(30)))
            {
                animationController.GetList().Remove(aux);
            }
            GUI.color = Color.white;

            if (GUILayout.Button("<", GUILayout.Width(30)))
            {
                Organizer<AnimationAssistant>.MoveIndexOfAList(aux,animationController.GetList(),true);
            }

            if (GUILayout.Button(">", GUILayout.Width(30)))
            {
                Organizer<AnimationAssistant>.MoveIndexOfAList(aux, animationController.GetList(), false);
            }

            GUILayout.EndHorizontal();

            if (aux.display)
            {
                ShowData(aux);
            }   
        }

        if (GUI.changed) 
        {
            EditorUtility.SetDirty(animationController);
            //EditorSceneManager.MarkSceneDirty(animationController.gameObject.scene);
        }
    }

    protected abstract void ShowData(AnimationAssistant animationAux);
    protected abstract void ShowTargetPosition(AnimationAssistant auxArg);
    protected abstract void ShowTargetScale(AnimationAssistant auxArg);
    protected abstract void ShowTargetRotation(AnimationAssistant auxArg);
    protected virtual void ShowColor(AnimationAssistant auxArg){}
}

#endif
