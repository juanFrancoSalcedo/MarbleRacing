namespace LeagueSYS
{
    [System.Serializable]
    public class LeagueParticipantData: ICopy<LeagueParticipantData>
    {
        public string teamName;
        public int points;
        public int lastPosition;
        public Pilot pilot;

        public LeagueParticipantData Copy() => (LeagueParticipantData)this.MemberwiseClone();
    }
}


