using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Board))]
public class RacerBoardPositions : MonoBehaviour, IRacerSettingsRegistrable
{
    [SerializeField] private BoardUIController boardPrefab = null;
    [SerializeField] private bool overrideUiController = false;
    [SerializeField] private bool isManufacturers = false;

    Board board;
    void Awake() 
    {
        board = GetComponent<Board>();
        SubscribeRacerSettings();
    }

#region IRaceSettings Methods
    public void SubscribeRacerSettings()
    {
        if (!RacersSettings.GetInstance().filled)
            RacersSettings.GetInstance().onListFilled += FillMyMarbles;
        else
            FillMyMarbles(RacersSettings.GetInstance().GetMarbles());
    }

    public void FillMyMarbles(List<Marble> marblesObteined)
    {
        board.DeleteAllParticipants();
        board.participantScores = new BoardParticipant[(isManufacturers) ? RacersSettings.GetInstance().leagueManager.Liga.Teams : marblesObteined.Count];
        //board.participantScores= new BoardParticipant[marblesObteined.Count];
        board.ResetParticipantSorted();

        for (int i = 0; i < ((isManufacturers)?RacersSettings.GetInstance().leagueManager.Liga.Teams: marblesObteined.Count); i++)
        //for (int i = 0; i < (marblesObteined.Count); i++)
        {
            BoardUIController boarInstance = Instantiate(boardPrefab, board.transform);
            board.participantScores[i] = boarInstance.BoardParticip;
            if (overrideUiController)
                marblesObteined[i].boardController = boarInstance;
        }
    }
    #endregion

}
