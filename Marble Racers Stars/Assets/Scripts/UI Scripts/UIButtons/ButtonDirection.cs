using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ButtonDirection : MonoBehaviour
{
    [SerializeField] private Marble marbleTurbo;
    [SerializeField] private Marble secondMarbleTurbo;
    private Marble thirdMarbleTurbo;
    [SerializeField] private bool forRight;
    [SerializeField] private Color fullFillColor;

    [Header("~~~~~~KinesteticObjects~~~~~~~~")]
    [SerializeField] private Image imageFill;
    [SerializeField] private ParticleSystem particlesDown;
    [SerializeField] private ParticleSystem particlesCharged;
    [SerializeField] private ParticleSystem particlePermanent;
    [SerializeField] private TextMeshProUGUI keyWord;
    private bool charged;
    private Color colorBuf;

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
            imageFill.fillAmount = (forRight)? (float)marbleTurbo.rightEnergy: (float)marbleTurbo.leftEnergy/ Constants.timeDriving;// *timeRestore;
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
            particlesDown.Play();
            particlesCharged.gameObject.SetActive(false);
            particlePermanent.gameObject.SetActive(false);
            imageFill.fillAmount = 0;
            keyWord.DOFade(0.1f, 0.5f);
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
            imageFill.color = colorBuf;
        }
    }

    private void ChargedShow()
    {
        imageFill.color = fullFillColor;
        particlesCharged.gameObject.SetActive(true);
        particlePermanent.gameObject.SetActive(true);
        keyWord.DOFade(0.6f, 0.5f);
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
