﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour
{
    [SerializeField] Image ballImage;
    [SerializeField] MarbleSelector selectorManger;
    private Button buttonComp;
    private Image imageCompo;
    bool selected;


    private void OnEnable()
    {
        SetMarbleImage();
        imageCompo = GetComponent<Image>();
        buttonComp = GetComponent<Button>();
        selectorManger.OnSelectedButton += SelectionState;
        buttonComp.onClick.AddListener(SendSelection);
    }

    private void OnDisable()
    {
        selectorManger.OnSelectedButton -= SelectionState;
        buttonComp.onClick.RemoveListener(SendSelection);
    }

    public void SetMarbleImage()
    {
        string marbleStrin;
        marbleStrin = "MarblesFavicon/" + selectorManger.allMarbles.GetNameSpecificIndex(GetRealIndex()).Replace("(M)", "(I)");
        Sprite resource = Resources.Load<Sprite>(marbleStrin);
        ballImage.sprite = resource;
    }

    void SendSelection()
    {
        selected = true;
        selectorManger.SelectMarble(GetRealIndex(), transform.GetSiblingIndex());
    }

    private int GetRealIndex() => RaceController.Instance.dataManager.allMarbles.GetIndexOfSpecificName(
            RaceController.Instance.dataManager.GetItemByIndex(transform.GetSiblingIndex()).name);

    private void SelectionState(int disparo)
    {
        if (disparo == transform.GetSiblingIndex())
            imageCompo.color = Color.cyan;
        else
            imageCompo.color = Color.white;
    }
}
