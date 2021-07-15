using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Notification : MonoBehaviour
{
    [SerializeField] private Image[] imagesNotification = null;
    [SerializeField] private GameObject[] objsDisable = null;
    private bool areAvalibleNotifications = false;
    [SerializeField] protected DataController dataManager = null;
    int debtMoney = 0;
    int debtTrophies = 0; 

    void Start()
    {
        int trophiesNecesity = (int)(debtMoney / Constants.moneyPerTrophy); 
        int money = dataManager.GetMoney();
        int trophies = dataManager.GetTrophys();
        if (money >= debtMoney && trophies >= trophiesNecesity)
        {
            areAvalibleNotifications = true;
        }
        else
        {
            areAvalibleNotifications = false;
        }
        StartCoroutine(ShowAdvice());
    }

    IEnumerator ShowAdvice()
    {
        while (areAvalibleNotifications)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(Vector3.one * 1.5f, 0.6f).SetEase(Ease.OutBack));
            sequence.Append(transform.DOScale(Vector3.one, 0.6f).SetEase(Ease.OutBack));
            yield return new WaitForSeconds(3);
        }

        for (int i = 0; i < imagesNotification.Length; i++)
        {
            imagesNotification[i].enabled = false;
            objsDisable[i].SetActive(false);
        }
    }
}
