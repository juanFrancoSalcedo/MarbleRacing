using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UnlockMarble : MonoBehaviour
{
    [SerializeField] private DataController dataController= null;
    [SerializeField] private TextMeshProUGUI textPercentage= null;
    [SerializeField] private Image imageFill= null;
    [SerializeField] private Animator animIsUnlock= null;
    [SerializeField] private Button buttonClaim = null;
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
