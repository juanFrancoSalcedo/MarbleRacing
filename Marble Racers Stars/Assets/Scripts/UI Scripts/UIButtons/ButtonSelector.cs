using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelector : BaseButtonComponent
{
    [SerializeField] Image ballImage = null;
    [SerializeField] MarbleSelector selectorManger = null;
    private Image imageCompo;
    // buffer the marble's name of this object
    string marbleNameSelection;
    int ralIn = 0;
    private void OnEnable()
    {
        SetMarbleImage();
        imageCompo = GetComponent<Image>();
        selectorManger.OnSelectedButton += SelectionState;
        buttonComponent.onClick.AddListener(SendSelection);
    }

    private void OnDisable()
    {
        selectorManger.OnSelectedButton -= SelectionState;
        buttonComponent.onClick.RemoveListener(SendSelection);
    }

    public void SetMarbleImage()
    {
        string marbleStrin;
        marbleNameSelection = RaceController.Instance.dataManager.GetItemByIndex(transform.GetSiblingIndex()).name;
        marbleStrin = "MarblesFavicon/" + marbleNameSelection.Replace("(M)","(I)");
        Sprite resource = Resources.Load<Sprite>(marbleStrin);
        ballImage.sprite = resource;
    }

    void SendSelection()
    {
        selectorManger.SelectMarble(selectorManger.allMarbles.GetIndexOfSpecificName(marbleNameSelection), transform.GetSiblingIndex());
    }

    private void SelectionState(int disparo)
    {
        if (disparo == transform.GetSiblingIndex())
            imageCompo.color = Color.cyan;
        else
            imageCompo.color = Color.white;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        { 
            print(DataController.Instance.GetSpecificKeyInt(KeyStorage.ITEMS_UNLOCKED_I));
            print(DataController.Instance.GetSpecificKeyString(KeyStorage.SEED_ITEMS_S));
        }
    }
}
