using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;
namespace LeagueSYS
{
    [RequireComponent(typeof(DataController))]
    public class LeagueHandler : MonoBehaviour,IRacerSettingsRegistrable 
    {
        private List<int> marbleListRandomIndex = new List<int>();
        private Marble[] marblesLeague = null;
        [SerializeField] private Board boardLeag = null;
        [SerializeField] private bool setMarbleMaterials = false;
        [SerializeField] private bool isManufacturers = false;
        private DataController dataManager = null;

        void Awake()
        {
            dataManager = GetComponent<DataController>();
            SubscribeRacerSettings();
            if (dataManager == null)
                Debug.LogError("I NEED DEPENDENCY DataManager " + name);
        }

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
                ConfigureData();
        }

        #endregion

        public void ConfigureData()
        {
            LeagueManager.CreateCompetitors(dataManager.allCups,dataManager.allMarbles);
            if (LeagueManager.LeagueRunning == null)
            {
            }
            LoadLeague();
            SetMarblesMaterials();
            SetSettingsRace();
            CheckCanSimulateRace();
        }

        void SetSettingsRace() 
        {
            RaceController.Instance.lapsLimit = LeagueManager.LeagueRunning.listPrix[LeagueManager.LeagueRunning.date].laps;
            RaceController.Instance.minPitsStops = (int)LeagueManager.LeagueRunning.listPrix[LeagueManager.LeagueRunning.date].wear;
            if (LeagueManager.LeagueRunning.listPrix[LeagueManager.LeagueRunning.date].usePowers)
            { 
                RaceController.Instance.UsePowersUps();
                if (!LeagueManager.LeagueRunning.listPrix[LeagueManager.LeagueRunning.date].useAllPows)
                    RaceController.Instance.UseSinglePow(LeagueManager.LeagueRunning.listPrix[LeagueManager.LeagueRunning.date].singlePow);
            }
            ShowScores();
        }

        private void SetMarblesMaterials()
        {
            if (dataManager == null) dataManager = GetComponent<DataController>();
            BubbleSort<LeagueSYS.LeagueParticipantData>.Sort(LeagueManager.LeagueRunning.listParticipants, "lastPosition");
            if (marblesLeague.Length > 0)
            {
                if (LeagueManager.LeagueRunning.GetIsPairs())
                    SetMaterialToPairs();
                else
                    SetMaterialToSingle();
            }
        }

        private void SetMaterialToPairs()
        {
            //bool playerSet = false;
            for (int i = 0; i < RacersSettings.GetInstance().GetCompetitorsPlusPairs(); i++)
            {
                if (LeagueManager.LeagueRunning.listParticipants[i].teamName.Equals(Constants.NORMI))
                {
                    marblesLeague[i].isPlayer = true;
                }

                MarbleData buffer = dataManager.allMarbles.GetSpecificMarble(LeagueManager.LeagueRunning.listParticipants[i].teamName);
                marblesLeague[i].namePilot = LeagueManager.LeagueRunning.listParticipants[i].pilot.namePilot;
                marblesLeague[i].idPilot = LeagueManager.LeagueRunning.listParticipants[i].pilot.ID;
                marblesLeague[i].SetStats(LeagueManager.LeagueRunning.listParticipants[i].pilot.stats);
                marblesLeague[i].SetMarbleSettings(buffer);
            }
        }
        private void SetMaterialToSingle()
        {
            for (int i = 0; i < RacersSettings.GetInstance().competitorsLength; i++)
            {
                if (LeagueManager.LeagueRunning.listParticipants[i].teamName.Equals(Constants.NORMI)) 
                {
                    marblesLeague[i].isPlayer = true;
                }

                MarbleData buffer = dataManager.allMarbles.GetSpecificMarble(LeagueManager.LeagueRunning.listParticipants[i].teamName);
                marblesLeague[i].namePilot = LeagueManager.LeagueRunning.listParticipants[i].pilot.namePilot;
                marblesLeague[i].idPilot = LeagueManager.LeagueRunning.listParticipants[i].pilot.ID;
                marblesLeague[i].SetStats(LeagueManager.LeagueRunning.listParticipants[i].pilot.stats);
                marblesLeague[i].SetMarbleSettings(buffer);
            }
        }
        private void LoadLeague()
        {
           // if (LeagueManager.LeagueRunning.listParticipants.Count<= 0 && setMarbleMaterials)
           // {
          //      ConfigureData();
           // }
           // if (LeagueManager.LeagueRunning == null) Debug.LogError("UN BUG Liga nula dios");
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
            //boardLeag.participantScores[0].GetComponent<BoardUIController>().BoardParticip.score = Random.Range(0, 20);
            
            for (int i = 0; i <  RacersSettings.GetInstance().GetCompetitorsPlusPairs(); i++)
            {
                LeagueManager.LeagueRunning.listParticipants[i].points += marblesLeague[i].scorePartial;
                LeagueManager.LeagueRunning.listParticipants[i].lastPosition = marblesLeague[i].finalPosition;
                //LeagueManager.LeagueRunning.listParticipants[i].points
                if (!isManufacturers)
                {
                    boardLeag.participantScores[i].GetComponent<BoardUIController>().BoardParticip.score = LeagueManager.LeagueRunning.listParticipants[i].points;
                    DisplayScoreLeague(i, LeagueManager.LeagueRunning);
                }
            }
            ShowScoresManufacturers();
            IncreaseDate();
            SaveLeague();
            boardLeag.SortScores();
            ShowPositionsInFront();
        }

        private async void CheckCanSimulateRace() 
        {
            await Task.Delay(200);
            if (LeagueManager.LeagueRunning.listParticipants.Where(item => item.teamName.Contains(Constants.NORMI)) == null)
                SimulateRace();
        }
        private async void SimulateRace()
        {
            print("SecondarySpriteTexture simuló la liga porque no habia marble player RECUERDEME MEJORAR");
            IncreaseDate();
            for (int i = 0; i < LeagueManager.LeagueRunning.GetCurrentMarbleCount(); i++)
            {
                LeagueManager.LeagueRunning.listParticipants[i].points += Constants.pointsPerRacePosition[i];
            }
            SaveLeague();
            await Task.Delay(200);
            UnityEngine.SceneManagement.SceneManager.LoadScene(dataManager.allCups.NextRace().NameTrack);//dataManager.allCups.
        }

        private async void ShowScores()
        {
            while (RacersSettings.GetInstance() != null && boardLeag.participantScores.Length != RacersSettings.GetInstance().GetCompetitorsPlusPairs())
                await Task.Yield();

            if (RacersSettings.GetInstance() == null) return;
            for (int i = 0; i < RacersSettings.GetInstance().GetCompetitorsPlusPairs(); i++)
            {
                //if (boardLeag.participantScores[i] == null) return;
                boardLeag.participantScores[i].GetComponent<BoardUIController>().BoardParticip.score = LeagueManager.LeagueRunning.listParticipants[i].points;
                DisplayScoreLeague(i, LeagueManager.LeagueRunning);
            }

            SaveLeague();
            boardLeag.SortScores();
            ShowPositionsInFront();
        }

        private async void ShowScoresManufacturers() 
        {
            await Task.Delay(10);

            for (int i = 0; i < LeagueManager.LeagueManufacturers.listParticipants.Count; i++)
            {
                List<LeagueParticipantData> fisrtPilot = LeagueManager.LeagueRunning.listParticipants.FindAll(x => x.teamName.Equals(LeagueManager.LeagueManufacturers.listParticipants[i].teamName));
                int sum = 0;
                fisrtPilot.ForEach(x => sum += x.points);
                LeagueManager.LeagueManufacturers.listParticipants[i].points = sum;

                boardLeag.participantScores[i].GetComponent<BoardUIController>().BoardParticip.score = LeagueManager.LeagueManufacturers.listParticipants[i].points;
                MarbleData cula = dataManager.allMarbles.GetSpecificMarble(LeagueManager.LeagueManufacturers.listParticipants[i].teamName);
                boardLeag.participantScores[i].GetComponent<BoardUIController>().StartAnimation("", LeagueManager.LeagueManufacturers.listParticipants[i].teamName,
                    LeagueManager.LeagueManufacturers.listParticipants[i].points.ToString(),false,cula.spriteMarbl);
            }
            SaveLeague();
            boardLeag.SortScores();
            ShowPositionsInFront();
        }

        void DisplayScoreLeague(int positionBoard, League league) 
        {
            boardLeag.participantScores[positionBoard].GetComponent<BoardUIController>().StartAnimation("",league.listParticipants[positionBoard].pilot.namePilot
                        , "" + league.listParticipants[positionBoard].points 
                        , marblesLeague[positionBoard].isPlayer, marblesLeague[positionBoard].marbleInfo.spriteMarbl,
                        marblesLeague[positionBoard]);
        }

        private void IncreaseDate() 
        {
            LeagueManager.LeagueRunning.date++;
            print(LeagueManager.LeagueRunning.date+" date");
        }

        void ShowPositionsInFront() => StartCoroutine(ShowPositionInLeague());

        IEnumerator ShowPositionInLeague()
        {
            for (int i = 0; i < boardLeag.participantScores.Length; i++)
            {
                if(gameObject.activeInHierarchy)
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
            string jSaved = Wrapper<League>.ToJsonSimple(LeagueManager.LeagueRunning);
            PlayerPrefs.SetString(KeyStorage.LEAGUE_S, jSaved);
            string jManufacturers= Wrapper<League>.ToJsonSimple(LeagueManager.LeagueManufacturers);
            PlayerPrefs.SetString(KeyStorage.LEAGUE_MANUFACTURERS_S, jManufacturers);
            print("guarde ligfa "+ PlayerPrefs.GetString(KeyStorage.LEAGUE_S));
        }

        #region BugFixing
        /// <summary>
        /// Bug Fixing The user close the application before update the league
        /// </summary>
        private void BugFixingCloseApplication()
        {
            if (LeagueManager.LeagueRunning.date >= LeagueManager.LeagueRunning.listPrix.Count)
            {
                if (GetComponent<DataController>())
                {
                    dataManager.EraseAll();
                    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                }
            }
        }
        #endregion
    }
}


