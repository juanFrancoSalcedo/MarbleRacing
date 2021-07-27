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
        public int multiplierMoney = 2;
        public string trophyPath;
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
        public bool GetIsPairs() => listPrix[NormalizedDate()].twoPilots;
        public int GetMarblesToQualifying() => listPrix[NormalizedDate()].marblesLessToQualy;
        public bool GetIsQualifying() => listPrix[NormalizedDate()].isQualifying;
        public bool GetUsingWear() => listPrix[NormalizedDate()].wear>0;
        public float GetFriction() => listPrix[NormalizedDate()].wear;
        int NormalizedDate() 
        {
            return (date >= listPrix.Count) ? listPrix.Count - 1 : date;
        }

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

        public int GetScoresByPilot(string pilotName) 
        {
            return listParticipants.Find(x => x.pilot.namePilot == pilotName).points;
        }
        public int GetScoresByPilot(int pilotId)
        {
            return listParticipants.Find(x => x.pilot.ID == pilotId).points;
        }
        public int GetScoresByTeam(string team)
        {
            int sum =0;
            listParticipants.ForEach(x => { sum += (x.teamName == team) ? x.points : 0;});
            return sum;
        }

        public int GetPositionInChampionship(int idPilot) 
        {
            return listParticipants.IndexOf(listParticipants.Find(x=> x.pilot.ID == idPilot));
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
        [ConditionalField(nameof(usePowers))] public bool useAllPows = false;
        [ConditionalField(nameof(useAllPows),true)] public PowerUpType singlePow;
        public bool isQualifying;
        [ConditionalField(nameof(isQualifying))] public int marblesLessToQualy = 3;
        public bool twoPilots;
        [Range(0,10)]public float wear =0;
    }


    [System.Serializable]
    public struct CupRequeriments
    {
        public int moneyRequeriments;
        public int trophiesRequeriments;
        public string nameCupPreviousRequeriments;
        public bool needSecondPilot;
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
        public MarbleStats stats;
    }
    [System.Serializable]
    public class MarbleStats 
    {
        public float forceTurbo = 0.36f;
        public float forceDirection = 0.18f;
        public float coldTimeTurbo;
        public float coldTimeDirection;
        public int hp = 54;
    }
}


