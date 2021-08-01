using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeagueSYS;
using MyBox;


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
    public TracksInfo DefaultTrack() => listCups[0].listPrix[0].trackInfo;
    public TracksInfo NextRace()
    {
        TracksInfo scene = DefaultTrack();
        if (LeagueManager.IsNullLeagueData())
        {
            scene = listCups[PlayerPrefs.GetInt(KeyStorage.CURRENTCUP_I)].listPrix[0].trackInfo;
            Debug.Log("Liga nula " + scene);
        }
        else
        {
            League liga = Wrapper<League>.FromJsonsimple(PlayerPrefs.GetString(KeyStorage.LEAGUE_S));
            if (liga.date < liga.listPrix.Count)
                scene = liga.listPrix[liga.date].trackInfo;
            else
                scene = liga.listPrix[0].trackInfo;
        }
        return scene;
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

    [ButtonMethod]
    private void ReverseCups() 
    {
        listCups.Reverse();
    }

}
