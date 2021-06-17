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
    [SerializeField] private ParticleSystem particlesDown = null;
    [SerializeField] private ParticleSystem particlesCharged = null;
    [SerializeField] private ParticleSystem particlePermanent = null;
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
            particlesDown.Play();
            particlesCharged.gameObject.SetActive(false);
            particlePermanent.gameObject.SetActive(false);
            imageFill.fillAmount = 0;
            keyWord.DOFade(0.1f, 0.5f);
            marbleTurbo.ApplyForceLimited();
            if (secondMarbleTurbo != null)
            {
                secondMarbleTurbo.ApplyForceLimited(true);
            }
            imageFill.color = colorBuf;
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

    private void ChargedShow()
    {
        imageFill.color = fullFillColor;
        particlesCharged.gameObject.SetActive(true);
        particlePermanent.gameObject.SetActive(true);
        keyWord.DOFade(0.6f, 0.5f);
    }
}
