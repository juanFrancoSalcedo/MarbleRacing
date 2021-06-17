using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyBox;
using TMPro;
using System.Threading.Tasks;

public class BSpecManager : Singleton<BSpecManager>
{
    
    private BSpectCameraController currentController = null;
    private Marble bufferBrokenMarble;
    public event System.Action<bool> onMarbleBrokenAssigned;
    private TypeBoardDisplay displayType = TypeBoardDisplay.timeInterval;
    public event System.Action<TypeBoardDisplay> onDisplayChanged;
    [Header("~~~~~ Race Obj ~~~")]
    [SerializeField] private BSpectCameraController miniCams = null;
    [SerializeField] private BSpectCameraController officialsCams = null;
    [SerializeField] private Renderer trackRenderer = null;
    [Header("~~~~~Pilot Stats ~~~")]
    [SerializeField] GameObject panelStatsInRace =null;
    [SerializeField] DisplayWearRace displayWear = null;
    [SerializeField] DisplayWearRace displayDirt = null;
    [SerializeField] DisplayStatInRace statTurbo = null;
    [SerializeField] DisplayStatInRace statDriving = null;
    [SerializeField] DisplayStatInRace statRechargeDriving = null;
    [SerializeField] DisplayStatInRace statRechargeTurbo = null;
    [SerializeField] TextMeshProUGUI textNamePilot = null;



    private IEnumerator Start()
    {
        currentController = officialsCams;
        currentController.isMainController = true;
        if (trackRenderer == null)
            trackRenderer = GameObject.FindGameObjectWithTag("Track").GetComponent<Renderer>();
        yield return new WaitForSeconds(1);
        System.Array.ForEach(RaceController.Instance.leaderBoardPositions.participantScores, x =>
        x.GetComponent<BoardUIController>().SusbcribeChangeModeBSpec());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            currentController.SwitchTracking();
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            panelStatsInRace.gameObject.SetActive(!panelStatsInRace.activeInHierarchy);
            if (panelStatsInRace.activeInHierarchy)
                ActiveMarbleStats();
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SeeThrough();
        }


        if (Input.GetKeyDown(KeyCode.F4))
        {
            currentController.isMainController = false;
            miniCams.gameObject.SetActive(!miniCams.gameObject.activeInHierarchy);
            currentController = (miniCams.gameObject.activeInHierarchy) ? miniCams : officialsCams;
            currentController.isMainController = true;
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            continueSearchingLasPosition = !continueSearchingLasPosition;
            onFocused?.Invoke(continueSearchingLasPosition);
            if (continueSearchingLasPosition)
                StartCoroutine(ContinuousSearch());
        }

        if (isSearching)
        { 
            searchString += Input.inputString;
            onSearched(isSearching,searchString);
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        { 
            if (isSearching)
                SearchMarble();
            isSearching = !isSearching;
            onSearched(isSearching,searchString);
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
            NextCompetitor();
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
            PreviousCompetitor();

        if (bufferBrokenMarble != null && Input.GetKeyDown(KeyCode.Alpha5))
            currentController.MarbleTarget = bufferBrokenMarble;

        if (Input.GetKeyDown(KeyCode.Tab))
            SwitchMode();
    }
    private void SwitchMode()
    {
        int indexMode = (int)displayType;
        indexMode++;
        if (indexMode >= System.Enum.GetNames(typeof(TypeBoardDisplay)).Length)
            indexMode = 0;
        displayType = (TypeBoardDisplay)System.Enum.GetValues(displayType.GetType()).GetValue(indexMode);
        onDisplayChanged?.Invoke(displayType);
        print(displayType.ToString());
    }

    public async Task AMarbleBroke(Marble theBroken) 
    {
        onMarbleBrokenAssigned?.Invoke(true);
        bufferBrokenMarble = theBroken;
        await Task.Delay(3000);
        bufferBrokenMarble = null;
        onMarbleBrokenAssigned?.Invoke(false);
    }

    void SeeThrough() 
    {
        System.Array.ForEach(trackRenderer.materials, delegate (Material x)
        {
            if (x.HasProperty("_LookThrough"))
                x.SetInt("_LookThrough", x.GetInt("_LookThrough") == 1 ? 0 : 1);
        }
        );
    }


    #region Searching Methods
    private string searchString = null;
    private bool isSearching = false;
    private int lastPositionSearching = 0;
    private bool continueSearchingLasPosition;
    public event System.Action<bool, string> onSearched;
    public event System.Action<bool> onFocused;
    IEnumerator ContinuousSearch() 
    {
        while (continueSearchingLasPosition)
        { 
            SearchMarble(lastPositionSearching);
            yield return new WaitForSeconds(0.3f);
        }
    }
    private void SearchMarble()
    {
        int result =0;
        if (int.TryParse(searchString, out result))
        {
            lastPositionSearching = result;
            currentController.MarbleTarget = RaceController.Instance.GetMarbleByPosition(result);
            searchString = "";
        }
        else
        {
            Marble marble = RaceController.Instance.GetMarbleByNamePilot(searchString.Substring(0,searchString.Length-1));
            if (marble != null)
                currentController.MarbleTarget = marble;
            searchString = "";
        }
    }
    private void SearchMarble(int positionMarb)
    {
        currentController.MarbleTarget = RaceController.Instance.GetMarbleByPosition(positionMarb);
        lastPositionSearching = positionMarb;
        ActiveMarbleStats();
    }

    private void NextCompetitor() 
    {
        int fri = lastPositionSearching - 1;
        lastPositionSearching = Mathf.Clamp(fri, 0, RacersSettings.GetInstance().competitorsLength);
        SearchMarble(lastPositionSearching);
    }

    private void PreviousCompetitor()
    {
        int fri = lastPositionSearching + 1;
        lastPositionSearching = Mathf.Clamp(fri,0,RacersSettings.GetInstance().competitorsLength);
        SearchMarble(lastPositionSearching);
    }

    private void ActiveMarbleStats() 
    {
        displayWear.ShowWear(currentController.MarbleTarget.InitStats, currentController.MarbleTarget.Stats);
        displayDirt.ShowDirt(currentController.MarbleTarget.m_collider.material.dynamicFriction);
        statTurbo.UpdateStats(currentController.MarbleTarget.idPilot);
        statDriving.UpdateStats(currentController.MarbleTarget.idPilot);
        statRechargeDriving.UpdateStats(currentController.MarbleTarget.idPilot);
        statRechargeTurbo.UpdateStats(currentController.MarbleTarget.idPilot);
        textNamePilot.text = "<font=\"TurboAgard_Normal\">" + (RacersSettings.GetInstance()
            .leagueManager.Liga.GetPositionInChampionship(currentController.MarbleTarget.idPilot)+1) +"</font> in the championship";
    }
    #endregion
}
