using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;
using System;
using LeagueSYS;

namespace LeagueSYS
{
    [RequireComponent(typeof(DataController))]
    public class LeagueHandler : MonoBehaviour, IRacerSettingsRegistrable
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
            if (LeagueManager.IsNullLeagueData())
                LeagueManager.CreateCompetitors(dataManager.allCups, dataManager.allMarbles);
            SetMarblesMaterials();
            SetSettingsRace();
            CheckCanSimulateRace();
        }

        void SetSettingsRace()
        {
            if (LeagueManager.LeagueRunning.date >= LeagueManager.LeagueRunning.listPrix.Count)
                LeagueManager.LeagueRunning.date = 0;
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
            for (int i = 0; i < RacersSettings.GetInstance().GetCompetitorsPlusPairs(); i++)
            {
                var copy_Team = LeagueManager.LeagueRunning.CopyDataParticipant(i);
                if (copy_Team.teamName.Equals(Constants.NORMI))
                    marblesLeague[i].isPlayer = true;
                MarbleData buffer = dataManager.allMarbles.GetSpecificMarble(copy_Team.teamName);
                marblesLeague[i].namePilot = copy_Team.pilot.namePilot;
                marblesLeague[i].idPilot = copy_Team.pilot.ID;
                marblesLeague[i].SetStats(copy_Team.pilot.stats);
                marblesLeague[i].SetMarbleSettings(buffer);
            }
        }
        private void SetMaterialToSingle()
        {
            for (int i = 0; i < RacersSettings.GetInstance().competitorsLength; i++)
            {
                var copy_Team = LeagueManager.LeagueRunning.CopyDataParticipant(i);
                if (copy_Team.teamName.Equals(Constants.NORMI))
                {
                    marblesLeague[i].isPlayer = true;
                }
                MarbleData buffer = dataManager.allMarbles.GetSpecificMarble(copy_Team.teamName);
                marblesLeague[i].namePilot = copy_Team.pilot.namePilot;
                marblesLeague[i].idPilot = copy_Team.pilot.ID;
                marblesLeague[i].SetStats(copy_Team.pilot.stats);
                marblesLeague[i].SetMarbleSettings(buffer);
            }
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
            for (int i = 0; i < RacersSettings.GetInstance().GetCompetitorsPlusPairs(); i++)
            {
                LeagueManager.LeagueRunning.listParticipants[i].points += marblesLeague[i].scorePartial;
                LeagueManager.LeagueRunning.listParticipants[i].lastPosition = marblesLeague[i].finalPosition;
                if (!isManufacturers)
                {
                    boardLeag.participantScores[i].GetComponent<BoardUIController>().BoardParticip.score = LeagueManager.LeagueRunning.listParticipants[i].points;
                    DisplayScoreLeague(i, LeagueManager.LeagueRunning);
                }
            }
            ShowScoresManufacturers();
            IncreaseDate();
            LeagueSaver saver = new LeagueSaver();
            saver.SaveLeague();
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
            LeagueSaver saver = new LeagueSaver();
            saver.SaveLeague();
            await Task.Delay(200);
            UnityEngine.SceneManagement.SceneManager.LoadScene(dataManager.allCups.NextRace().NameTrack);//dataManager.allCups.
        }

        private async void ShowScores()
        {
            while (RacersSettings.GetInstance() != null && boardLeag.participantScores.Length != RacersSettings.GetInstance().GetCompetitorsPlusPairs())
                await Task.Yield();

            if (RacersSettings.GetInstance() == null)
                return;
            for (int i = 0; i < RacersSettings.GetInstance().GetCompetitorsPlusPairs(); i++)
            {
                boardLeag.participantScores[i].GetComponent<BoardUIController>().BoardParticip.score = LeagueManager.LeagueRunning.listParticipants[i].points;
                DisplayScoreLeague(i, LeagueManager.LeagueRunning);
            }

            //SaveLeague();
            boardLeag.SortScores();
            ShowPositionsInFront();
        }

        private async void ShowScoresManufacturers()
        {
            await Task.Delay(10);
            Dictionary<string, int> sums = new Dictionary<string, int>();
            //names of the teams, are storage in list for access by index easier
            List<string> keys = new List<string>();

            for (int i = 0; i < LeagueManager.LeagueRunning.listParticipants.Count; i++)
            {
                string name_team = LeagueManager.LeagueRunning.listParticipants[i].teamName;
                int score = LeagueManager.LeagueRunning.listParticipants[i].points;
                if (!sums.ContainsKey(name_team))
                {
                    sums.Add(name_team, LeagueManager.LeagueRunning.listParticipants[i].points);
                    keys.Add(name_team);
                }
                else if (sums.TryGetValue(name_team, out var catchedScore))
                {
                    var innerSum = sums[name_team];
                    innerSum += score;
                    sums[name_team] = innerSum;
                }
            }

            for (int i = 0; i < boardLeag.participantScores.Length; i++)
            {
                var uiComponent = boardLeag.participantScores[i].GetComponent<BoardUIController>();
                uiComponent.BoardParticip.score = sums[keys[i]];
                MarbleData dataMarble = dataManager.allMarbles.GetSpecificMarble(keys[i]);
                uiComponent.StartAnimation("", keys[i], sums[keys[i]].ToString(),false,dataMarble.spriteMarbl);
            }

            boardLeag.SortScores();
            ShowPositionsInFront();
        }

        void DisplayScoreLeague(int positionBoard, League league)
        {
            var clonePilot = league.CopyDataParticipant(positionBoard);

            boardLeag.participantScores[positionBoard].GetComponent<BoardUIController>().StartAnimation("", clonePilot.pilot.namePilot
                        , "" + clonePilot.points
                        , marblesLeague[positionBoard].isPlayer, marblesLeague[positionBoard].marbleInfo.spriteMarbl,
                        marblesLeague[positionBoard]);
        }

        private void IncreaseDate() => LeagueManager.LeagueRunning.date++;

        void ShowPositionsInFront() => StartCoroutine(ShowPositionInLeague());

        IEnumerator ShowPositionInLeague()
        {
            for (int i = 0; i < boardLeag.participantScores.Length; i++)
            {
                if (gameObject.activeInHierarchy)
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

    }
}

public class LeagueManufactures
{
    

    public List<LeagueParticipantData> GetParticipantsLeague() 
    {
        Dictionary<string, int> sums = new Dictionary<string, int>();
        //names of the teams, are storage in list for access by index easier
        List<string> keys = new List<string>();
        List<LeagueParticipantData> participants = new List<LeagueParticipantData>();
        for (int i = 0; i < LeagueManager.LeagueRunning.listParticipants.Count; i++)
        {
            string name_team = LeagueManager.LeagueRunning.listParticipants[i].teamName;
            int score = LeagueManager.LeagueRunning.listParticipants[i].points;
            if (!sums.ContainsKey(name_team))
            {
                sums.Add(name_team, LeagueManager.LeagueRunning.listParticipants[i].points);
                keys.Add(name_team);
            }
            else if (sums.TryGetValue(name_team, out var catchedScore))
            {
                var innerSum = sums[name_team];
                innerSum += score;
                sums[name_team] = innerSum;
            }
        }

        foreach (var item in sums)
        {
            var newParticipant = new LeagueParticipantData();
            newParticipant.teamName = item.Key;
            newParticipant.points = item.Value;
            participants.Add(newParticipant);
        }
        return participants;
    }

}



