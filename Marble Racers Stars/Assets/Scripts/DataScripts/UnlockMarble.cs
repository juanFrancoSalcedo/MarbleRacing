using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UnlockMarble : MonoBehaviour
{
    [SerializeField] private DataManager dataController;
    [SerializeField] private TextMeshProUGUI textPercentage;
    [SerializeField] private Image imageFill;
    [SerializeField] private Animator animIsUnlock;
    [SerializeField] private Button buttonClaim;
    bool showed;

    private void OnEnable()
    {
        ShowPercentageMarble();
    }

    public void ShowPercentageMarble()
    {
        int percent = (dataController.GetMarblePercentage() > 100) ? 100 : dataController.GetMarblePercentage(); 
        float amount = (float)(percent/100f);
        imageFill.DOFillAmount(amount,0.5f).SetEase(Ease.OutQuad).SetDelay(1);
        textPercentage.text = "" + percent + "%";
        if (percent >= 100)
        {
            buttonClaim.enabled = true;
            buttonClaim.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Claim Marble";
            animIsUnlock.SetBool("Unlock", true);
        }
        showed = true;
    }

    private void Update()
    {
        if (!showed)
        {
            ShowPercentageMarble();
        }
    }
}
