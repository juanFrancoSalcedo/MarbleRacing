using UnityEngine;

namespace LeagueSYS
{
    public class LeagueSaver
    {
        public void SaveLeague()
        {
            string jSaved = Wrapper<League>.ToJsonSimple(LeagueManager.LeagueRunning);
            PlayerPrefs.SetString(KeyStorage.LEAGUE_S, jSaved);
        }
    }
}



