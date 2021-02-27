using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeagueSYS;

public static class CurrentLeague 
{
    public static event System.Action<League> onLeagueSet;
    private static League leagueRunning;
    public static League LeagueRunning { get { return leagueRunning; } 
        set { leagueRunning = value; onLeagueSet?.Invoke(leagueRunning);} }

    public static void CreateRandomLeague() 
    {
        List<int> marbleListRandomIndex = new List<int>();
    }
}
