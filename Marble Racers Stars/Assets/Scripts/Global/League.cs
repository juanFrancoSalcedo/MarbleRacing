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
        public List<GrandPrix> listPrix = new List<GrandPrix>();
        public CupRequeriments requerimentsLeague;
        public int GetCurrentMarbleCount() => listPrix[date].marblesCount;
        public int GetMarblesToQualifying() => listPrix[date].marblesLessToQualy;
        public bool GetIsQualifying() => listPrix[date].isQualifying;
    }

    [System.Serializable]
    public class LeagueParticipantData
    {
        public string participantName;
        public int points;
        public int lastPosition;
    }

    [System.Serializable]
    public class GrandPrix
    {
        public TracksInfo trackInfo;
        public int laps = 1;
        public int marblesCount = 12;
        public bool usePowers;
        public bool isQualifying;
        [ConditionalField(nameof(isQualifying))]
        public int marblesLessToQualy = 3;
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
        public int ID;
    }
}

