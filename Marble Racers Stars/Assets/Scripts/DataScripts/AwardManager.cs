using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeagueSYS;
using TMPro;
using MyBox;
using UnityEngine.UI;

public class AwardManager : MonoBehaviour
{
    [SerializeField] bool isNewSkin;
    [SerializeField] private Button buttonLoad;
    [SerializeField] private DataManager dataManager;
    League _league;
    [ConditionalField(nameof(isNewSkin), true)] [SerializeField] TextMeshProUGUI textMoney;
    [SerializeField] TextMeshProUGUI textCongratulatio;
    [ConditionalField(nameof(isNewSkin), true)] [SerializeField] TextMeshProUGUI textPosition;
    [ConditionalField(nameof(isNewSkin), true)] [SerializeField] GameObject firstPositionObj;
    [ConditionalField(nameof(isNewSkin), true)] [SerializeField] GameObject otherPositionObj;
    [ConditionalField(nameof(isNewSkin))] [SerializeField] MarbleDataList allMarbles;
    [ConditionalField(nameof(isNewSkin))] [SerializeField] Marble marbleShow;

    void Start()
    {
        Time.timeScale = 1;
        buttonLoad.onClick.AddListener(ResetLeague);
        _league = Wrapper<League>.FromJsonsimple(PlayerPrefs.GetString(KeyStorage.LEAGUE_MANUFACTURERS_S));

        if (_league == null)
            Debug.LogError("al perecer se filtro una liga vacia");

        if (isNewSkin)
            NewSkin();
        else
            Invoke("SearchPlayerMarble", 0.2f);   
    }
    void SearchPlayerMarble()
    {
        List<LeagueParticipantData> sortedParti = new List<LeagueParticipantData>();
        sortedParti = _league.listParticipants;
        int playerPosition  =11;
        BubbleSort<LeagueParticipantData>.SortReverse(sortedParti, "points");
        playerPosition = sortedParti.FindIndex(0, sortedParti.Count, x => x.teamName == Constants.NORMI);
        ShowAward(playerPosition);
    }

    private void ShowAward(int indexParticipantPlayer) 
    {
        int coins = Constants.pointsPerRacePosition[indexParticipantPlayer] * 10;

        if (indexParticipantPlayer == 0)
        {
            textCongratulatio.text = "Congratulations";
            textPosition.text = "1";
            textMoney.text = "+" + coins;
            firstPositionObj.SetActive(true);
            dataManager.WinTrophy(1);

            if (dataManager.GetSpecificKeyInt(KeyStorage.CURRENTCUP_I) > dataManager.GetCupsWon())
                dataManager.WinCup();
        }
        else
        {
            textCongratulatio.text = "it wasn't enough";
            textPosition.text = "" + (indexParticipantPlayer + 1);
            textMoney.text = "+" + coins;
            otherPositionObj.SetActive(true);
        }
        dataManager.SetTransactionMoney(coins);
    }

    private void NewSkin()
    {
        textCongratulatio.text = "New Marble";
        MarbleData newMarbl;
        if (dataManager.GetItemUnlockedCount() < allMarbles.GetLengthList() - 1)
            newMarbl = allMarbles.GetSpecificMarble((dataManager.GetUnlockedItem()));
        else
            newMarbl = allMarbles.GetSpecificMarble((dataManager.GetUnlockedItem()-1));
        marbleShow.SetMarbleSettings(newMarbl);
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
            PlayerPrefs.SetInt(KeyStorage.GIFT_CLAIMED_I,0);
            dataManager.EraseLeague();
        }
    }
}
