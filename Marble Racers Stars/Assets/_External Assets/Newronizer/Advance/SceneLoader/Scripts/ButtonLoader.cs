using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newronizer;
using Newronizer.SceneLoader;

[RequireComponent(typeof(CallerSceneLoader))]
public class ButtonLoader : BaseButtonAttendant
{
    private void Start() => buttonComponent.onClick.AddListener(LoadScene);
    private void LoadScene() => GetComponent<CallerSceneLoader>().LoadScene();
}