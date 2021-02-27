using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeagueSYS
{
    [RequireComponent(typeof(DataManager))]
    public class LeagueManager : MonoBehaviour
    {
        public League liga { get; set; }
        private List<int> marbleListRandomIndex = new List<int>();
        [SerializeField] private Marble[] marblesLeague;
        [SerializeField] private Board boardLeag;
        [SerializeField] private BoardUIController[] boardIP;
        [SerializeField] private bool setMarbleMaterials;
        private DataManager dataManager;

        void Awake()
        {
            if (marblesLeague[0] == null)
                Debug.LogError("THERE IS NOT MARBLES" + name);

            dataManager = GetComponent<DataManager>();

            if (dataManager == null)
                Debug.LogError("I NEED DEPENDENCY DataManager " + name);

            if (setMarbleMaterials)
                ConfigureData();
        }

        void ConfigureData()
        {
            liga = new League();
            liga.nameLeague = "Current Liga";

            if (string.IsNullOrEmpty(PlayerPrefs.GetString(KeyStorage.LEAGUE)))
            {
                liga.date = 0;
                liga.listPrix = dataManager.allCups.listCups[PlayerPrefs.GetInt(KeyStorage.CURRENTCUP_I, 0)].listPrix;
                int count = 0;
                while (count < 12)
                {
                    int currentRandom = Random.Range(1, RacersSettings.GetInstance().allMarbles.GetLengthList());
                    if (!marbleListRandomIndex.Contains(currentRandom))
                    {
                        marbleListRandomIndex.Add(currentRandom);
                        count++;
                    }
                }
                marbleListRandomIndex[3] = 0;
                for (int i = 0; i < marbleListRandomIndex.Count; i++)
                {
                    dataManager.allMarbles.PrintInIndex(marbleListRandomIndex[i]);

                    LeagueParticipantData par = new LeagueParticipantData();
                    //TODO en este caso no eliminar de la memria porque falta setiar los materiales y todos eso
                    par.points = 0;
                    par.participantName = RacersSettings.GetInstance().allMarbles.GetSpecificMarble(marbleListRandomIndex[i]).nameMarble;
                    liga.listParticipants.Add(par);
                }
                SaveLeague();
                CurrentLeague.LeagueRunning = liga;
            }
            else
            {
                LoadLeague();
            }

            if (setMarbleMaterials) SetMarblesMaterials();

            if (liga != null)
            {
                RaceController.Instance.lapsLimit = liga.listPrix[liga.date].laps;
                if (liga.listPrix[liga.date].usePowers)
                    RaceController.Instance.UsePowersUps();
                ShowScores();
            }
        }

        private void SetMarblesMaterials()
        {
            if (dataManager == null) dataManager = GetComponent<DataManager>();
            if (marblesLeague.Length > 0)
            {
                for (int i = 0; i < liga.listParticipants.Count; i++)
                    marblesLeague[i].SetMarbleSettings(dataManager.allMarbles.GetEspecificMarble(liga.listParticipants[i].participantName));
            }
        }

        private void LoadLeague()
        {
            liga = Wrapper<League>.FromJsonsimple(PlayerPrefs.GetString(KeyStorage.LEAGUE));
            if (liga == null) Debug.LogError("Liga nula Ohh diossss");
            CurrentLeague.LeagueRunning = liga;
            for (int i = 0; i < liga.listParticipants.Count; i++)
            {
                marbleListRandomIndex.Add(RacersSettings.GetInstance().
                    allMarbles.GetIndexOfSpecificName(liga.listParticipants[i].participantName));
            }
            if (liga.date >= liga.listPrix.Count)
            {
                Debug.LogError(" ESTO ES UN BUG SE PASO LAS FECHAS, AL PARECER CERRO EL JUEGO ANTES DE ACTUALIZAR LA LIGA");
                if (GetComponent<DataManager>())
                {
                    dataManager.EraseAll();
                    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                }
            }
        }

        public void CalculateScores()
        {
            ConfigureData();
            liga.date++;

            string playerName = (PlayerPrefs.GetString(KeyStorage.NAME_PLAYER).Equals("")) ? Constants.NORMI :
                PlayerPrefs.GetString(KeyStorage.NAME_PLAYER);
            for (int i = 0; i < 12; i++)
            {
                liga.listParticipants[i].points += marblesLeague[i].scorePartial;
                boardIP[i].BoardParticip.score = liga.listParticipants[i].points;
                boardIP[i].StartAnimation("", (marblesLeague[i].isPlayer) ? playerName : liga.listParticipants[i].participantName
                            , "" + liga.listParticipants[i].points
                            , marblesLeague[i].isPlayer, marblesLeague[i].marbleInfo.spriteMarbl,
                            marblesLeague[i]);
            }
            SaveLeague();
            boardLeag.SortScores();
            ShowPositionsInFront();
        }

        public void ShowScores()
        {
            string playerName = (PlayerPrefs.GetString(KeyStorage.NAME_PLAYER).Equals("")) ? Constants.NORMI :
                PlayerPrefs.GetString(KeyStorage.NAME_PLAYER);
            if (liga == null) ConfigureData();
            for (int i = 0; i < 12; i++)
            {

                boardIP[i].BoardParticip.score = liga.listParticipants[i].points;
                boardIP[i].StartAnimation("", (marblesLeague[i].isPlayer) ? playerName : liga.listParticipants[i].participantName
                            , "" + liga.listParticipants[i].points
                            , marblesLeague[i].isPlayer, marblesLeague[i].marbleInfo.spriteMarbl,
                            marblesLeague[i]);
            }

            SaveLeague();
            boardLeag.SortScores();
            ShowPositionsInFront();
            //Invoke("ShowPositionsInFront",0.2f);
        }

        void ShowPositionsInFront() => StartCoroutine(ShowPositionInLeague());

        IEnumerator ShowPositionInLeague()
        {
            for (int i = 0; i < boardIP.Length; i++)
            {
                boardIP[i].textPosition.text = "" + (boardIP[i].transform.GetSiblingIndex() + 1);
            }

            while (gameObject.activeInHierarchy)
            {
                yield return new WaitForSeconds(1);
                for (int i = 0; i < boardIP.Length; i++)
                {
                    boardIP[i].textPosition.text = "" + (boardIP[i].transform.GetSiblingIndex() + 1);
                }
            }
        }

        private void SaveLeague()
        {
            string jSaved = Wrapper<League>.ToJsonSimple(liga);
            PlayerPrefs.SetString(KeyStorage.LEAGUE, jSaved);
        }
    }

    [System.Serializable]
    public class League
    {
        public string nameLeague;
        [HideInInspector]
        public int date;
        [HideInInspector]
        public List<LeagueParticipantData> listParticipants = new List<LeagueParticipantData>();
        public List<GrandPrix> listPrix = new List<GrandPrix>();
        public CupRequeriments requerimentsLeague;
    }

    [System.Serializable]
    public class LeagueParticipantData
    {
        public string participantName;
        public int points;
    }

    [System.Serializable]
    public class GrandPrix
    {
        public TracksInfo trackInfo;
        public int laps = 1;
        public bool usePowers;
    }

    [System.Serializable]
    public struct CupRequeriments
    {
        public int moneyRequeriments;
        public int trophiesRequeriments;
        public string nameCupPreviousRequeriments;
    }
}


