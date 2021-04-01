using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
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
                    bufferLiga.nameLeague = "Current Liga";

                    if (string.IsNullOrEmpty(PlayerPrefs.GetString(KeyStorage.LEAGUE)))
                    {
                        bufferLiga.date = 0;
                        bufferLiga.listPrix = dataManager.allCups.listCups[PlayerPrefs.GetInt(KeyStorage.CURRENTCUP_I, 0)].listPrix;
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
        private League liga = null;
        private List<int> marbleListRandomIndex = new List<int>();
        private Marble[] marblesLeague;
        [SerializeField] private Board boardLeag;
        [SerializeField] private bool setMarbleMaterials;
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
            ConfigureData();
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
                int count = 0;
                while (count < RacersSettings.GetInstance().competitorsLength)
                {
                    int currentRandom = Random.Range(1, RacersSettings.GetInstance().allMarbles.GetLengthList());
                    if (!marbleListRandomIndex.Contains(currentRandom))
                    {
                        marbleListRandomIndex.Add(currentRandom);
                        count++;
                    }
                }

                for (int i = 0; i < marbleListRandomIndex.Count; i++)
                {
                    dataManager.allMarbles.PrintInIndex(marbleListRandomIndex[i]);

                    LeagueParticipantData par = new LeagueParticipantData();
                    //TODO en este caso no eliminar de la memria porque falta setiar los materiales y todos eso
                    par.points = 0;
                    par.participantName = RacersSettings.GetInstance().allMarbles.GetSpecificMarble(marbleListRandomIndex[i]).nameMarble;
                    Liga.listParticipants.Add(par);
                }
                SaveLeague();
                CurrentLeague.LeagueRunning = Liga;
            }
            else
                LoadLeague();

            if (setMarbleMaterials) SetMarblesMaterials();
            if (Liga != null) SetSettingsRace();
        }

        void SetSettingsRace() 
        {
            RaceController.Instance.lapsLimit = Liga.listPrix[Liga.date].laps;
            if (Liga.listPrix[Liga.date].usePowers)
                RaceController.Instance.UsePowersUps();
            if (RacersSettings.GetInstance().legaueManager.Liga.GetIsQualifying())
                PositionsQualy();
            else
                ShowScores();
        }

        private void SetMarblesMaterials()
        {
            if (dataManager == null) dataManager = GetComponent<DataManager>();
            if (marblesLeague.Length > 0)
            {
                for (int i = 0; i < Liga.listParticipants.Count; i++)
                    marblesLeague[i].SetMarbleSettings(dataManager.allMarbles.GetEspecificMarble(Liga.listParticipants[i].participantName));
            }
        }

        private void LoadLeague()
        {
            if (Liga == null) Debug.LogError("UN BUG Liga nula dios");
            CurrentLeague.LeagueRunning = Liga;
            // Bug Fixing The user close the application before update the league
            if (Liga.date >= Liga.listPrix.Count)
            {
                if (GetComponent<DataManager>())
                {
                    dataManager.EraseAll();
                    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                }
            }
        }

        public void ShowPlayersScoreInfo() => CalculateScores();

        private void CalculateScores()
        {
            ConfigureData();
            Liga.date++;
            for (int i = 0; i < RacersSettings.GetInstance().competitorsLength; i++)
            {
                Liga.listParticipants[i].points += marblesLeague[i].scorePartial;
                Liga.listParticipants[i].lastPosition = marblesLeague[i].finalPosition;
                boardLeag.participantScores[i].GetComponent<BoardUIController>().BoardParticip.score = Liga.listParticipants[i].points;
                boardLeag.participantScores[i].GetComponent<BoardUIController>().BoardParticip.secondScore = marblesLeague[i].finalPosition;
                DisplayScoreLeague(i);
            }
            SaveLeague();
            boardLeag.SortScores();
            ShowPositionsInFront();
        }

        private void PositionsQualy()
        {
            ConfigureData();
            Liga.date++;
            Liga.listParticipants[0].points = 1;
            for (int i = 0; i < RacersSettings.GetInstance().competitorsLength; i++)
            {
                boardLeag.participantScores[i].GetComponent<BoardUIController>().BoardParticip.score = Liga.listParticipants[i].points;
                DisplayScoreLeague(i);
            }
            SaveLeague();
            boardLeag.SortScores();
            ShowPositionsInFront();
        }

        public async void ShowScores()
        {
            while (boardLeag.participantScores.Length != RacersSettings.GetInstance().competitorsLength)
                await Task.Yield();
            
            if (Liga == null) ConfigureData();
            for (int i = 0; i < RacersSettings.GetInstance().competitorsLength ; i++)
            {
                DisplayScoreLeague(i);
            }

            SaveLeague();
            boardLeag.SortScores();
            ShowPositionsInFront();
            //Invoke("ShowPositionsInFront",0.2f);
        }

        void DisplayScoreLeague(int positionBoard) 
        {
            string playerName = (PlayerPrefs.GetString(KeyStorage.NAME_PLAYER).Equals("")) ? Constants.NORMI :
                PlayerPrefs.GetString(KeyStorage.NAME_PLAYER);

            boardLeag.participantScores[positionBoard].GetComponent<BoardUIController>().StartAnimation("", (marblesLeague[positionBoard].isPlayer) ? playerName : Liga.listParticipants[positionBoard].participantName
                        , "" +Liga.listParticipants[positionBoard].points
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

        private void SaveLeague()
        {
            string jSaved = Wrapper<League>.ToJsonSimple(Liga);
            PlayerPrefs.SetString(KeyStorage.LEAGUE, jSaved);
        }
    }
}


