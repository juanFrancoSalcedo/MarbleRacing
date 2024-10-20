﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeagueSYS;
using TMPro;
using MyBox;
using UnityEngine.UI;

public class AwardManager : MonoBehaviour
{
    [SerializeField] bool isNewSkin = false;
    [SerializeField] private Button buttonLoad = null;
    [ConditionalField(nameof(isNewSkin), true)][SerializeField] private Button buttonExtraPoints;
    [SerializeField] private DataController dataManager = null;
    League _league;
    [ConditionalField(nameof(isNewSkin), true)] [SerializeField] TextMeshProUGUI textMoney = null;
    [SerializeField] TextMeshProUGUI textCongratulatio = null;
    [ConditionalField(nameof(isNewSkin), true)] [SerializeField] TextMeshProUGUI textPosition = null;
    [ConditionalField(nameof(isNewSkin), true)] [SerializeField] GameObject firstPositionObj = null;
    [ConditionalField(nameof(isNewSkin), true)] [SerializeField] GameObject otherPositionObj = null;
    [ConditionalField(nameof(isNewSkin))] [SerializeField] MarbleDataList allMarbles = null;
    [ConditionalField(nameof(isNewSkin))] [SerializeField] Marble marbleShow = null;
    [SerializeField] private Material matBlack;

    void Start()
    {
        Time.timeScale = 1;
        buttonLoad.onClick.AddListener(ResetLeague);
        if (isNewSkin)
        {
            NewSkin();
        }
        else
            Invoke(nameof(SearchPlayerMarble), 0.2f);   
    }


    void SearchPlayerMarble()
    {
        // Search the data of player inside the league data and calculate his reward
        var sortedParti = new List<LeagueParticipantData>();
        var manufactures = new LeagueManufactures();
        _league = LeagueManager.LeagueRunning;
        var _leagueParticipant = manufactures.GetParticipantsLeague();
        _leagueParticipant.ForEach(p => sortedParti.Add(p.Copy()));
        int playerPosition  =11;
        BubbleSort<LeagueParticipantData>.SortReverse(sortedParti, "points");
        playerPosition = sortedParti.FindIndex(0, sortedParti.Count, x => x.teamName == Constants.NORMI);
        playerPos = playerPosition;
        ShowAward(playerPosition);
    }
    private int playerPos =0;
    private int GetCoins() => Constants.pointsPerRacePosition[playerPos] * (dataManager.allCups.GetCurrentLeague().multiplierMoney * (int)rewardVideoAmount);
    private void ShowAward(int indexParticipantPlayer) 
    {
        if (indexParticipantPlayer == 0)
        {
            textCongratulatio.text = "Congratulations";
            textPosition.text = "1";
            textMoney.text = "+" + GetCoins();
            CreateTrophy();
            dataManager.WinTrophy(1);

            if (dataManager.GetSpecificKeyInt(KeyStorage.CURRENTCUP_I) > dataManager.GetCupsWon())
                dataManager.WinCup();
            AIUpdateStats();
        }
        else
        {
            textCongratulatio.text = "it wasn't enough";
            textPosition.text = "" + (indexParticipantPlayer + 1);
            textMoney.text = "+" + GetCoins();
            CreateTrophy(matBlack);
        }
    }
    double rewardVideoAmount = 1;
    private void VideoWatched(bool watched) 
    {
        IncreseRewardAmount(2.0);
        buttonExtraPoints.gameObject.SetActive(false);
    }

    private void IncreseRewardAmount(double amount)
    {
        rewardVideoAmount = amount;
        string coinsStrings = textMoney.text.Substring(1,textMoney.text.Length-1);
        print(textMoney.text+"_"+coinsStrings);
        int initValue = int.Parse(coinsStrings);
        int finalValue = initValue*(int)rewardVideoAmount;
        StartCoroutine(ShowIncresingCoins(initValue,finalValue));
    }

    IEnumerator ShowIncresingCoins(int initValue, int finalValue) 
    {
        int baseValue = initValue;
        int diference = finalValue - initValue;
        int amountPerIteration = (int)(diference / 24);
        while (baseValue < finalValue)
        {
            baseValue +=amountPerIteration;
            textMoney.text = "+" + baseValue;
            yield return new WaitForSeconds(0.02f);
        }
        textMoney.text = "+" + finalValue;
    }

    private void NewSkin()
    {
        textCongratulatio.text = "New Marble";
        MarbleData newMarbl;
        if (dataManager.GetItemUnlockedCount() < allMarbles.GetLengthList() - 1)
        { 
            newMarbl = allMarbles.GetSpecificMarble(dataManager.GetNextUnlockedItem());
        }
        else
        { 
            textCongratulatio.text = "All the marbles \n has been unlocked";
             newMarbl = allMarbles.GetSpecificMarble(0);
        }
        marbleShow.SetMarbleSettings(newMarbl);
        AIUpdateStats();
    }

    void ResetLeague()
    {
        if (isNewSkin)
        {
            dataManager.IncreaseItemUnlocked();
            dataManager.SetSpecificKeyInt(KeyStorage.MARBLEPERCENTAGE_I,0);
        }
        else
        {
            MoneyManager.Transact(GetCoins());
            PlayerPrefs.SetInt(KeyStorage.GIFT_CLAIMED_I,0);
            dataManager.EraseLeague();
        }
    }
    private void CreateTrophy(Material mat = null) 
    {
        ResourceRequest trophyRequest = Resources.LoadAsync<GameObject>(_league.trophyPath);
        GameObject trophy;
        if (mat != null)
        {
            trophy = Instantiate((GameObject)trophyRequest.asset, otherPositionObj.transform);
            System.Array.ForEach(trophy.GetComponentsInChildren<Renderer>(true), i => i.material = mat);
            otherPositionObj.SetActive(true);
        }
        else
        {
            trophy = Instantiate((GameObject)trophyRequest.asset, firstPositionObj.transform);
            firstPositionObj.transform.GetChild(0).transform.GetChild(0).transform.SetParent(trophy.transform);
            firstPositionObj.SetActive(true);
        }
    }

    private void AIUpdateStats() 
    {
        PilotsStatsSetter.SetARandomPilotStats();
    } 
}
