using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeagueSYS;

public static class CurrentLeague 
{
    public static event System.Action<League> onLeagueSet;
    private static League leagueRunning;
    private static League leagueManufacturers;
    public static League LeagueRunning { get { return leagueRunning; } 
        set { leagueRunning = value; onLeagueSet?.Invoke(leagueRunning);} }
    public static League LeagueManufacturers
    {
        get { return leagueManufacturers; }
        set { leagueManufacturers = value;}
    }

    public static League DefaultLiga(Cups allCups)
    {      
        League bufferLiga = new League();
        bufferLiga.nameLeague = "Pilots Cups";

        if (string.IsNullOrEmpty(PlayerPrefs.GetString(KeyStorage.LEAGUE_S)))
        {
            bufferLiga.date = 0;
            bufferLiga.listPrix = allCups.listCups[PlayerPrefs.GetInt(KeyStorage.CURRENTCUP_I, 0)].listPrix;
            bufferLiga.Teams = allCups.listCups[PlayerPrefs.GetInt(KeyStorage.CURRENTCUP_I, 0)].Teams;
        }
        else
        {
            bufferLiga = Wrapper<League>.FromJsonsimple(PlayerPrefs.GetString(KeyStorage.LEAGUE_S));
        }
        return bufferLiga;
    }

}
