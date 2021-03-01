using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using LeagueSYS;
using System.Runtime.CompilerServices;
using System.Linq;
public class RaceController : Singleton<RaceController>, IMainExpected
{
    public int lapsLimit;
    public int numberCompetitors =12;
    public int lap { get; private set; } = 0;
    private List<Marble> marbles = new List<Marble>();
    private Queue<Marble> marblesPassed = new Queue<Marble>();
    public Sector sectorInFront;
    public TriggerDetector goalFinal;
    public Board leaderBoardPositions;
    public Board leaderBoardScores;
    public List<Sector> sectorsTrack = new List<Sector>();
    [SerializeField] private CinematicController endRaceControl;
    [SerializeField] DataManager dataManager;
    [SerializeField] Transform _transformSectorConf;
    [SerializeField] GameObject panelIncreaseLap;
    public System.Action OnCountTrafficLigthEnded;
    public System.Action<int> OnPlayerArrived;
    private League leagueSaved;
    private bool alreadyPassPlayer;
    private bool lapPlusShoowed;
    public bool usePowerUps { get; private set; }
    private Marble instancePlayer= null;
    private int rosal;
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

    private void Awake()
    {
        StartCoroutine(SortLeaderBoard());
        SubscribeToTheMainMenu();
        FillMarbles();
        leagueSaved = Wrapper<LeagueSYS.League>.FromJsonsimple(PlayerPrefs.GetString(KeyStorage.LEAGUE));
    }

    void Start()=>goalFinal.OnTriggerEntered += SumLap;
    void FillMarbles() => marbles = GameObject.FindObjectsOfType<Marble>().OfType<Marble>().ToList();
    public void UsePowersUps()=> usePowerUps = true;
    public void SubscribeToTheMainMenu()=> MainMenuController.GetInstance().OnRaceReady += ReadyToPlay;

    public void ReadyToPlay()
    {
        Invoke("StartRace", 2f);
        sectorInFront.triggerDetector.OnTriggerEntered += ExtendRace;
    }

    public void StartRace()
    {
        OnCountTrafficLigthEnded?.Invoke();
    }
    
    public void SumLap(Transform other)
    {
        Marble currentMarble = other.GetComponent<Marble>();
        if (!currentMarble) return;

        if (currentMarble.sectorsPassed >= currentMarble.currentMarbleLap * _transformSectorConf.childCount)
            currentMarble.currentMarbleLap++;

        if (currentMarble.currentMarbleLap > lapsLimit)
        {
            int competi = 0;
            competi = (currentMarble.boardController.transform.GetSiblingIndex() > 11) ? 11 : currentMarble.boardController.transform.GetSiblingIndex();
            
            currentMarble.scorePartial = Constants.pointsPerRacePosition[competi];
            ShowMarblePosition(currentMarble, currentMarble.boardController.transform.GetSiblingIndex());

            if (currentMarble.isPlayer && !alreadyPassPlayer)
            {
                for (int i = 0; i < 12; i++)
                {
                    if (marbles[i].scorePartial == 0)
                        marbles[i].scorePartial = Constants.pointsPerRacePosition[marbles[i].boardController.transform.GetSiblingIndex()];
                }

                endRaceControl.NextMision();
                Invoke("StopTimeRace", 16);
                OnPlayerArrived?.Invoke((currentMarble.boardController.transform.GetSiblingIndex() + 1));
                alreadyPassPlayer = true;
            }
        }

        //if(marblesPassed)

        //if (marbles.Count < numberCompetitors)
        //    marbles.Add(currentMarble);
    }

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
            ,(_marble.isPlayer)?playerName:_marble.marbleInfo.nameMarble
            ,"+"+Constants.pointsPerRacePosition[positionBoard]
            ,_marble.isPlayer, (_marble.isPlayer)?_marble.bufferPlayer.spriteMarbl: _marble.marbleInfo.spriteMarbl,_marble);

        if (_marble.isPlayer)
        {
            dataManager.SetTransactionMoney(Constants.pointsPerRacePosition[positionBoard]);
            dataManager.IncreaseMarblePercentage((int)(Constants.pointsPerRacePosition[positionBoard])*3);
        }
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
        if (leagueSaved != null)
        {
            foreach (LeagueParticipantData mar in leagueSaved.listParticipants)
            {
                if (mar.participantName.Equals(_marble.marbleInfo.nameMarble))
                {
                    handi = (float)(mar.points / 25*leagueSaved.listPrix.Count);
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
