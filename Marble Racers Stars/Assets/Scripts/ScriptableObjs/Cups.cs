using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeagueSYS;


[CreateAssetMenu(fileName = "New_Cups", menuName = "Inventory/Cups")]
public class Cups : ScriptableObject
{
    public List<League> listCups = new List<League>();

    private void OnValidate()
    {
        listCups.ForEach(x => { if (!x.requerimentsLeague.needSecondPilot && x.listPrix.Find(i => i.twoPilots) != null)
                Debug.LogError("OJO QUE HAY UNA CARRERA QUE ES DE DOS PERO NO PIDE EL SEGUNDO PILOTO");
        });
    }

    public string NextRace()
    {
        string nameScene = "(T)Hut On The Hill";
        if (string.IsNullOrEmpty(PlayerPrefs.GetString(KeyStorage.LEAGUE_S)))
        {
            nameScene = listCups[PlayerPrefs.GetInt(KeyStorage.CURRENTCUP_I)].listPrix[0].trackInfo.NameTrack;
            Debug.Log("Liga nula"+ nameScene);
        }
        else
        {
            League liga = Wrapper<League>.FromJsonsimple(PlayerPrefs.GetString(KeyStorage.LEAGUE_S));
            if (liga.date < liga.listPrix.Count)
                nameScene = liga.listPrix[liga.date].trackInfo.NameTrack;
            else
                nameScene = liga.listPrix[0].trackInfo.NameTrack;
        }
        return nameScene;
    }

    public int GetIndexLeagueByName(string _nameLeague)
    {
        int argReturn = 0;
        foreach (var item in listCups)
        {
            if (item.nameLeague.Equals(_nameLeague))
            {
                argReturn = listCups.IndexOf(item);
                break;
            }
        }
        return argReturn;
    }

    public League GetCurrentLeague() 
    {
        return listCups[PlayerPrefs.GetInt(KeyStorage.CURRENTCUP_I)];
    }
}
