using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEditor;

public class CinematicAssistant : MonoBehaviour
{
    public CinematicController control;// { get; set; }
    [HideInInspector]
    public bool forSkip = false;
    [SerializeField] private float delayCompleted = 0.6f;
    [SerializeField] private bool misionCompleted;
    public TypeMisionTutorial missionType;

    [HideInInspector] public DoAnimationController animatorControl;
    [HideInInspector] public TypingAnimator typingObject;
    [HideInInspector] public string textToShow;
    [HideInInspector] public TMP_InputField fieldTextMision;
    [HideInInspector] public string textEquivalent;
    [HideInInspector] public UnityEngine.UI.Button buttonMision;
    [HideInInspector] public List<AnimationAssistant> animationsMission = new List<AnimationAssistant>();
    public UnityEvent OnCompletedMission;
    [HideInInspector] public SideCharacter characterSide;
    [HideInInspector] public LanguageCharacter languageDialogue;
    [HideInInspector] public bool characterApeear;
    //[HideInInspector] public CharacterScriptable characterStats;
    //[HideInInspector] public ChapterChecker chapterReader;

    private void OnEnable()
    {
        control = GetComponentInParent<CinematicController>();
        switch (missionType)
        {
            case TypeMisionTutorial.animationCompleted:
                animatorControl.listAux = animationsMission;
                animatorControl.OnCompleted += MisionFinished;
                break;

            case TypeMisionTutorial.textCompleted:
                typingObject.OnComplitedText += MisionFinished;
                typingObject.textCompo.text = textToShow;
                typingObject.EraseAndSaveText();
                break;

            case TypeMisionTutorial.buttonClicked:
                buttonMision.onClick.AddListener(MisionFinished);
                break;

            case TypeMisionTutorial.dialogue:
                control.dialogue.OnComplitedText += MisionFinished;
                break;

            //case TypeMisionTutorial.chapterReader:
            //    chapterReader.OnChaptered += MisionFinished;
            //    break;
        }
    }

    private void Update()
    {
        if (control == null)
        {
            control = GetComponentInParent<CinematicController>();
        }

        switch (missionType)
        {
            case TypeMisionTutorial.textEquals:

                if (fieldTextMision.text.Equals(textEquivalent) && !misionCompleted)
                {
                    MisionFinished();
                    misionCompleted = true;
                }
                break;
        }
    }

    public void StartMision()
    {
        if (misionCompleted)
        {
            MisionFinished();
        }
        else
        {
            switch (missionType)
            {
                case TypeMisionTutorial.animationCompleted:
                    animatorControl.listAux = animationsMission;
                    animatorControl.ActiveAnimation();
                    break;

                case TypeMisionTutorial.textCompleted:
                    typingObject.StartAnimation();
                    break;

                //case TypeMisionTutorial.dialogue:
                //    ShowDialogue();
                //    break;
            }
        }
    }

    public void MisionFinished()
    {
        Invoke("SendReport", delayCompleted);
        switch (missionType)
        {
            case TypeMisionTutorial.animationCompleted:
                animatorControl.OnCompleted -= MisionFinished;
                break;

            case TypeMisionTutorial.textCompleted:
                typingObject.OnComplitedText -= MisionFinished;
                break;

            case TypeMisionTutorial.buttonClicked:
                buttonMision.onClick.RemoveListener(MisionFinished);
                break;

            case TypeMisionTutorial.dialogue:
                control.dialogue.OnComplitedText -= MisionFinished;
                break;

            //case TypeMisionTutorial.chapterReader:
            //    chapterReader.OnChaptered -= MisionFinished;
            //    break;
        }
    }

    public void SendReport()
    {
        OnCompletedMission?.Invoke();
        if (forSkip)
        {
            //control.SkipMission();
        }
        else
        {
            control.NextMision();
        }
        gameObject.SetActive(false);
    }

    public void BackController()
    {
        bool disableAfter = false;
        if (!control.gameObject.activeInHierarchy)
        {
            control.gameObject.SetActive(true);
            disableAfter = true;
        }

#if UNITY_EDITOR
        Selection.activeObject = control.gameObject;
#endif

        if (disableAfter) control.gameObject.SetActive(false);
    }

    public void SelfDestroy()
    {
        if (control != null)
        {
            control.missionAnimations.Remove(this);
        }
        Invoke("DestroyByTime",0.01f);
    }

    private void DestroyByTime()
    {
        DestroyImmediate(gameObject);
    }

    public void FindControl()
    {
        control = transform.parent.GetComponent<CinematicController>();
        control = GetComponentInParent<CinematicController>();
    }

    //private void ShowDialogue()
    //{
    //    control.dialogue.SetFont(FontPool.GetInstance().GetFontLnaguage(languageDialogue));

    //    if (characterSide == SideCharacter.Left)
    //    {
    //        control.name1.SetTextFull(characterStats.characterStats.nameCharacter);
    //        control.name1.ShowFullText();
    //        control.dialogue.SetColor(characterStats.colorCharacter);
    //        if (characterApeear)
    //        {
    //            control.imageChar1.ActiveAnimation();
    //            control.name1.SetColor(characterStats.colorCharacter);
    //            control.name1.SetFont(characterStats.fontCharacterName);
    //            control.imageChar1.GetComponent<UnityEngine.UI.Image>().sprite = characterStats.characterStats.spriteCharacter;
    //        }
    //    }
    //    else if (characterSide == SideCharacter.Right)
    //    {
    //        control.name2.SetTextFull(characterStats.name);
    //        control.name2.ShowFullText();
    //        control.dialogue.SetColor(characterStats.colorCharacter);
    //        if (characterApeear)
    //        {
    //            control.imageChar2.ActiveAnimation();
    //            control.name2.SetColor(characterStats.colorCharacter);
    //            control.name2.SetFont(characterStats.fontCharacterName);
    //            control.imageChar2.GetComponent<UnityEngine.UI.Image>().sprite = characterStats.characterStats.spriteCharacter;
    //        }
    //    }
    //    else if (characterSide == SideCharacter.None)
    //    {
    //        control.dialogue.SetColor(Color.black);
    //    }
        
    //    control.dialogue.SetTextFull(textToShow);
    //    control.dialogue.StartAnimation();
    //}
}
