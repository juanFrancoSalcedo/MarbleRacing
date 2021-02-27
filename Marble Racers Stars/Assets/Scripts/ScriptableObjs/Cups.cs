using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeagueSYS;


[CreateAssetMenu(fileName = "New_Cups", menuName = "Inventory/Cups")]
public class Cups : ScriptableObject
{
    public List<League> listCups = new List<League>();

    public string NextRace()
    {
        string nameScene = "(T)Hut On The Hill";

        if (string.IsNullOrEmpty(PlayerPrefs.GetString(KeyStorage.LEAGUE)))
        {
            nameScene = listCups[PlayerPrefs.GetInt(KeyStorage.CURRENTCUP_I)].listPrix[0].trackInfo.NameTrack;
            Debug.Log("Liga nula"+ nameScene);
        }
        else
        {
            League liga = Wrapper<League>.FromJsonsimple(PlayerPrefs.GetString(KeyStorage.LEAGUE));

            if (liga.date < liga.listPrix.Count)
            {
                nameScene = liga.listPrix[liga.date].trackInfo.NameTrack;
            }
            else
            {
                nameScene = liga.listPrix[0].trackInfo.NameTrack;
                //PlayerPrefs.SetInt(KeyStorage.LEAGUESTARTED_I, 0);
            }
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

}
