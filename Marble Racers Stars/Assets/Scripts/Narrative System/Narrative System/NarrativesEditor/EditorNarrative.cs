using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


#if UNITY_EDITOR
using UnityEditor.SceneManagement;

[CustomEditor(typeof(CinematicController))]
public class EditorNarrative : Editor
{
    CinematicController controller;

    public override void OnInspectorGUI()
    {
        controller = (CinematicController) target;

        base.OnInspectorGUI();
        GUI.color = Color.red;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("[Delete All]", GUILayout.Width(80)))
        {
            controller.DeleteAllMissions();
        }

        GUI.color = Color.white;

        if (GUILayout.Button("Show Dialogue Settings", GUILayout.Width(200)))
        {
            controller.ShowInInspectorDialogs();
        }
        GUILayout.EndHorizontal();

        CollapseDialogueSettings();

        GUI.color = Color.yellow;
        if (GUILayout.Button("[Add Assistant]"))
        {
            controller.InstanciateAssitant();
        }
        GUI.color = Color.white;
        
        if (controller.missionAnimations.Count > 0)
        {
            foreach (CinematicAssistant assi in controller.missionAnimations)
            {
                EditorGUILayout.BeginHorizontal();

                if (controller.missionAnimations.IndexOf(assi) == controller.misionIndex+1)
                {
                    GUI.color = Color.green;
                    if (GUILayout.Button("i",GUILayout.Width(15)))
                    {
                       controller.SetIndexMission(assi);
                    }
                }
                else
                {
                    GUI.color = Color.white;
                    if (GUILayout.Button("", GUILayout.Width(15)))
                    {
                        controller.SetIndexMission(assi);
                    }
                }


                GUI.color = (controller.IDLastButtonAssistants == assi.GetInstanceID()) ? Color.cyan : Color.white;
                
                if (GUILayout.Button((assi !=null)?assi.gameObject.name:"null"))
                {
                    controller.IDLastButtonAssistants = assi.GetInstanceID();
                    Selection.activeObject = assi;
                }

                GUI.color = Color.red;
                if (GUILayout.Button("-",GUILayout.Width(30)))
                {
                    assi.SelfDestroy();
                }
                GUI.color = Color.white;

                if (GUILayout.Button("<", GUILayout.Width(30)))
                {
                    Organizer<CinematicAssistant>.MoveIndexOfAList(assi,controller.missionAnimations,true);
                }

                if (GUILayout.Button(">", GUILayout.Width(30)))
                {
                    Organizer<CinematicAssistant>.MoveIndexOfAList(assi, controller.missionAnimations, false);
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(controller);
            EditorSceneManager.MarkSceneDirty(controller.gameObject.scene);
        }
    }

    private void CollapseDialogueSettings()
    {
        if (!controller.inInspector) return;

        //controller.positionInChar1 = EditorGUILayout.Vector3Field("Pos Char1 In",controller.positionInChar1);
        //controller.positionInChar2 = EditorGUILayout.Vector3Field("Pos Char2 In",controller.positionInChar2);
        //controller.positionOutChar1 = EditorGUILayout.Vector3Field("Pos Char1 Out",controller.positionOutChar1);
        //controller.positionOutChar2 = EditorGUILayout.Vector3Field("Pos Char2 Out",controller.positionOutChar2);

        SerializedProperty buttonContin;
        buttonContin= serializedObject.FindProperty("buttonContinue");
        EditorGUILayout.PropertyField(buttonContin, new GUIContent("Button Continue"));

        SerializedProperty dialogue;
        dialogue = serializedObject.FindProperty("dialogue");
        EditorGUILayout.PropertyField(dialogue, new GUIContent("Dialogue"));

        SerializedProperty name1;
        name1 = serializedObject.FindProperty("name1");
        EditorGUILayout.PropertyField(name1, new GUIContent("Name 1"));

        SerializedProperty name2;
        name2 = serializedObject.FindProperty("name2");
        EditorGUILayout.PropertyField(name2, new GUIContent("Name 2"));

        SerializedProperty imageChar1;
        imageChar1 = serializedObject.FindProperty("imageChar1");
        EditorGUILayout.PropertyField(imageChar1, new GUIContent("Image Char 1"));

        SerializedProperty imageChar2;
        imageChar2 = serializedObject.FindProperty("imageChar2");
        EditorGUILayout.PropertyField(imageChar2, new GUIContent("Image Char 2"));

        serializedObject.ApplyModifiedProperties();
    }

}
#endif
