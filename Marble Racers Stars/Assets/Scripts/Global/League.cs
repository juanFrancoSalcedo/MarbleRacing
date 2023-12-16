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
        public int TeamsNumber;
        public List<GrandPrix> listPrix = new List<GrandPrix>();
        public CupRequeriments requerimentsLeague;
        public int GetCurrentMarbleCount() 
        {
            if (GetLastTrackWasQualy() && !GetIsQualifying()) 
            {
                if (!listPrix[date - 1].twoPilots && listPrix[date].twoPilots)
                {
                    Debug.LogError("POSIBLEMENTE EXISTIRÁ UN PROBLEMA");
                    return (TeamsNumber - listPrix[date - 1].marblesLessToQualy);
                }

                return TeamsNumber*(listPrix[date].twoPilots ? 2:1) - listPrix[date - 1].marblesLessToQualy;
            }
            else 
            {
                return TeamsNumber;
            } 
        }
        public bool GetIsPairs() => listPrix[NormalizedDate()].twoPilots;
        public int GetMarblesToQualifying() => listPrix[NormalizedDate()].marblesLessToQualy;
        public bool GetIsQualifying() => listPrix[NormalizedDate()].isQualifying;
        /// <summary>
        /// If the race use pits
        /// </summary>
        /// <returns></returns>
        public bool GetUsingWear() => listPrix[NormalizedDate()].wear>0;
        public float GetFriction() => listPrix[NormalizedDate()].wear;
        int NormalizedDate() 
        {
            return (date >= listPrix.Count) ? listPrix.Count - 1 : date;
        }

        public LeagueParticipantData CopyDataParticipant(int index) 
        {
            return listParticipants[index].Copy();
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

        public int GetScoresByPilot(string pilotName) => listParticipants.Find(x => x.pilot.namePilot == pilotName).points;
        public int GetScoresByPilot(int pilotId) => listParticipants.Find(x => x.pilot.ID == pilotId).points;
        public int GetScoresByTeam(string team)
        {
            int sum =0;
            listParticipants.ForEach(x => { sum += (x.teamName == team) ? x.points : 0;});
            return sum;
        }

        public int GetPositionInChampionship(int idPilot) => listParticipants.IndexOf(listParticipants.Find(x => x.pilot.ID == idPilot));
    }
}


