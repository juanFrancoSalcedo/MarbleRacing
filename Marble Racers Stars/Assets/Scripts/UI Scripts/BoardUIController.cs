using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using MyBox;

public class BoardUIController : MonoBehaviour
{
    private BoardParticipant boardParticip;
    public BoardParticipant BoardParticip {
        get { boardParticip = GetComponent<BoardParticipant>(); return boardParticip; }
         set { } 
    }
    public TextMeshProUGUI textPosition;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textScores;
    public Image ImageBackground;
    public Image ImageCircle;
    public Image ImageCircleOutline;
    public bool scaleFromZero = true;
    Marble bufferMarble;

    void Awake()
    {
        ImageBackground = GetComponent<Image>();
        
        if (scaleFromZero)
        {
            ImageBackground.rectTransform.DOScaleY(0, 0);
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
        {
            StartCoroutine(UpdateImageMarble());
        } 
        bufferMarble = marrble;
        textPosition.text = positionText;
        textName.text = nameText;
        textScores.text = scoreText;
        ImageCircle.sprite = marbleThubnail;
        ImageCircleOutline.enabled = isPlayer;
        ImageBackground.GetComponent<Outline>().enabled = isPlayer;
        ImageBackground.color = (isPlayer) ? new Color(239f/255f,183f/255f,74f/255f,1f): ImageBackground.color;

        if (scaleFromZero)
        {
            ImageBackground.rectTransform.DOScaleY(0, 0);
        }
        ImageBackground.rectTransform.DOScaleY(1,0.3f).SetEase(Ease.OutBounce).SetUpdate(true);
    }

    public void StartAnimation(string positionText, string nameText, string scoreText, bool isPlayer, Sprite marbleThubnail)
    {
        textPosition.text = positionText;
        textName.text = nameText;
        textScores.text = scoreText;
        ImageCircle.sprite = marbleThubnail;
        ImageCircleOutline.enabled = isPlayer;
        ImageBackground.GetComponent<Outline>().enabled = isPlayer;
        ImageBackground.color = (isPlayer) ? new Color(239f / 255f, 183f / 255f, 74f / 255f, 1f) : ImageBackground.color;

        if (scaleFromZero)
        {
            ImageBackground.rectTransform.DOScaleY(0, 0);
        }
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
