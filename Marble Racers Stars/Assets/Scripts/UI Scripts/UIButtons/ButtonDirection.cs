using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ButtonDirection : MonoBehaviour
{
    [SerializeField] private Marble marbleTurbo = null;
    [SerializeField] private Marble secondMarbleTurbo = null;
    private Marble thirdMarbleTurbo = null;
    [SerializeField] private bool forRight = false;
    [SerializeField] private Color fullFillColor = Color.white;

    [Header("~~~~~~KinesteticObjects~~~~~~~~")]
    [SerializeField] private Image imageFill = null;
    [SerializeField] private Image imageGlow = null;
    [SerializeField] private TextMeshProUGUI keyWord = null;
    private bool charged = false;
    private Color colorBuf = Color.white;

    private void Start()
    {
        colorBuf = imageFill.color;
        marbleTurbo = RaceController.Instance.marblePlayerInScene;
        SetSecondPlayer();
        SearchDopelganger();
    }

    private void SetSecondPlayer()
    {
        if (RaceController.Instance.SecondPlayerInScene != null)
            secondMarbleTurbo = RaceController.Instance.SecondPlayerInScene;
    }

    private void SearchDopelganger()
    {
        if (secondMarbleTurbo == null || thirdMarbleTurbo == null)
        {
            Marble[] marblesPosibles = GameObject.FindObjectsOfType<Marble>();

            while (secondMarbleTurbo == null)
            {
                int randi = Random.Range(0, marblesPosibles.Length);
                secondMarbleTurbo = (!marblesPosibles[randi].isPlayer)? marblesPosibles[randi]: null;
            }

            while (thirdMarbleTurbo == null)
            {
                int randi = Random.Range(0, marblesPosibles.Length);
                thirdMarbleTurbo = (!marblesPosibles[randi].isPlayer) ? marblesPosibles[randi] : null;
            }
        }
    }

    void Update()
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

        if (Input.GetAxisRaw("Horizontal") == -1 && !forRight)
        {
            SendForceMarble();
        }
        if (Input.GetAxisRaw("Horizontal") == 1 && forRight)
        {
            SendForceMarble();
        }

        if (imageFill.fillAmount < 1)
        {
            imageFill.fillAmount = ((forRight)? (float)marbleTurbo.rightEnergy: (float)marbleTurbo.leftEnergy) / (Constants.timeDriving-marbleTurbo.Stats.coldTimeDirection);
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

    public void SendForceMarble()
    {
        if (charged)
        {
            charged = false; 
            PressedShow();
            if (forRight)
            {
                marbleTurbo.ApplyForceLimited(true);
                if (secondMarbleTurbo != null)
                {
                    secondMarbleTurbo.ApplyForceLimited(true);
                }
                if (thirdMarbleTurbo != null)
                {
                    thirdMarbleTurbo.ApplyForceLimited(true);
                }
            }
            else
            {
                marbleTurbo.ApplyForceLimited(false);
                if (secondMarbleTurbo != null)
                {
                    secondMarbleTurbo.ApplyForceLimited(false);
                }
                if (thirdMarbleTurbo != null)
                {
                    thirdMarbleTurbo.ApplyForceLimited(false);
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
        imageGlow.rectTransform.DOScale(1, 0.4f).SetEase(Ease.InOutExpo).OnPlay(() => imageGlow.gameObject.SetActive(true));
        imageGlow.DOFade(1, 0.4f);
    }

    [Header("Omitir")]
    [SerializeField] RectTransform limitDown;
    [SerializeField] RectTransform middlePoint;
    [SerializeField] RectTransform limitUp;

    Vector3 CalculateBezier(float _time, Vector3 pi, Vector3 middle, Vector3 pf)
    {
        float powTime = Mathf.Pow(_time,2);
        float powU = Mathf.Pow(1-_time,2);
        Vector3 p;
        p = powU * pi + 2*(1 - _time) * _time * middle + powTime * pf;
        return p;
    }
}
