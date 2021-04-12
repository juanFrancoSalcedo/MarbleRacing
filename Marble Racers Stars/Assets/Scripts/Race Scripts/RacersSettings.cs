using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeagueSYS;
using System.Threading.Tasks;

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
    public LeagueManager leagueManager { get; private set; }
    public bool filled { get; set;}
    // LAMENTO DECIRLE PERO ESTO NO SE PUEDE CAMBIAR POR QUE TENDRIAMOS PROBLEMAS CON GUARDAR LOS NOIOMBRES DE NORMI
    // RECUERDE PLAYER Y NORMI EN LA POSICION 4 [3]

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
    private void Awake()
    {
        FillMarbles();
    }
    
    public void FillMarbles() 
    {
        leagueManager = GameObject.FindObjectOfType<LeagueManager>();
        if (leagueManager == null)
        {
            Debug.LogError("there are not league manager avalible");
            return;
        }
        else
        { 
           competitorsLength = leagueManager.Liga.GetCurrentMarbleCount();
            print(competitorsLength);
        }
        if (leagueManager.Liga.GetIsPairs())
            CreatePairs();
        else
            CreateSingle();
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
        return (leagueManager.Liga.GetIsPairs())?competitorsLength*2:competitorsLength;
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
