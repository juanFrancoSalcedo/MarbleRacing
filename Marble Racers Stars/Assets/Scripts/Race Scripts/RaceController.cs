﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using LeagueSYS;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

public class RaceController : Singleton<RaceController>, IMainExpected, IRacerSettingsRagistrable
{
    public int lapsLimit;
    public int lap { get; private set; } = 0;
    [HideInInspector]
    public List<Marble> marbles { get; private set; } = new List<Marble>();
    public Board leaderBoardPositions;
    public Board leaderBoardScores;
    public List<Sector> sectorsTrack = new List<Sector>();
    [SerializeField] private CinematicController endRaceControl;
    public DataManager dataManager;
    [SerializeField] Transform _transformSectorConf;
    [SerializeField] GameObject panelIncreaseLap;
    private bool alreadyPassPlayer;
    private bool lapPlusShoowed;
#region Power Ups Settings
    public bool usePowerUps { get; private set; }
    public bool useSinglePower { get; private set; }
    public PowerUpType typeSingle { get; private set; }
#endregion
    public System.Action OnCountTrafficLigthEnded;
    public event System.Action<int> OnPlayerArrived;
    [Header("~~~~~Sectors Specific~~~~")]
    public Sector sectorInFront;
    public TriggerDetector goalFinal;
    public TriggerDetector qualiTriggerStarter;
    [Header("~~~~~Quali~~~~")]
    [SerializeField] private GameObject prefabMarbleZombie;
    private bool marbleZombieInstantiated;
    private Marble marbleQualyfing;

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
        SubscribeToTheMainMenu();
        Application.targetFrameRate = 30;
        Time.timeScale = 1;
    }

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
        if (RacersSettings.GetInstance().leagueManager.Liga.GetIsQualifying())
        {
            sectorInFront.triggerDetector.OnTriggerEntered += IncreseLapByQualy;
        }
    }

    public void UsePowersUps() => usePowerUps = true;

    public void UseSinglePow(PowerUpType pow) 
    {
        useSinglePower = true;
        typeSingle = pow;
    }

    #region Main Spectators
    public void SubscribeToTheMainMenu() => MainMenuController.GetInstance().OnRaceReady += ReadyToPlay;

    public void ReadyToPlay()
    {
        Invoke("StartRace", 2f);
        StartCoroutine(SortLeaderBoard());
        sectorInFront.triggerDetector.OnTriggerEntered += ExtendRace;
        qualiTriggerStarter.OnTriggerEntered += CheckQualifying;
    }
    #endregion

    public void StartRace()
    {
        OnCountTrafficLigthEnded?.Invoke();
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

            if (!RacersSettings.GetInstance().leagueManager.Liga.GetIsQualifying())
            {
                currentMarble.scorePartial = Constants.pointsPerRacePosition[competi];
                ShowMarblePosition(currentMarble, currentMarble.boardController.transform.GetSiblingIndex());
            }
            else
            { 
                if(competi == 0) 
                    currentMarble.scorePartial = 1;
                ShowMarblePosition(currentMarble, currentMarble.boardController.transform.GetSiblingIndex(), RacersSettings.GetInstance().leagueManager.Liga.GetIsQualifying());
            }
           
            if ((currentMarble.isPlayer || marblePlayerInScene.isZombieQualy) && !alreadyPassPlayer)
                PlayerArrived(currentMarble);
        }
    }

    private void PlayerArrived(Marble marblePlayer) 
    {
        if (!RacersSettings.GetInstance().leagueManager.Liga.GetIsQualifying())
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
    }

    #region Qualifiying

    private void CheckQualifying(Transform transform)
    {
        if (!transform.GetComponent<Marble>()) return;
        if (RacersSettings.GetInstance().leagueManager.Liga.GetIsQualifying() && !marbleZombieInstantiated)
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
        while (countMarbleszombies < RacersSettings.GetInstance().leagueManager.Liga.GetMarblesToQualifying()) 
        {
            yield return new WaitForEndOfFrame();
        }
        onQualifiyingCompleted?.Invoke();
        sectorInFront.triggerDetector.OnTriggerEntered -= IncreseLapByQualy;
    }

    public bool AddMarbleZombie(GameObject obj) 
    {
        countMarbleszombies++;
        if (countMarbleszombies == RacersSettings.GetInstance().leagueManager.Liga.GetMarblesToQualifying())
            return true;
        else if (countMarbleszombies > RacersSettings.GetInstance().leagueManager.Liga.GetMarblesToQualifying())
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
                if (panelIncreaseLap != null && Random.Range(-3f,3f)>0)
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
    public Marble GetPositionMarble(int position)
    {
        Marble theMar = null;
        foreach (Marble mar in marbles)
        {
            if (mar.boardController.BoardParticip.transform.GetSiblingIndex() == position)
            {
                theMar = mar;
                break;
            }
        }
        return theMar;
    }
    private void StopTimeRace()
    {
        Time.timeScale = 0;
    }

    public void ShowMarblePosition( Marble _marble, int positionBoard)
    {
        if (leaderBoardScores == null) { return; }
        string playerName = (PlayerPrefs.GetString(KeyStorage.NAME_PLAYER).Equals("")) ? Constants.NORMI :PlayerPrefs.GetString(KeyStorage.NAME_PLAYER);

        leaderBoardScores.participantScores[positionBoard].GetComponent<BoardUIController>().StartAnimation((positionBoard + 1).ToString()
            , _marble.namePilot+ " " + _marble.marbleInfo.abbreviation
            , "+"+Constants.pointsPerRacePosition[positionBoard]
            ,_marble.isPlayer, _marble.marbleInfo.spriteMarbl,_marble);
        //(_marble.isPlayer) ? _marble.bufferPlayer.spriteMarbl :
        if (_marble.isPlayer)
        {
            dataManager.SetTransactionMoney(Constants.pointsPerRacePosition[positionBoard]);
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
        string playerName = (PlayerPrefs.GetString(KeyStorage.NAME_PLAYER).Equals("")) ? Constants.NORMI : PlayerPrefs.GetString(KeyStorage.NAME_PLAYER);

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
    public float GetHandicapByLeagueSaved(Marble _marble)
    {
        float handi = 0;
        if (RacersSettings.GetInstance().leagueManager.Liga != null)
        {
            foreach (LeagueParticipantData mar in RacersSettings.GetInstance().leagueManager.Liga.listParticipants)
            {
                if (!_marble.isZombieQualy && mar.teamName.Equals(_marble.marbleInfo.nameMarble))
                {
                    handi = (float)(mar.points / 25* RacersSettings.GetInstance().leagueManager.Liga.listPrix.Count);
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
