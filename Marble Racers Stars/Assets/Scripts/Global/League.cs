using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;


namespace LeagueSYS
{
    [System.Serializable]
    public class League
    {
        public string nameLeague;
        [HideInInspector]
        public int date;
        [HideInInspector]
        public List<LeagueParticipantData> listParticipants = new List<LeagueParticipantData>();
        public int Teams;
        public List<GrandPrix> listPrix = new List<GrandPrix>();
        public CupRequeriments requerimentsLeague;
        public int GetCurrentMarbleCount() 
        {
            if (GetLastTrackWasQualy() && !GetIsQualifying()) 
            {
                if (!listPrix[date - 1].twoPilots && listPrix[date].twoPilots)
                {
                    Debug.LogError("POSIBLEMENTE EXISTIRÁ UN PROBLEMA");
                    return (Teams - listPrix[date - 1].marblesLessToQualy);
                }

                return Teams*(listPrix[date].twoPilots ? 2:1) - listPrix[date - 1].marblesLessToQualy;
            }
            else 
            {
                return Teams;
            } 
        }
        public bool GetIsPairs() => listPrix[date].twoPilots;
        public int GetMarblesToQualifying() => listPrix[date].marblesLessToQualy;
        public bool GetIsQualifying() => listPrix[date].isQualifying;

        private bool GetLastTrackWasPairs()
        {
            if (date == 0)
                return false;
            if (listPrix[date - 1].twoPilots)
                return true;
            else
                return false;
        }
        private bool GetLastTrackWasQualy() 
        {
            if (date == 0) 
                return false;
            if (listPrix[date - 1].isQualifying)
                return true;
            else
                return false;
        }
    }

    [System.Serializable]
    public class LeagueParticipantData
    {
        public string teamName;
        public int points;
        public int lastPosition;
        public Pilot pilot;
    }

    [System.Serializable]
    public class GrandPrix
    {
        public TracksInfo trackInfo;
        public int laps = 1;
        public bool usePowers;
        public bool isQualifying;
        [ConditionalField(nameof(isQualifying))] public int marblesLessToQualy = 3;
        public bool twoPilots;
    }

    [System.Serializable]
    public struct CupRequeriments
    {
        public int moneyRequeriments;
        public int trophiesRequeriments;
        public string nameCupPreviousRequeriments;
    }

    [System.Serializable]
    public class ListPilots
    {
        public List<Pilot> listPilots = new List<Pilot>();
    }

    [System.Serializable]
    public struct Pilot
    {
        public string namePilot;
        public string team;
        public int driving;
        public int colorPilotId;
        public int ID;
    }
}

