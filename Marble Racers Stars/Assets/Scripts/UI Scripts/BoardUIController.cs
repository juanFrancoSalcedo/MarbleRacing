﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using MyBox;

public class BoardUIController : MonoBehaviour
{
    [SerializeField] private bool actualizarTiempo;
    [SerializeField] private bool updateAnimation = false;
    private BoardParticipant boardParticip;
    public BoardParticipant BoardParticip {
        get 
        {
            if (boardParticip ==null) 
            boardParticip = GetComponent<BoardParticipant>();
            return boardParticip; 
        }
         set { } 
    }
    public TextMeshProUGUI textPosition;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textScores;
    public Image ImageBackground;
    public Image ImageCircle;
    public Image ImageCircleOutline;
    public Image imageCover;
    public bool scaleFromZero = true;
    public bool UpdateAnimation { get { return updateAnimation; } set { updateAnimation = value; } }
    public Marble bufferMarble { get; set; }
    private int previousSiblingIndex = 0;
    public TypeBoardDisplay boardDisplayType = TypeBoardDisplay.timeInterval;

    void Awake()
    {
        ImageBackground = GetComponent<Image>();
        if (scaleFromZero)
            ImageBackground.rectTransform.DOScaleY(0, 0);
    }

    private void Update()
    {
        if (updateAnimation) 
        {
            if (previousSiblingIndex != transform.GetSiblingIndex())
                StartAnimation();
            previousSiblingIndex = transform.GetSiblingIndex();
        }
    }

    bool startedUpdate;
    public void UpdateAutomatically() 
    {
        if (!startedUpdate)
            StartCoroutine(UpdateScoreAtomatically());
        startedUpdate = true;
    }

    private IEnumerator UpdateScoreAtomatically() 
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f);
            textScores.text = TextDataByDisplayType();
        }
    }

    public void SusbcribeChangeModeBSpec()
    {
        BSpecManager.Instance.onDisplayChanged += delegate (TypeBoardDisplay t) { boardDisplayType = t; };
    }

    private string TextDataByDisplayType() 
    {
        float lol = 0;
        Marble next = RaceController.Instance.GetMarbleByPosition(transform.GetSiblingIndex()-1);
        imageCover.gameObject.SetActive(false);
        textScores.color = Color.white;
        switch (boardDisplayType) 
        {
            //case TypeBoardDisplay.distance:
            //    if (next == null)
            //        return "Leader";
            //    lol = (float) boardParticip.score - next.boardController.boardParticip.score;
            //    break;

            case TypeBoardDisplay.timeInterval:
                if (next == null)
                    return "Interval";
                lol = Vector3.Distance(bufferMarble.transform.position,next.transform.position)/bufferMarble.rb.linearVelocity.magnitude;
                return lol.ToString("f2").Replace(',',':');

            case TypeBoardDisplay.pointsPlus:
                lol = LeagueManager.LeagueRunning.GetScoresByPilot(bufferMarble.namePilot);
                return lol.ToString() +"+"+Constants.pointsPerRacePosition[transform.GetSiblingIndex()];

            case TypeBoardDisplay.pitStops:
                lol = bufferMarble.pitStopCount;
                return lol.ToString();

            case TypeBoardDisplay.cover:
                textScores.color = Color.black;
                imageCover.gameObject.SetActive(true);
                TintCoverImage(bufferMarble.marbleCovering);
                return ""+bufferMarble.marbleCovering.ToString()[0];
        }
        return lol.ToString("f2");
    }

    private void TintCoverImage(TypeCovering covering) 
    {
        switch (covering) 
        {
            case TypeCovering.HardElastic:
                imageCover.color = new Color(192f / 255f, 253f / 255f, 126f / 255f);
                break;
            case TypeCovering.Medium:
                imageCover.color = new Color(255f / 255f, 193f / 255f, 7f / 255f);
                break;
            case TypeCovering.SoftRough:
                imageCover.color = new Color(255f/255f,77f/255f,77f/255f);
                break;
        }
    }

    //[ButtonMethod]
    //void Kool()
    //{
    //    textPosition.color = Color.black;
    //    textPosition.rectTransform.sizeDelta = new Vector2(70,70);
    //    //ImageCircle = ImageCircleOutline.transform.GetChild(0).GetComponent<Image>();
    //    //DestroyImmediate(ImageCircleOutline.transform.GetChild(1).gameObject);
    //    //ImageCircle.sprite = null;
    //    //ImageCircle.color = Color.white;
    //}

    public void StartAnimation(string positionText, string nameText, string scoreText, bool isPlayer, Sprite marbleThubnail,Marble marrble)
    {
        if (marbleThubnail == null) 
            StartCoroutine(UpdateImageMarble());
        bufferMarble = marrble;
        textPosition.text = positionText;
        textName.text = nameText + " " + ((!isPlayer) ? marrble.marbleInfo.abbreviation : Constants.AbbrevetionNameNormi(RaceController.Instance.dataManager));
        textScores.text = scoreText;
        ImageCircle.sprite = marbleThubnail;
        ImageCircleOutline.enabled = isPlayer;
        ImageBackground.GetComponent<Outline>().enabled = isPlayer;
        //ImageBackground.color = (isPlayer) ? new Color(239f/255f,183f/255f,74f/255f,1f): ImageBackground.color;

        if (scaleFromZero)
            ImageBackground.rectTransform.DOScaleY(0, 0);
        ImageBackground.rectTransform.DOScaleY(1, 0.3f).SetEase(Ease.OutBounce).SetUpdate(true).OnComplete(delegate 
            {
                if (positionText.Equals("<POS>"))
                    textPosition.text =""+(transform.GetSiblingIndex()+1);
            }
        );
    }

    public void StartAnimation(string positionText, string nameText, string scoreText, bool isPlayer, Sprite marbleThubnail)
    {
        bool isNormi = nameText.Equals(Constants.NORMI);
        string playerName = (isNormi)? Constants.ReplaceNameNormi(RaceController.Instance.dataManager): nameText;
        textPosition.text = positionText;
        textName.text = playerName;
        textScores.text = scoreText;
        ImageCircle.sprite = (isNormi)?RaceController.Instance.marblePlayerInScene.marbleInfo.spriteMarbl :marbleThubnail;
        ImageCircleOutline.enabled = isPlayer;
        ImageBackground.GetComponent<Outline>().enabled = isPlayer;
        //ImageBackground.color = (nameText.Equals(isNormi)) ? new Color(239f / 255f, 183f / 255f, 74f / 255f, 1f) : ImageBackground.color;

        if (scaleFromZero)
            ImageBackground.rectTransform.DOScaleY(0, 0);
        ImageBackground.rectTransform.DOScaleY(1, 0.3f).SetEase(Ease.OutBounce).SetUpdate(true);
    }

    public void StartAnimation() 
    {
        textPosition.text = "" + (transform.GetSiblingIndex() + 1);
        if (scaleFromZero)
            ImageBackground.rectTransform.DOScaleY(0, 0);
        ImageBackground.rectTransform.DOScaleY(1, 0.3f).SetEase(Ease.OutBounce).SetUpdate(true);
    }

    public void EndAnimation()
    {
        ImageBackground.rectTransform.DOScaleY(0,0.3f).SetEase(Ease.InBounce).SetUpdate(true);
    }

    IEnumerator UpdateImageMarble() 
    {
        while(ImageCircle.sprite == null) 
        {
            ImageCircle.sprite = bufferMarble.marbleInfo.spriteMarbl;
            yield return new WaitForEndOfFrame();
        }
    }
}

public enum TypeBoardDisplay 
{
    timeInterval,
    pointsPlus,
    pitStops,
    cover
}
