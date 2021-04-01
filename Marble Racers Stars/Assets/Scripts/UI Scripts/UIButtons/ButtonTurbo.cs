using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ButtonTurbo : MonoBehaviour
{
    [SerializeField] private Marble marbleTurbo;
    [SerializeField] private Marble secondMarbleTurbo;
    [SerializeField] private Color fullFillColor;

    [Header("~~~~~~KinesteticObjects~~~~~~~~")]
    [SerializeField] private Image imageFill;
    [SerializeField] private ParticleSystem particlesDown;
    [SerializeField] private ParticleSystem particlesCharged;
    [SerializeField] private ParticleSystem particlePermanent;
    [SerializeField] RectTransform limitDown;
    [SerializeField] RectTransform limitUp;
    [SerializeField] private TextMeshProUGUI keyWord;
    private bool charged;
    private Color colorBuf;
    private float sumFill = 1;

    private void OnEnable()
    {
        colorBuf = imageFill.color;
        StartCoroutine(ChargeButton());
        marbleTurbo = RaceController.Instance.marblePlayerInScene;
    }

    public void SendForceMarble()
    {
        if (charged)
        {
            charged = false;
            particlesDown.Play();
            particlesCharged.gameObject.SetActive(false);
            particlePermanent.gameObject.SetActive(false);
            sumFill = 0;
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
                imageFill.fillAmount =(float) marbleTurbo.frontEnergy/Constants.timeAceleration;
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
