using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeagueSYS;
using System.Threading.Tasks;
using System.Xml.Schema;

public static class LeagueManager
{
    private static League leagueRunning = null;
    
    public static League LeagueRunning 
    {
        get
        {
            if (leagueRunning == null)
            {
                League bufferLiga = new League();
                var cup_current = PlayerPrefs.GetInt(KeyStorage.CURRENTCUP_I, 0);
                bufferLiga.nameLeague = DataController.Instance.allCups.listCups[cup_current].nameLeague;
                if (IsNullLeagueData())
                {
                    bufferLiga.date = 0;
                    bufferLiga.listPrix = DataController.Instance.allCups.listCups[cup_current].listPrix;
                    bufferLiga.TeamsNumber = DataController.Instance.allCups.listCups[cup_current].TeamsNumber;
                    bufferLiga.trophyPath = DataController.Instance.allCups.listCups[cup_current].trophyPath;
                }
                else
                    bufferLiga = Wrapper<League>.FromJsonsimple(PlayerPrefs.GetString(KeyStorage.LEAGUE_S));
                leagueRunning = bufferLiga;
            }
            return leagueRunning;
        }
        private set
        {
            leagueRunning = value;
        }
    }


    public static bool IsNullLeagueData() => string.IsNullOrEmpty(PlayerPrefs.GetString(KeyStorage.LEAGUE_S,""));

    public static void CreateCompetitors(Cups allCups, MarbleDataList allMarbles) 
    {
        List<int> listMarbles = new List<int>() ;
        listMarbles = FillRandomMarbleCompetitors(allCups,allMarbles);
        //TODO BUENO lo que pude estar pasando es que me esta setiando con los ids de normi
        CreateCompetitors(allCups,listMarbles,allMarbles);
    }
    private static List<int> FillRandomMarbleCompetitors(Cups allCups, MarbleDataList allMarbles)
    {
        List<int> marbleListRandomIndex = new List<int>();
        int count = 0;
        while (count < allCups.GetCurrentLeague().GetCurrentMarbleCount()-1)
        {
            int currentRandom = Random.Range(1, allMarbles.GetLengthList());
            if (allMarbles.CheckIndexMarbleIsItem(currentRandom))
            {
                continue;
            }
            if (!marbleListRandomIndex.Contains(currentRandom) && currentRandom != 0)
            {
                marbleListRandomIndex.Add(currentRandom);
                if (LeagueRunning.GetIsPairs())
                    marbleListRandomIndex.Add(currentRandom);
                count++;
            }
        }
        marbleListRandomIndex.Add(0);
        if (LeagueRunning.GetIsPairs())
            marbleListRandomIndex.Add(0);
        return marbleListRandomIndex;
    }

    private static void CreateCompetitors(Cups allCups, List<int> marbleListRandomIndex,MarbleDataList allMarbles)
    {
        if (leagueRunning.listParticipants.Count == marbleListRandomIndex.Count)
            return;
        for (int i = 0; i < marbleListRandomIndex.Count; i++)
        {
            LeagueParticipantData par = new LeagueParticipantData();
            par.points = 0;
            par.teamName = allMarbles.GetSpecificMarble(marbleListRandomIndex[i]).nameMarble;
            if (i % 2 == 0)
                par.pilot = PilotsDataManager.Instance.SelectPilot(par.teamName, 2);
            else
                par.pilot = PilotsDataManager.Instance.SelectPilot(par.teamName, 1);
            LeagueRunning.listParticipants.Add(par);
        }
        Debug.Log("COMPEtitors");
    }

    public static void ClearLeague() 
    {
        LeagueRunning = null;
        PlayerPrefs.DeleteKey(KeyStorage.LEAGUE_S);
    }

}
