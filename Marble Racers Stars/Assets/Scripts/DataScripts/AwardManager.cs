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
        buttonLoad.onClick.AddListener(ResetLeague);
        _league = Wrapper<League>.FromJsonsimple(PlayerPrefs.GetString(KeyStorage.LEAGUE));

        if (_league == null)
        {
            Debug.LogError("al perecer se filtro una liga vacia");
        }

        if (isNewSkin)
        {
            NewSkin();
        }
        else
        {
            if (PlayerPrefs.GetInt(KeyStorage.ENDURANCE_POSITION_I, 0) != 0)
            {
                Invoke("EnduranceAward", 0.2f);
            }
            else
            {
                Invoke("SearchPlayerMarble", 0.2f);
            }
        }
    }

    private void EnduranceAward()
    {
        int playerPosition = PlayerPrefs.GetInt(KeyStorage.ENDURANCE_POSITION_I);
        playerPosition = (playerPosition > 12)? 12:playerPosition;
        int coins = Constants.pointsPerRacePosition[(playerPosition-1)] * 10;
        dataManager.SetTransactionMoney(coins);

        if (playerPosition == 1)
        {
            textCongratulatio.text = "Congratulations";
            textPosition.text = "1";
            textMoney.text = "+" + coins;
            firstPositionObj.SetActive(true);
        }
        else
        {
            textCongratulatio.text = "it wasn't enough";
            textPosition.text = "" + (playerPosition);
            textMoney.text = "+" + coins;
            otherPositionObj.SetActive(true);
        }
    }

    void SearchPlayerMarble()
    {
        List<LeagueParticipantData> sortedParti = new List<LeagueParticipantData>();
        sortedParti = _league.listParticipants;
        //print(_league.listParticipants.Count+" abstr");
        int playerPosition  =11;

        for (int i = 0; i < _league.listParticipants.Count - 1; i++)
        {
            for (int j = 0; j < _league.listParticipants.Count - 1; j++)
            {
                    if (_league.listParticipants[j].points < _league.listParticipants[j + 1].points)
                    {
                        LeagueParticipantData buffer = sortedParti[j + 1];
                        sortedParti[j + 1] = sortedParti[j];
                        sortedParti[j] = buffer;
                    }
            }
        }
        
        for (int i = 0; i < sortedParti.Count; i++)
        {
            if (sortedParti[i].participantName.Equals("Normi"))
            {
                playerPosition = i;
            }
        }

        int coins = Constants.pointsPerRacePosition[playerPosition] * 10;

        if (playerPosition == 0)
        {
            textCongratulatio.text = "Congratulations";
            textPosition.text = "1";
            textMoney.text = "+"+coins;
            firstPositionObj.SetActive(true);
            dataManager.WinTrophy(1);

            if (dataManager.GetSpecificKeyInt(KeyStorage.CURRENTCUP_I) > dataManager.GetCupsWon())
            {
                dataManager.WinCup();
            }
        }
        else
        {
            textCongratulatio.text = "it wasn't enough";
            textPosition.text = ""+(playerPosition+1);
            textMoney.text = "+" + coins;
            otherPositionObj.SetActive(true);
        }

        dataManager.SetTransactionMoney(coins);
    }

    private void NewSkin()
    {
        textCongratulatio.text = "New Marble";
        MarbleData newMarbl;
        if (dataManager.GetMarbleUnlocked() < allMarbles.GetLengthList() - 1)
            newMarbl = allMarbles.GetSpecificMarble((dataManager.GetMarbleUnlocked()+1));
        else
            newMarbl = allMarbles.GetSpecificMarble((dataManager.GetMarbleUnlocked()));
        marbleShow.SetMarbleSettings(newMarbl);
    }

    void ResetLeague()
    {
        if (isNewSkin)
        {
            dataManager.IncreaseMarbleUnlocked();
            dataManager.SetSpecificKeyInt(KeyStorage.MARBLEPERCENTAGE_I,0);
        }
        else
        {
            if (PlayerPrefs.GetInt(KeyStorage.ENDURANCE_POSITION_I, 0) == 0)
                dataManager.EraseLeague();
            else
            {
                if (PlayerPrefs.GetInt(KeyStorage.ENDURANCE_POSITION_I, 0) == 1)
                {
                    dataManager.WinTrophy(1);
                }
                PlayerPrefs.SetInt(KeyStorage.ENDURANCE_POSITION_I, 0);
            }
        }
    }
}
