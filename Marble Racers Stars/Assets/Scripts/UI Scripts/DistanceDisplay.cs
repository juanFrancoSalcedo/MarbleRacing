using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class DistanceDisplay : MonoBehaviour,IMainExpected
{
    Marble marRead;
    [SerializeField] private Image myImage;
    [SerializeField] private Image frontImage;
    [SerializeField] private Image behindImage;
    [SerializeField] TextMeshProUGUI textDistanceBehind;
    [SerializeField] TextMeshProUGUI textDistanceFront;
    private bool raceStarted;
    void Start() => SubscribeToTheMainMenu();
    public void SubscribeToTheMainMenu() => MainMenuController.GetInstance().OnRaceReady += ReadyToPlay;
    public void ReadyToPlay() => Invoke("CouldTime", 6f);
    void CouldTime() => StartCoroutine(SortDistance());

    IEnumerator SortDistance() 
    {
        marRead = RaceController.Instance.marblePlayerInScene;
        myImage.enabled = true;
        frontImage.enabled = true;
        behindImage.enabled = true;
        if (marRead == null) 
        {
            print("renegado");
        }
        myImage.sprite = marRead.marbleInfo.spriteMarbl;
        
        while (gameObject.activeInHierarchy) 
        {
            if (myImage.sprite == null) 
                myImage.sprite = RaceController.Instance.marblePlayerInScene.marbleInfo.spriteMarbl;
            int posBoard = marRead.boardController.transform.GetSiblingIndex();
            ShowFront(posBoard);
            ShowBehind(posBoard);
            yield return new WaitForSeconds(0.6f);
        }
    }

    void ShowFront(int posInBoard) 
    {
        myImage.sprite = marRead.marbleInfo.spriteMarbl;
        Sprite spriteBefore =null;
        Marble frontMarb = null;

        if (frontImage.gameObject.activeInHierarchy) spriteBefore = frontImage.sprite;
        
        if (posInBoard == 0)
        {
            textDistanceFront.text = "";
            frontImage.gameObject.SetActive(false);
        }
        else
        {
            frontMarb = RaceController.Instance.GetPositionMarble((posInBoard - 1));
            if (frontMarb == null)
            {
                return;
            }
            float dist = marRead.boardController.BoardParticip.score - frontMarb.boardController.BoardParticip.score;
            textDistanceFront.text = dist.ToString("f1");
            frontImage.gameObject.SetActive(true);
            frontImage.sprite = frontMarb.marbleInfo.spriteMarbl;
        }

        if (frontImage.gameObject.activeInHierarchy)
        {
            if (spriteBefore != null && !ReferenceEquals(frontMarb.marbleInfo.spriteMarbl, spriteBefore))
            {
                float posX = frontImage.transform.localPosition.x+24;
                float posXOrig = frontImage.transform.localPosition.x;
                frontImage.DOFade(0,0);
                frontImage.transform.DOLocalMoveX(posX,0);
                frontImage.transform.DOLocalMoveX(posXOrig,0.5f);
                frontImage.DOFade(1,0.5f);
            }
        }
    }

    void ShowBehind(int posInBoard) 
    {
        Sprite spriteBefore = null;
        Marble behindMarb = null;

        if (behindImage.gameObject.activeInHierarchy) spriteBefore = behindImage.sprite;

        if (posInBoard < RaceController.Instance.leaderBoardPositions.participantScores.Length-1)
        {
            behindMarb = RaceController.Instance.GetPositionMarble(posInBoard + 1);
            if (behindMarb == null)
            {
                return;
            }
            float dist = marRead.boardController.BoardParticip.score - behindMarb.boardController.BoardParticip.score;
            textDistanceBehind.text = dist.ToString("f1");
            behindImage.gameObject.SetActive(true);
            behindImage.sprite = behindMarb.marbleInfo.spriteMarbl;
        }
        else
        {
            textDistanceBehind.text = "";
            behindImage.gameObject.SetActive(false);
        }

        if (behindImage.gameObject.activeInHierarchy)
        {
            if (spriteBefore != null && !ReferenceEquals(behindMarb.marbleInfo.spriteMarbl, spriteBefore))
            {
                float posX = behindImage.transform.localPosition.x - 24;
                float posXOrig = behindImage.transform.localPosition.x;
                behindImage.DOFade(0, 0);
                behindImage.transform.DOLocalMoveX(posX, 0);
                behindImage.transform.DOLocalMoveX(posXOrig, 0.5f);
                behindImage.DOFade(1, 0.5f);
            }
        }
    }

}
