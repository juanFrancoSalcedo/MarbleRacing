using System.Collections;
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
    [SerializeField] private DataManager dataManager = null;
    League _league;
    [ConditionalField(nameof(isNewSkin), true)] [SerializeField] TextMeshProUGUI textMoney = null;
    [SerializeField] TextMeshProUGUI textCongratulatio = null;
    [ConditionalField(nameof(isNewSkin), true)] [SerializeField] TextMeshProUGUI textPosition = null;
    [ConditionalField(nameof(isNewSkin), true)] [SerializeField] GameObject firstPositionObj = null;
    [ConditionalField(nameof(isNewSkin), true)] [SerializeField] GameObject otherPositionObj = null;
    [ConditionalField(nameof(isNewSkin))] [SerializeField] MarbleDataList allMarbles = null;
    [ConditionalField(nameof(isNewSkin))] [SerializeField] Marble marbleShow = null;

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
        int coins = Constants.pointsPerRacePosition[indexParticipantPlayer] * dataManager.allCups.GetCurrentLeague().multiplierMoney;

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
        MoneyManager.Transact(coins);
        AIUpdateStats();
    }

    private void NewSkin()
    {
        textCongratulatio.text = "New Marble";
        MarbleData newMarbl;
        if (dataManager.GetItemUnlockedCount() < allMarbles.GetLengthList() - 1)
            newMarbl = allMarbles.GetSpecificMarble((dataManager.GetNextUnlockedItem())+1);
        else
        { 
            textCongratulatio.text = "All the marbles 1 \n has been unlocked";
             newMarbl = allMarbles.GetSpecificMarble((dataManager.GetNextUnlockedItem()-1));
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
            PlayerPrefs.SetInt(KeyStorage.GIFT_CLAIMED_I,0);
            dataManager.EraseLeague();
        }
    }

    private void AIUpdateStats() => PilotsStatsSetter.SetARandomPilotStats();
}
