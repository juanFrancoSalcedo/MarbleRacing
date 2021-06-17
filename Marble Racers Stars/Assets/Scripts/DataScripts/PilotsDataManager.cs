using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeagueSYS;
using System.IO;
using System;
using MyBox;

public class PilotsDataManager : Singleton<PilotsDataManager>
{
    private ListPilots listPilots;
    public ListPilots GetListPilots () => listPilots ?? SetListPilots();

    private string path 
    {
        get 
        {
            string bufferPath = Application.persistentDataPath + "/Pilotos.json";
            TextAsset textAsset = (TextAsset)Resources.Load("Pilots",typeof(TextAsset));
            if (!File.Exists(bufferPath))
            {
                File.WriteAllText(bufferPath, Encrypt(textAsset.ToString()));
            }
            return Application.persistentDataPath + "/Pilotos.json"; 
        }
        set {}
    }

    private string Encrypt(string textToEncrypt) 
    {
        byte[] encrypted = System.Text.Encoding.Unicode.GetBytes(textToEncrypt);
        return Convert.ToBase64String(encrypted);
    }
    private string Decrypt(string textToDecrypt) 
    {
        byte[] decrypted = Convert.FromBase64String(textToDecrypt);
        return System.Text.Encoding.Unicode.GetString(decrypted);
    }
    private ListPilots SetListPilots()
    {
        if(listPilots!= null)
            return listPilots;
        string jsonString = Decrypt(File.ReadAllText(path));
        listPilots = Wrapper<ListPilots>.FromJsonsimple(jsonString);
        return listPilots;
    }

    void InsertPilot(Pilot newPilot, ListPilots _listPilots) 
    {
        if (_listPilots.listPilots.Exists(x => x.ID == newPilot.ID))
        {
            Debug.LogError("there is a pilot with that id ");
            return;
        }

        _listPilots.listPilots.Add(newPilot);
        File.WriteAllText(path,Wrapper<ListPilots>.ToJsonSimple(_listPilots));
    }

    public void UpdatePilot(Pilot _pilot)
    {
        if (!listPilots.listPilots.Exists(x => x.ID == _pilot.ID))
        {
            Debug.LogError("there is not pilot with that id ");
            return;
        }
        int indexList = listPilots.listPilots.FindIndex(x => x.ID == _pilot.ID);
        listPilots.listPilots[indexList] = _pilot;
        File.WriteAllText(path,Encrypt(Wrapper<ListPilots>.ToJsonSimple(listPilots)));
        if (RacersSettings.GetInstance()!= null && RacersSettings.GetInstance().leagueManager.Liga.listParticipants.Exists(x => x.teamName == _pilot.team)) 
        {
            RacersSettings.GetInstance().leagueManager.Liga.listParticipants.Find(x => x.teamName == _pilot.team).pilot = _pilot;
            RacersSettings.GetInstance().leagueManager.SaveLeague();
            print("Auch");
        }
        SetListPilots();
    }

    public Pilot SelectPilot(string team, int idColor)
    {
        int indexList = GetListPilots().listPilots.FindIndex(x => x.team == team && x.colorPilotId == idColor);
        return GetListPilots().listPilots[indexList];
    }

    public Pilot SelectPilot(int idPilot)
    {
        return GetListPilots().listPilots.Find(x => x.ID == idPilot);
    }
}
