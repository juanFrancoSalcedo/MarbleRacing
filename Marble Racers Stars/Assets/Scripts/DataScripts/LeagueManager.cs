using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;
namespace LeagueSYS
{
    [RequireComponent(typeof(DataManager))]
    public class LeagueManager : MonoBehaviour,IRacerSettingsRagistrable 
    {
        public League Liga 
        {
            get
            {
                if(liga == null)
                { 
                    League bufferLiga = new League();
                    bufferLiga.nameLeague = "Pilots Cups";

                    if (string.IsNullOrEmpty(PlayerPrefs.GetString(KeyStorage.LEAGUE)))
                    {
                        bufferLiga.date = 0;
                        bufferLiga.listPrix = dataManager.allCups.listCups[PlayerPrefs.GetInt(KeyStorage.CURRENTCUP_I, 0)].listPrix;
                        bufferLiga.Teams = dataManager.allCups.listCups[PlayerPrefs.GetInt(KeyStorage.CURRENTCUP_I, 0)].Teams;
                    }
                    else
                    {
                        bufferLiga = Wrapper<League>.FromJsonsimple(PlayerPrefs.GetString(KeyStorage.LEAGUE));
                    }
                    liga = bufferLiga;
                }
                return liga;
            }
            private set 
            {
            }
        }
        public League Manufacturers
        {
            get
            {
                if (manufacturers == null)
                {
                    League bufferLiga = new League();
                    bufferLiga.nameLeague = "Manufacturers Cups";

                    if (string.IsNullOrEmpty(PlayerPrefs.GetString(KeyStorage.LEAGUE_MANUFACTURERS_S)))
                    {
                        bufferLiga.date = 0;
                        bufferLiga.listPrix = dataManager.allCups.listCups[PlayerPrefs.GetInt(KeyStorage.CURRENTCUP_I, 0)].listPrix;
                        bufferLiga.Teams = dataManager.allCups.listCups[PlayerPrefs.GetInt(KeyStorage.CURRENTCUP_I, 0)].Teams;
                    }
                    else
                    {
                        bufferLiga = Wrapper<League>.FromJsonsimple(PlayerPrefs.GetString(KeyStorage.LEAGUE_MANUFACTURERS_S));
                    }
                    manufacturers = bufferLiga;
                }
                return manufacturers;
            }
            private set
            {
            }
        }
        private League liga = null;
        private League manufacturers = null;
        private List<int> marbleListRandomIndex = new List<int>();
        private Marble[] marblesLeague;
        [SerializeField] private Board boardLeag;
        [SerializeField] private bool setMarbleMaterials;
        [SerializeField] private bool isManufacturers;
        private DataManager dataManager;


#region IRaceSettings Methods
        public void SubscribeRacerSettings() 
        {
            if (!RacersSettings.GetInstance().filled)
                RacersSettings.GetInstance().onListFilled += FillMyMarbles;
            else
                FillMyMarbles(RacersSettings.GetInstance().GetMarbles());
        }
        public void FillMyMarbles(List<Marble> marblesObteined)
        {
            marblesLeague = marblesObteined.ToArray();
            if (setMarbleMaterials)
            { 
                ConfigureData();
            }
        }

        #endregion
        void Awake()
        {
            SubscribeRacerSettings();
            dataManager = GetComponent<DataManager>();

            if (dataManager == null)
                Debug.LogError("I NEED DEPENDENCY DataManager " + name);
        }

        void ConfigureData()
        {
            if (string.IsNullOrEmpty(PlayerPrefs.GetString(KeyStorage.LEAGUE)))
            {
                FillRandomMarbleCompetitors();
                CreateCompetitors();
                SaveLeague();
                CurrentLeague.LeagueRunning = Liga;
            }
            else
                LoadLeague();

            if (setMarbleMaterials) SetMarblesMaterials();
            if (Liga != null) SetSettingsRace();
            CheckCanSimulateRace();
        }
        void FillRandomMarbleCompetitors() 
        {
            int count = 0;

            while (count < RacersSettings.GetInstance().competitorsLength-1)
            {
                int currentRandom = Random.Range(1, RacersSettings.GetInstance().allMarbles.GetLengthList());
                if (!marbleListRandomIndex.Contains(currentRandom))
                {
                    marbleListRandomIndex.Add(currentRandom);
                    if (RacersSettings.GetInstance().leagueManager.Liga.GetIsPairs())
                        marbleListRandomIndex.Add(currentRandom);
                    count++;
                }
            }
            marbleListRandomIndex.Add(0);
            if (RacersSettings.GetInstance().leagueManager.Liga.GetIsPairs())
            { 
                marbleListRandomIndex.Add(0);
                marblesLeague[marblesLeague.Length - 2].isPlayer = true;
            }
            else
                marblesLeague[marblesLeague.Length - 1].isPlayer = true;
        }

        private void CreateCompetitors() 
        {
            for (int i = 0; i < marbleListRandomIndex.Count; i++)
            {
                dataManager.allMarbles.PrintInIndex(marbleListRandomIndex[i]);
                LeagueParticipantData par = new LeagueParticipantData();
                par.points = 0;
                par.participantName = RacersSettings.GetInstance().allMarbles.GetSpecificMarble(marbleListRandomIndex[i]).nameMarble;
                Liga.listParticipants.Add(par);
                if (i % 2 == 0)
                { 
                    Manufacturers.listParticipants.Add(par);
                    //print("ddd _ "+Manufacturers.listParticipants[i].participantName);
                }
            }
        }

        void SetSettingsRace() 
        {
            RaceController.Instance.lapsLimit = Liga.listPrix[Liga.date].laps;
            if (Liga.listPrix[Liga.date].usePowers)
                RaceController.Instance.UsePowersUps();
            ShowScores();
        }

        private void SetMarblesMaterials()
        {
            if (dataManager == null) dataManager = GetComponent<DataManager>();
            BubbleSort<LeagueSYS.LeagueParticipantData>.Sort(Liga.listParticipants, "lastPosition");
            if (marblesLeague.Length > 0)
            {
                if (Liga.GetIsPairs())
                    SetMaterialToPairs();
                else
                    SetMaterialToSingle();
            }
        }

        private void SetMaterialToPairs()
        {
            bool playerSet = false;
            for (int i = 0; i < RacersSettings.GetInstance().GetCompetitorsPlusPairs(); i++)
            {
                marblesLeague[i].SetMarbleSettings(dataManager.allMarbles.GetEspecificMarble(Liga.listParticipants[i].participantName));
                if (marblesLeague[i].marbleInfo.nameMarble.Equals(dataManager.allMarbles.GetSpecificMarble(0).nameMarble) && !playerSet)
                {
                    marblesLeague[i].isPlayer = true;
                    playerSet = true;
                }
            }
        }

        private void SetMaterialToSingle()
        {
            for (int i = 0; i < RacersSettings.GetInstance().competitorsLength; i++)
            {
                marblesLeague[i].SetMarbleSettings(dataManager.allMarbles.GetEspecificMarble(Liga.listParticipants[i].participantName));
                if (marblesLeague[i].marbleInfo.nameMarble.Equals(dataManager.allMarbles.GetSpecificMarble(0).nameMarble))
                    marblesLeague[i].isPlayer = true;
            }
        }

        private void LoadLeague()
        {
            if (Liga == null) Debug.LogError("UN BUG Liga nula dios");
            CurrentLeague.LeagueRunning = Liga;
            CurrentLeague.LeagueManufacturers = Manufacturers;
            BugFixingCloseApplication();
        }
        public void ShowPlayersScoreInfo() 
        {
            if (isManufacturers)
                ShowScoresManufacturers();
            else
                ShowScores();
        }

        public void CalculateScores()
        {
            ConfigureData();
            for (int i = 0; i <  RacersSettings.GetInstance().GetCompetitorsPlusPairs(); i++)
            {
                Liga.listParticipants[i].points += marblesLeague[i].scorePartial;
                Liga.listParticipants[i].lastPosition = marblesLeague[i].finalPosition;
                if (!isManufacturers)
                { 
                    boardLeag.participantScores[i].GetComponent<BoardUIController>().BoardParticip.score = Liga.listParticipants[i].points;
                    DisplayScoreLeague(i,liga);
                }
            }
            ShowScoresManufacturers();
            Liga.date++;
            SaveLeague();
            boardLeag.SortScores();
            ShowPositionsInFront();
        }

        private async void CheckCanSimulateRace() 
        {
            await Task.Delay(200);
            if (Liga.listParticipants.Where(item => item.participantName.Contains(Constants.NORMI)) == null)
                SimulateRace();
        }
        private async void SimulateRace()
        {
            print("SecondarySpriteTexture simuló la liga porque no habia marble player RECUERDEME MEJORAR");
            Liga.date++;
            for (int i = 0; i < Liga.GetCurrentMarbleCount(); i++)
            {
                Liga.listParticipants[i].points += Constants.pointsPerRacePosition[i];
            }
            SaveLeague();
            await Task.Delay(200);
            UnityEngine.SceneManagement.SceneManager.LoadScene(dataManager.allCups.NextRace());//dataManager.allCups.
        }

        private async void ShowScores()
        {
            while (boardLeag.participantScores.Length != RacersSettings.GetInstance().GetCompetitorsPlusPairs())
                await Task.Yield();

            if (Liga == null) ConfigureData(); 
            for (int i = 0; i < RacersSettings.GetInstance().GetCompetitorsPlusPairs(); i++)
            {
                boardLeag.participantScores[i].GetComponent<BoardUIController>().BoardParticip.score = Liga.listParticipants[i].points;
                DisplayScoreLeague(i,Liga);
            }

            SaveLeague();
            boardLeag.SortScores();
            ShowPositionsInFront();
        }

        private async void ShowScoresManufacturers() 
        {
            //while (boardLeag..Length != Manufacturers.listParticipants.Count)
              await Task.Delay(10);
            // The RacerBoardPosition Must be isManufacturers
            if (Liga == null) ConfigureData();
            print(Manufacturers.listParticipants.Count+" earth");
            for (int i = 0; i < Manufacturers.listParticipants.Count; i++)
            {
                int indexMarblePair =0;
                Manufacturers.listParticipants[i].points = 0;
                for (int j = 0; j < Liga.listParticipants.Count; j++)
                {
                    if (Manufacturers.listParticipants[i].participantName.Equals(Liga.listParticipants[j].participantName))
                    {
                        Manufacturers.listParticipants[i].points += Liga.listParticipants[j].points;
                        indexMarblePair = j;
                    }
                }
                print(Manufacturers.listParticipants[i].participantName+" fama "+i+" -fama " +Manufacturers.listParticipants[i].points);
                MarbleData cula = dataManager.allMarbles.GetEspecificMarble(Manufacturers.listParticipants[i].participantName);
                boardLeag.participantScores[i].GetComponent<BoardUIController>().StartAnimation("",Manufacturers.listParticipants[i].participantName,
                    Manufacturers.listParticipants[i].points.ToString(),false,cula.spriteMarbl);
                boardLeag.participantScores[i].GetComponent<BoardUIController>().BoardParticip.score = Manufacturers.listParticipants[i].points;
            }
            SaveLeague();
            boardLeag.SortScores();
            ShowPositionsInFront();
        }

        void DisplayScoreLeague(int positionBoard, League league) 
        {
            string playerName = (PlayerPrefs.GetString(KeyStorage.NAME_PLAYER).Equals("")) ? Constants.NORMI :
                PlayerPrefs.GetString(KeyStorage.NAME_PLAYER);

            boardLeag.participantScores[positionBoard].GetComponent<BoardUIController>().StartAnimation("", (marblesLeague[positionBoard].isPlayer) ? playerName : league.listParticipants[positionBoard].participantName
                        , "" + league.listParticipants[positionBoard].points 
                        , marblesLeague[positionBoard].isPlayer, marblesLeague[positionBoard].marbleInfo.spriteMarbl,
                        marblesLeague[positionBoard]);
        }

        void ShowPositionsInFront() => StartCoroutine(ShowPositionInLeague());

        IEnumerator ShowPositionInLeague()
        {
            for (int i = 0; i < boardLeag.participantScores.Length; i++)
            {
                boardLeag.participantScores[i].GetComponent<BoardUIController>().textPosition.text = "" + (boardLeag.participantScores[i].GetComponent<BoardUIController>().transform.GetSiblingIndex() + 1);
            }

            while (gameObject.activeInHierarchy)
            {
                yield return new WaitForSeconds(1);
                for (int i = 0; i < boardLeag.participantScores.Length; i++)
                {
                    boardLeag.participantScores[i].GetComponent<BoardUIController>().textPosition.text = "" + (boardLeag.participantScores[i].GetComponent<BoardUIController>().transform.GetSiblingIndex() + 1);
                }
            }
        }

        public void SaveLeague()
        {
            string jSaved = Wrapper<League>.ToJsonSimple(Liga);
            PlayerPrefs.SetString(KeyStorage.LEAGUE, jSaved);
            string jManufacturers= Wrapper<League>.ToJsonSimple(Manufacturers);
            PlayerPrefs.SetString(KeyStorage.LEAGUE_MANUFACTURERS_S, jManufacturers);
        }

        #region BugFixing
        /// <summary>
        /// Bug Fixing The user close the application before update the league
        /// </summary>
        private void BugFixingCloseApplication()
        {
            if (Liga.date >= Liga.listPrix.Count)
            {
                if (GetComponent<DataManager>())
                {
                    dataManager.EraseAll();
                    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                }
            }
        }
        #endregion
    }
}


