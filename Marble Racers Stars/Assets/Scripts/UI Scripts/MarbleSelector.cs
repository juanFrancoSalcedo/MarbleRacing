﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MarbleSelector : MonoBehaviour
{
    public MarbleDataList allMarbles;
    [SerializeField] DataManager dataManager;
    [SerializeField] List<Button> buttonsSelction = new List<Button>();
    [SerializeField] private Animator animatorTraffic;
    RectTransform rectTrans;
    public System.Action<int>  OnSelectedButton;

    void Start()
    {
        rectTrans = GetComponent<RectTransform>();

        buttonsSelction[0].gameObject.SetActive(true);
        for (int i = 1; i < dataManager.GetItemUnlockedCount() + 1; i++)
        {
            Button but = Instantiate(buttonsSelction[0],buttonsSelction[0].transform.parent);
            buttonsSelction.Add(but);
            buttonsSelction[i].gameObject.SetActive(true);
        }
    }

    public void OpenClose()
    {
        if (rectTrans.localScale.x == 0)
        {
            rectTrans.DOScaleX(1, 0.4f).OnComplete(EnableVisualComponents).SetEase(Ease.OutQuart);
            animatorTraffic.SetBool("Customizing",true);
        }
        else
        {
            rectTrans.DOScaleX(0, 0.4f).OnComplete(DisableVisualComponents).SetEase(Ease.OutQuart);
            animatorTraffic.SetBool("Customizing", false);
        }
    }

    void EnableVisualComponents()
    {
        GetComponent<Image>().enabled = true;
        GetComponent<ScrollRect>().enabled = true;
    }

    void DisableVisualComponents()
    {
        GetComponent<Image>().enabled = false;
        GetComponent<ScrollRect>().enabled = false;
    }

    public void SelectMarble(int idex)
    {
        OnSelectedButton?.Invoke(idex);

        RaceController.Instance.marblePlayerInScene.SetMarbleSettings(idex);
    }
}
