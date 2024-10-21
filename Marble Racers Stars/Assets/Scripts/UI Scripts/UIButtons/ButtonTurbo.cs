using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ButtonTurbo : MonoBehaviour
{
    [SerializeField] private Marble marbleTurbo = null;
    [SerializeField] private Marble secondMarbleTurbo = null;
    [SerializeField] private Color fullFillColor = Color.white;

    [Header("~~~~~~KinesteticObjects~~~~~~~~")]
    [SerializeField] private Image imageFill = null;
    [SerializeField] private Image imageGlow = null;
    //[SerializeField] RectTransform limitDown = null;
    //[SerializeField] RectTransform limitUp = null;
    [SerializeField] private TextMeshProUGUI keyWord = null;
    private bool charged = false;
    private Color colorBuf = Color.white;

    private void OnEnable()
    {
        colorBuf = imageFill.color;
        StartCoroutine(ChargeButton());
        marbleTurbo = RaceController.Instance.marblePlayerInScene;
        SetSecondPlayer();
    }
    private void SetSecondPlayer() 
    {
        if (RaceController.Instance.SecondPlayerInScene != null)
            secondMarbleTurbo = RaceController.Instance.SecondPlayerInScene;
    }

    public void SendForceMarble()
    {
        if (charged)
        {
            charged = false;
            PressedShow();
            marbleTurbo.ApplyForceLimited();
            if (secondMarbleTurbo != null)
            {
                secondMarbleTurbo.ApplyForceLimited(true);
            }
        }
    }
    
    private void Update()
    {
        if (Time.timeScale == 0)
        {
            imageFill.raycastTarget = false;
            keyWord.raycastTarget = false;
        }
        else
        {
            imageFill.raycastTarget = true;
            keyWord.raycastTarget = true;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            SendForceMarble();
        }
    }

    private IEnumerator ChargeButton()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            if (imageFill.fillAmount < 1)
            {
                imageFill.fillAmount =(float) marbleTurbo.frontEnergy/(Constants.timeAceleration-marbleTurbo.Stats.coldTimeTurbo);
            }
            else
            {
                if (!charged)
                {
                    ChargedShow();
                    charged = true;
                }
            }
        }
    }

    private void PressedShow() 
    {
        imageFill.color = colorBuf;
        imageFill.fillAmount = 0;
        imageGlow.rectTransform.DOScale(2, 0.4f).SetEase(Ease.InOutExpo);
        imageGlow.DOFade(0, 0.4f).OnComplete(() => imageGlow.gameObject.SetActive(false));
        keyWord.DOFade(0.1f, 0.4f);
    }
    private void ChargedShow()
    {
        imageFill.color = fullFillColor;
        imageGlow.rectTransform.DOScale(2, 0);
        imageGlow.rectTransform.DOScale(1, 0.4f).SetEase(Ease.InOutExpo).OnPlay(()=>imageGlow.gameObject.SetActive(true));
        imageGlow.DOFade(1, 0.4f);
        if (ToggleAutomatic.IsAutomatic)
            Invoke(nameof(AutomaticPressed), 0.4f);
    }

    private void AutomaticPressed()
    {
        SendForceMarble();
    }
}
