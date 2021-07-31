using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using LeagueSYS;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

public class RaceController : Singleton<RaceController>, IMainExpected, IRacerSettingsRegistrable
{
    public int minPitsStops { get; set; }
    public int lapsLimit;
    public int lap { get; private set; } = 0;
    [HideInInspector]
    public List<Marble> marbles { get; private set; } = new List<Marble>();
    public Board leaderBoardPositions = null;
    public Board leaderBoardScores = null;
    public List<Sector> sectorsTrack = new List<Sector>();
    [SerializeField] private CinematicController endRaceControl = null;
    public DataController dataManager = null;
    [SerializeField] Transform _transformSectorConf = null;
    [SerializeField] GameObject panelIncreaseLap = null;
    private bool alreadyPassPlayer = false;
    private bool lapPlusShoowed = false;
    public RaceState stateOfRace = RaceState.NoRacing;
#region Power Ups Settings
    public bool usePowerUps { get; private set; }
    public bool useSinglePower { get; private set; }
    public PowerUpType typeSingle { get; private set; }
#endregion
    public System.Action OnCountTrafficLigthEnded = null;
    public event System.Action<int> OnPlayerArrived = null;
    [Header("~~~~~Sectors Specific~~~~")]
    public Sector sectorInFront = null;
    public TriggerDetector goalFinal = null;
    public TriggerDetector qualiTriggerStarter = null;
    [Header("~~~~~Quali~~~~")]
    [SerializeField] private GameObject prefabMarbleZombie = null;
    private bool marbleZombieInstantiated = false;
    private Marble marbleQualyfing = null;
    private Marble instancePlayer = null;
    public Marble marblePlayerInScene {
        get
        {
            if (instancePlayer != null) return instancePlayer;
            foreach (Marble marb in marbles)
            {
                if (marb.isPlayer)
                {
                    instancePlayer = marb;
                    return instancePlayer;
                }
            }
            return null;
        }
        private set { }
    }

    private Marble secondPlayer = null;
    public Marble SecondPlayerInScene
    {
        get
        {
            if (secondPlayer != null) return secondPlayer;
            foreach (Marble marb in marbles)
            {
                if (marb.isPlayer && !ReferenceEquals(marb, marblePlayerInScene))
                {
                    secondPlayer = marb;
                    return secondPlayer;
                }
            }
            return null;
        }
        private set { }
    }

    private void OnValidate()
    {
        if (prefabMarbleZombie == null)
        {
            print("LA MARBLE QUALI ESTA NULA");
        }
    }

    private void Awake()
    {
        SubscribeRacerSettings();
        SubscribeToMainMenu();
        Application.targetFrameRate = 30;
        Time.timeScale = 1;
    }


    private void OnEnable()=> AdsManager.Instance.onRewarded += IncreaseLapLimit;

    private void OnDisable() => AdsManager.Instance.onRewarded -= IncreaseLapLimit;

    #region IRaceSettings Methods

    public void SubscribeRacerSettings()
    {
        RacersSettings.GetInstance().onListFilled += FillMyMarbles;
    }

    public void FillMyMarbles(List<Marble> marblesObteined)
    {
        marbles = marblesObteined;
        foreach (var item in marbles)
        {
            item.name += item.transform.GetSiblingIndex();
            item.currentSector = sectorInFront;
        }
    }

    #endregion

    void Start()
    {
        goalFinal.OnTriggerEntered += SumLap;
        if (LeagueManager.LeagueRunning.GetIsQualifying())
        {
            sectorInFront.triggerDetector.OnTriggerEntered += IncreseLapByQualy;
        }
        StartCoroutine(SearchingPlayer());
    }

    private IEnumerator SearchingPlayer() 
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
        }
    }

    public void UsePowersUps() => usePowerUps = true;

    public void UseSinglePow(PowerUpType pow) 
    {
        useSinglePower = true;
        typeSingle = pow;
    }

    #region Main Spectators
    public void SubscribeToMainMenu() => MainMenuController.GetInstance().OnRaceReady += ReadyToPlay;

    public void ReadyToPlay()
    {
        Invoke("StartRace", 2f);
        StartCoroutine(SortLeaderBoard());
        sectorInFront.triggerDetector.OnTriggerEntered += ExtendRace;
        qualiTriggerStarter.OnTriggerEntered += CheckQualifying;
        ActiveBoarPositions();
       
    }
    #endregion

    private void ActiveBoarPositions() 
    {
        System.Array.ForEach(leaderBoardPositions.participantScores, delegate (BoardParticipant x)
        {
            x.GetComponent<BoardUIController>().UpdateAutomatically();
        }
       );
        marbles.ForEach(x => x.UpdateBoardForBroadcasting());
    }

    public void StartRace()
    {
        OnCountTrafficLigthEnded?.Invoke();
        stateOfRace = RaceState.Racing;
    }

    public void SumLap(Transform other)
    {
        Marble currentMarble = other.GetComponent<Marble>();
        if (!currentMarble) return;
        if (currentMarble.isZombieQualy) return;

        if (currentMarble.sectorsPassed >= currentMarble.currentMarbleLap * _transformSectorConf.childCount)
        { 
            currentMarble.currentMarbleLap++;
            currentMarble.onLapWasSum?.Invoke();
        }

        if (currentMarble.currentMarbleLap > lapsLimit)
        {
            int competi = (currentMarble.boardController.transform.GetSiblingIndex() > RacersSettings.GetInstance().GetCompetitorsPlusPairs() - 1) ?
                RacersSettings.GetInstance().competitorsLength - 1 :
                currentMarble.boardController.transform.GetSiblingIndex();
            // assign the classification place even though it is inaccurate 
            currentMarble.finalPosition = competi;

            if (!LeagueManager.LeagueRunning.GetIsQualifying())
            {
                currentMarble.scorePartial = Constants.pointsPerRacePosition[competi];
                ShowMarblePosition(currentMarble, currentMarble.boardController.transform.GetSiblingIndex());
            }
            else
            { 
                if(competi == 0) 
                    currentMarble.scorePartial = 1;
                ShowMarblePosition(currentMarble, currentMarble.boardController.transform.GetSiblingIndex(), LeagueManager.LeagueRunning.GetIsQualifying());
            }
           
            if ((currentMarble.isPlayer || marblePlayerInScene.isZombieQualy) && !alreadyPassPlayer)
                PlayerArrived(currentMarble);
        }
    }

    private void PlayerArrived(Marble marblePlayer) 
    {
        if (!LeagueManager.LeagueRunning.GetIsQualifying())
        { 
            for (int i = 0; i < RacersSettings.GetInstance().competitorsLength; i++)
            { 
                  marbles[i].scorePartial = Constants.pointsPerRacePosition[marbles[i].boardController.transform.GetSiblingIndex()];
            }
        }
        endRaceControl.NextMision();
        Invoke("StopTimeRace", 16);
        OnPlayerArrived?.Invoke((marblePlayer.boardController.transform.GetSiblingIndex() + 1));
        alreadyPassPlayer = true;
        stateOfRace = RaceState.RaceEnded;
    }

    #region Qualifiying

    private void CheckQualifying(Transform transform)
    {
        if (!transform.GetComponent<Marble>()) return;
        if (LeagueManager.LeagueRunning.GetIsQualifying() && !marbleZombieInstantiated)
        {
            marbleQualyfing = Instantiate(prefabMarbleZombie, null).GetComponent<Marble>();
            marbleQualyfing.currentSector = sectorInFront;
            marbleQualyfing.transform.position = RacersSettings.GetInstance().startersPositions[30].position;
            marbleZombieInstantiated = true;
            marbleQualyfing.FirstImpulse();
            StartCoroutine(CheckMarbleDeath());
        }
    }

    int countMarbleszombies;
    public event System.Action onQualifiyingCompleted;

    IEnumerator CheckMarbleDeath() 
    {
        while (countMarbleszombies < LeagueManager.LeagueRunning.GetMarblesToQualifying()) 
        {
            yield return new WaitForEndOfFrame();
        }
        onQualifiyingCompleted?.Invoke();
        sectorInFront.triggerDetector.OnTriggerEntered -= IncreseLapByQualy;
    }

    public bool AddMarbleZombie(GameObject obj) 
    {
        countMarbleszombies++;
        if (countMarbleszombies == LeagueManager.LeagueRunning.GetMarblesToQualifying())
            return true;
        else if (countMarbleszombies > LeagueManager.LeagueRunning.GetMarblesToQualifying())
            return false;
        else
            return true;
    }

    private void IncreseLapByQualy(Transform other) 
    {
        Marble otherCompo = other.GetComponent<Marble>();
        if (!otherCompo.isZombieQualy && otherCompo.boardController.transform.GetSiblingIndex() <1)
        {
            IncreaseLapLimit();
        }
    }

#endregion

    public void ExtendRace(Transform marbleTransfor)
    {
        if (!lapPlusShoowed && dataManager.GetMoney() >= 10)
        {
            Marble currentMarble = marbleTransfor.GetComponent<Marble>();

            if (currentMarble.boardController.transform.GetSiblingIndex() ==0 && !currentMarble.isPlayer && currentMarble.currentMarbleLap >= lapsLimit)
            {
                lapPlusShoowed = true;
                if (panelIncreaseLap != null)
                {
                    panelIncreaseLap.SetActive(true);
                    Time.timeScale = 0;
                }
            }
        }
    }
    public void IncreaseLapLimit()
    {
        panelIncreaseLap.SetActive(false);
        lapsLimit++;
        Time.timeScale = 1;
    }

    private void StopTimeRace()
    {
        Time.timeScale = 0;
    }
#region League Sort Methods
    public void ShowMarblePosition( Marble _marble, int positionBoard)
    {
        if (leaderBoardScores == null) { return; }
        leaderBoardScores.participantScores[positionBoard].GetComponent<BoardUIController>().StartAnimation((positionBoard + 1).ToString()
            , _marble.namePilot
            , "+"+Constants.pointsPerRacePosition[positionBoard]
            ,_marble.isPlayer, _marble.marbleInfo.spriteMarbl,_marble);
        //(_marble.isPlayer) ? _marble.bufferPlayer.spriteMarbl :
        if (_marble.isPlayer)
        {
            MoneyManager.Transact(Constants.pointsPerRacePosition[positionBoard]);
            dataManager.IncreaseMarblePercentage((int)(Constants.pointsPerRacePosition[positionBoard])*3);
        }
    }
    /// <summary>
    ///  Method for Qualy
    /// </summary>
    /// <param name="_marble"></param>
    /// <param name="positionBoard"></param>
    /// <param name="isQualy"></param>

    public void ShowMarblePosition(Marble _marble, int positionBoard, bool isQualy)
    {
        if (leaderBoardScores == null) { Debug.LogError("arañan"); return; }
        string playerName = Constants.ReplaceNameNormi(dataManager);

        leaderBoardScores.participantScores[positionBoard].GetComponent<BoardUIController>().StartAnimation((positionBoard + 1).ToString()
            , (_marble.isPlayer) ? playerName : _marble.marbleInfo.nameMarble
            ,(positionBoard ==0)?"Pole +1":""
            , _marble.isPlayer, _marble.marbleInfo.spriteMarbl, _marble);
        //(_marble.isPlayer) ? _marble.bufferPlayer.spriteMarbl :
        //if (_marble.isPlayer)
    }
    IEnumerator SortLeaderBoard()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            leaderBoardPositions.SortScores();
        }
    }

    public Marble GetMarbleByPosition(int position)
    {
        Marble theMar = null;
        theMar = marbles.Find(x => x.boardController.BoardParticip.transform.GetSiblingIndex() == position);
        return theMar;
    }

    public Marble GetMarbleByNamePilot(string _namePilot) 
    {
        return marbles.Find(delegate (Marble x) {
            Marble renuente = null;
            if (x.namePilot.Equals(_namePilot)) {renuente = x;}
            return renuente;
        }); 
    }
    #endregion

    public float GetHandicapByLeagueSaved(Marble _marble)
    {
        float handi = 0;
        if (LeagueManager.LeagueRunning != null)
        {
            foreach (LeagueParticipantData mar in LeagueManager.LeagueRunning.listParticipants)
            {
                if (!_marble.isZombieQualy && mar.teamName.Equals(_marble.marbleInfo.nameMarble))
                {
                    handi = (float)(mar.points /(30* LeagueManager.LeagueRunning.listPrix.Count));
                    break;
                }
            }
        }
        handi = 0;
        return handi;
    }

    [ButtonMethod]
    private void ConfigureSectors()
    {
        sectorsTrack.Clear();
        for (int i = 0; i < _transformSectorConf.childCount; i++)
        {
            Sector partialSector = _transformSectorConf.GetChild(i).GetComponent<Sector>();
            sectorsTrack.Add(partialSector);
            if (i < _transformSectorConf.childCount - 1)
            {
                partialSector.nextSector = _transformSectorConf.GetChild(i +1).GetComponent<Sector>();
            }
        }
        _transformSectorConf.GetChild(_transformSectorConf.childCount-1).GetComponent<Sector>().nextSector = _transformSectorConf.GetChild(0).GetComponent<Sector>();
    }
}
