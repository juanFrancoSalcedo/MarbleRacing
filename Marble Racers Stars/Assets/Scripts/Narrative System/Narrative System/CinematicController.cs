using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
public class CinematicController : MonoBehaviour
{
    [HideInInspector] bool skpButton;
    [SerializeField] bool rewindAtEnd;
    [HideInInspector]
    public UnityEngine.UI.Button buttonContinue;
    [HideInInspector]
    public TypingAnimator dialogue;
    [HideInInspector]
    public TypingAnimator name1;
    [HideInInspector]
    public TypingAnimator name2;
    [HideInInspector]
    public DoAnimationController imageChar1;
    [HideInInspector]
    public DoAnimationController imageChar2;
    [HideInInspector]
    public bool inInspector;
    [HideInInspector]
    public DoAnimationController panelAnim;
    [HideInInspector]
    public List<CinematicAssistant> missionAnimations = new List<CinematicAssistant>();
    [HideInInspector]
    public List<CinematicAssistant> skipeAnimations = new List<CinematicAssistant>();
    [HideInInspector]
    public int misionIndex = -1;
    private int bufferMissionIndex =-1;
    private int skipeMisionIndex = -1;
    public int IDLastButtonAssistants { get; set; } = 0;

    private void OnValidate()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.hideFlags = HideFlags.HideInHierarchy;
        }

        if (skpButton)
        {
            SkipMission();
            skpButton=false;
        }
    }

    private void OnEnable()
    {
        if (rewindAtEnd)
        {
            misionIndex = bufferMissionIndex;
        }
        Invoke("NextMision",0.03f);
    }

    public void NextMision()
    {
        if (misionIndex + 1 >= missionAnimations.Count)
        {
            return;
        }
        misionIndex++;
        missionAnimations[misionIndex].gameObject.SetActive(true);
        missionAnimations[misionIndex].StartMision();
    }

    [ButtonMethod]
    private void SkipMission()
    {
        misionIndex = missionAnimations.Count-2;
        NextMision();

        //if (skipeMisionIndex + 1 >= skipeAnimations.Count)
        //{
        //    return;
        //}
        //skipeMisionIndex++;
        //skipeAnimations[skipeMisionIndex].gameObject.SetActive(true);
        //skipeAnimations[skipeMisionIndex].StartMision();
    }

    public void InstanciateAssitant()
    {
        GameObject gameAssis = new GameObject();
        gameAssis.transform.SetParent(transform);
        gameAssis.AddComponent(typeof(CinematicAssistant));

        CinematicAssistant assitant = gameAssis.GetComponent<CinematicAssistant>();
        missionAnimations.Add(assitant);
        assitant.control = this;
        gameAssis.SetActive(false);
        gameAssis.name = "a" + missionAnimations.IndexOf(assitant);
        gameAssis.hideFlags = HideFlags.HideInHierarchy;
    }

    public void DeleteAllMissions()
    {
        missionAnimations.Clear();
        for (int i=0; i< transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
            transform.GetChild(i).GetComponent<CinematicAssistant>().SelfDestroy();
        }
    }

    public void SetIndexMission(CinematicAssistant assi)
    {
        int boom = missionAnimations.IndexOf(assi);
        boom--;
        misionIndex = boom;
        bufferMissionIndex = misionIndex;
    }

    public void ShowInInspectorDialogs()
    {
        inInspector = !inInspector;
    }
}

public enum TypeMisionTutorial
{
    None,
    textEquals,
    textCompleted,
    animationCompleted,
    buttonClicked,
    dialogue
    //chapterReader
}

public enum SideCharacter
{
    None,
    Left,
    Right
}

public enum LanguageCharacter
{
    Tarbla,
    Deosumicio,
    Narrador
}

