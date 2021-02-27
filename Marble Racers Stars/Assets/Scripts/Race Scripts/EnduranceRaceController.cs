using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnduranceRaceController : MonoBehaviour
{
    [SerializeField] Marble[] marblesCompeti;
    [SerializeField] Marble[] marblesCompas;
    [SerializeField] DataManager dataManag;
    [SerializeField] private TextMeshProUGUI textPlace;

    private void Awake()
    {
        DistributeMarbleData();
    }

    void Start()
    {
        Invoke("BeginRace",1f);
        RaceController.Instance.OnPlayerArrived += SavePlayerPositionEndurance;
    }

    void BeginRace()
    {
        MainMenuController.GetInstance().PrepareRace();
    }

    void SavePlayerPositionEndurance(int posPlayer)
    {
        PlayerPrefs.SetInt(KeyStorage.ENDURANCE_POSITION_I,posPlayer);
        textPlace.text = ""+posPlayer;
        RaceController.Instance.OnPlayerArrived -= SavePlayerPositionEndurance;
    }

    void DistributeMarbleData()
    {
        List<int> listParticipant = new List<int>();
        int limit = 3;// RacersSettings.GetInstance().GetMarbleDataList().marblesDataList.Count;
        while(listParticipant.Count <16)
        {
            int rando = Random.Range(1, limit);
            if (!listParticipant.Contains(rando) && dataManag.GetCurrentMarble() != rando)
            {
                listParticipant.Add(rando);
            }
        }

        for (int i = 0; i < marblesCompeti.Length; i++)
        {
            if (marblesCompeti[i].isPlayer)
            {
                //marblesCompeti[i].SetMarbleSettings(RacersSettings.GetInstance().GetMarbleDataList().marblesDataList[dataManag.GetCurrentMarble()]);
            }
            else
            {
                //marblesCompeti[i].SetMarbleSettings(RacersSettings.GetInstance().GetMarbleDataList().marblesDataList[listParticipant[i]]);
            }

            if (marblesCompas[i].isPlayer)
            {
                //marblesCompas[i].SetMarbleSettings(RacersSettings.GetInstance().GetMarbleDataList().marblesDataList[dataManag.GetCurrentMarble()]);
            }
            else
            {
                //marblesCompas[i].SetMarbleSettings(RacersSettings.GetInstance().GetMarbleDataList().marblesDataList[listParticipant[i]]);
            }
        }
    }
}
