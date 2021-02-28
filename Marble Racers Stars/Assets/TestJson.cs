using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeagueSYS;
using System.IO;

public class TestJson : MonoBehaviour
{
    void Start()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("Pilots",typeof(TextAsset));

        string path = Application.persistentDataPath + "/Pilotos.json";

        if (!File.Exists(path))
            File.WriteAllText(path, textAsset.ToString());
        else
        {
            //Pilot newPilot = new Pilot { namePilot = "Claudio Abate", driving = 2, ID = 5, team = "Devastator" };
        }
       
    }

    ListPilots GetListPilots(string jsonString) 
    {
        return Wrapper<ListPilots>.FromJsonsimple(jsonString);
    }

    void InsertPilot(Pilot newPilot, ListPilots _listPilots, string _path) 
    {
        if (_listPilots.listPilots.Exists(x => x.ID == newPilot.ID))
        {
            Debug.LogError("there is a pilot with that id ");
            return;
        }

        _listPilots.listPilots.Add(newPilot);
        File.WriteAllText(_path,Wrapper<ListPilots>.ToJsonSimple(_listPilots));
    }

    void UpdatePilot(Pilot _pilot, ListPilots _listPilots, string _path)
    {
        if (!_listPilots.listPilots.Exists(x => x.ID == _pilot.ID))
        {
            Debug.LogError("there is not pilot with that id ");
            return;
        }
        int indexList = _listPilots.listPilots.FindIndex(x => x.ID == _pilot.ID);
        _listPilots.listPilots[indexList] = _pilot;
        File.WriteAllText(_path, Wrapper<ListPilots>.ToJsonSimple(_listPilots));
    }

}
