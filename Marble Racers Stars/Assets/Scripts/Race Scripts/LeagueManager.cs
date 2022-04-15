using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeagueSYS;
using System.Threading.Tasks;

public static class LeagueManager
{
    private static League leagueRunning = null;
    private static League leagueManufacturers = null;
    
    public static League LeagueRunning 
    {
        get
        {
            if (leagueRunning == null)
            {
                League bufferLiga = new League();
                bufferLiga.nameLeague = "Pilots Cups";
                if (IsNullLeagueData())
                {
                    Debug.Log("Nula desde la raiz");
                    bufferLiga.date = 0;
                    bufferLiga.listPrix = DataController.Instance.allCups.listCups[PlayerPrefs.GetInt(KeyStorage.CURRENTCUP_I, 0)].listPrix;
                    bufferLiga.Teams = DataController.Instance.allCups.listCups[PlayerPrefs.GetInt(KeyStorage.CURRENTCUP_I, 0)].Teams;
                    bufferLiga.trophyPath = DataController.Instance.allCups.listCups[PlayerPrefs.GetInt(KeyStorage.CURRENTCUP_I, 0)].trophyPath;
                    CreateCompetitors(DataController.Instance.allCups, DataController.Instance.allMarbles);
                }
                else
                {
                    bufferLiga = Wrapper<League>.FromJsonsimple(PlayerPrefs.GetString(KeyStorage.LEAGUE_S));
                    Debug.Log("se su pone que exitse y la copia");
                }
                SaveLeague();
                leagueRunning = bufferLiga;
            }
            return leagueRunning;
        }
        set
        {
            leagueRunning = value;
        }
    }

    public static League LeagueManufacturers
    {
        get
        {
            if (leagueManufacturers == null)
            {
                League bufferLiga = new League();
                bufferLiga.nameLeague = "Manufacturers Cups";

                if (string.IsNullOrEmpty(PlayerPrefs.GetString(KeyStorage.LEAGUE_MANUFACTURERS_S)))
                {
                    bufferLiga.date = 0;
                    bufferLiga.listPrix = DataController.Instance.allCups.listCups[PlayerPrefs.GetInt(KeyStorage.CURRENTCUP_I, 0)].listPrix;
                    bufferLiga.Teams = DataController.Instance.allCups.listCups[PlayerPrefs.GetInt(KeyStorage.CURRENTCUP_I, 0)].Teams;
                }
                else
                {
                    bufferLiga = Wrapper<League>.FromJsonsimple(PlayerPrefs.GetString(KeyStorage.LEAGUE_MANUFACTURERS_S));
                }
                SaveLeague();
                leagueManufacturers = bufferLiga;
            }
            return leagueManufacturers;
        }
        set
        {
            leagueManufacturers = value;
        }
    }

    public static bool IsNullLeagueData() => string.IsNullOrEmpty(PlayerPrefs.GetString(KeyStorage.LEAGUE_S,""));
    public static bool IsNullManufacturersData() => string.IsNullOrEmpty(PlayerPrefs.GetString(KeyStorage.LEAGUE_MANUFACTURERS_S,""));
    private static League DefaultLeague(Cups allCups)
    {
        League bufferLiga = new League();
        bufferLiga.nameLeague = "Pilots Cups";
        Debug.Log("Default");
        if (IsNullLeagueData())
        {
            bufferLiga.date = 0;
            bufferLiga.listPrix = allCups.listCups[PlayerPrefs.GetInt(KeyStorage.CURRENTCUP_I, 0)].listPrix;
            bufferLiga.Teams = allCups.listCups[PlayerPrefs.GetInt(KeyStorage.CURRENTCUP_I, 0)].Teams;
            bufferLiga.trophyPath = DataController.Instance.allCups.listCups[PlayerPrefs.GetInt(KeyStorage.CURRENTCUP_I, 0)].trophyPath;
        }
        else
        {
            bufferLiga = Wrapper<League>.FromJsonsimple(PlayerPrefs.GetString(KeyStorage.LEAGUE_S));
            Debug.Log("Soy Default pero existo");
        }
        return bufferLiga;
    }

    public static void CreateCompetitors(Cups allCups, MarbleDataList allMarbles) 
    {
        //if (!IsNullLeagueData()) return;
        //LeagueRunning = DefaultLeague(allCups);
        //LeagueManufacturers= DefaultLeague(allCups);
        List<int> listMarbles = new List<int>() ;
        listMarbles = FillRandomMarbleCompetitors(allCups,allMarbles);
        CreateCompetitors(allCups,listMarbles,allMarbles);
        CreateTeams();
        SaveLeague();
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
                //print("Continuo porque el "+ currentRandom+" is a item");
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
        for (int i = 0; i < marbleListRandomIndex.Count; i++)
        {
            LeagueParticipantData par = new LeagueParticipantData();
            par.points = 0;
            par.teamName = allMarbles.GetSpecificMarble(marbleListRandomIndex[i]).nameMarble;
            if (i % 2 == 0)
            {
                par.pilot = PilotsDataManager.Instance.SelectPilot(par.teamName, 2);
            }
            else
                par.pilot = PilotsDataManager.Instance.SelectPilot(par.teamName, 1);
            LeagueRunning.listParticipants.Add(par);
        }
    }
    private static void CreateTeams()
    {
        foreach (var item in LeagueRunning.listParticipants)
            if (!LeagueManufacturers.listParticipants.Exists(x => x.teamName == item.teamName))
                LeagueManufacturers.listParticipants.Add(item);
    }
    public static void SaveLeague()
    {
        string jSaved = Wrapper<League>.ToJsonSimple(leagueRunning);
        PlayerPrefs.SetString(KeyStorage.LEAGUE_S, jSaved);
        string jManufacturers = Wrapper<League>.ToJsonSimple(leagueManufacturers);
        PlayerPrefs.SetString(KeyStorage.LEAGUE_MANUFACTURERS_S, jManufacturers);
    }
}
