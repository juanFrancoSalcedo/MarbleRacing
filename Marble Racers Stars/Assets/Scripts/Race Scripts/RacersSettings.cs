using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeagueSYS;
using System.Threading.Tasks;
using System.IO;

public class RacersSettings : MonoBehaviour
{
    private static RacersSettings Instance;
    public MarbleDataList allMarbles;
    public int competitorsLength = 12;
    public GameObject marblePrefab;
    public Transform[] startersPositions;
    private List<Marble> listMarbles = new List<Marble>();
    public System.Func<MarbleDataList> OnListLoaded;
    public event System.Action<List<Marble>> onListFilled;
    public bool filled { get; set;}

    public bool Broadcasting() => pathBroadcast.Contains("true");
    // NO SE PUEDE DEW QUALY A TWO PILOTs ParA RECORADRLE Solo de two pilots a qualy

    public static RacersSettings GetInstance()
    {
        if (Instance == null)
        {
            Instance = GameObject.FindObjectOfType<RacersSettings>();
        }
        else
        {
            RacersSettings[] raceSetti = GameObject.FindObjectsOfType<RacersSettings>();
            if (raceSetti.Length > 1)
            {
                Debug.LogError("THERE ARE TWO RACE SETTINGS");
            }
        }
        return Instance;
    }
    private  void  Awake()
    {
        FillMarbles();
        RaceBroadcastSettings();
    }

    private void RaceBroadcastSettings() 
    {
        if (pathBroadcast.Equals("true"))
            CameraSettings.Instance.gameObject.SetActive(false);
        else
            BSpecManager.Instance.gameObject.SetActive(false);
    }

    private string pathBroadcast
    {
        get
        {
            TextAsset textAsset = (TextAsset)Resources.Load("BroadcastSettings", typeof(TextAsset));
            return textAsset.text;
        }
        set { }
    }

    public void FillMarbles() 
    {
        competitorsLength = LeagueManager.LeagueRunning.GetCurrentMarbleCount();
        if (LeagueManager.LeagueRunning.GetIsPairs())
        {
            CreatePairs();
        }
        else
        { 
            CreateSingle();
        }
        filled = true;
        CallSubscribers();
    }

    private void CreateSingle()
    {
        for (int i = 0; i < competitorsLength; i++)
        {
            Marble instance = Instantiate(marblePrefab, startersPositions[i]).GetComponent<Marble>();
            instance.transform.SetParent(startersPositions[0].transform.parent);
            listMarbles.Add(instance);
        }
    }

    private void CreatePairs() 
    {
        for (int i = 0; i < GetCompetitorsPlusPairs(); i++)
        {
            Marble instance = Instantiate(marblePrefab, startersPositions[i]).GetComponent<Marble>();
            instance.transform.SetParent(startersPositions[0].transform.parent);
            listMarbles.Add(instance);
        }
    }

    public int GetCompetitorsPlusPairs() 
    {
        return (LeagueManager.LeagueRunning.GetIsPairs())?competitorsLength*2:competitorsLength;
    }

    public List<Marble> GetMarbles() 
    {
        return listMarbles;
    }
    async void CallSubscribers()
    {
         await Task.Delay(100);
         onListFilled?.Invoke(listMarbles);
    }

    public MarbleDataList GetMarbleDataList()=>  allMarbles;
}
