using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class TrophyUnlocker : MonoBehaviour
{
    [SerializeField] private DataController dataMana = null;
    [SerializeField] private Material matUnlocked = null;
    [SerializeField] private GameObject brilliantParticles = null;
    [SerializeField] private GameObject imageCurrentTournament = null;
    [SerializeField] private GameObject padLock = null;
    [SerializeField] private int numberCup =0;
    [SerializeField] private DebtCollector collectorDebt = null;
    int? cupsWon = null;
    int? cupsUnlocked = null;
    Renderer renderCompo = null;

    void Start()
    {
        collectorDebt.OnTrhopyWasUnlocked += LastUnlocked;
        cupsWon = dataMana.GetCupsWon();
        cupsUnlocked = dataMana.GetSpecificKeyInt(KeyStorage.CUPSUNLOCKED_I);
        renderCompo = GetComponent<Renderer>();

        if (cupsWon >= numberCup)
            renderCompo.material = matUnlocked;

        CheckAutomaticTrophy();

        if (cupsUnlocked >= numberCup)
            padLock.SetActive(false);

        ActiveCurrenCupIsThis(numberCup);
    }
    

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (cupsUnlocked >= numberCup)
        {
            if (LeagueManager.LeagueRunning != null)
            {
                WarningTrophyChange.Instance.ChooseCup(this);
                WarningTrophyChange.Instance.gameObject.SetActive(true);
            }
            else
                ActiveCurrenCupIsThis();
        }
        else
            ActiveDebtCollector();
    }

    public void ActiveCurrenCupIsThis(int cup)
    {
        if (dataMana.GetSpecificKeyInt(KeyStorage.CURRENTCUP_I) == numberCup)
        {
            Camera.main.GetComponent<SwipeMovement>().FollowTrophyPosition(transform.position);
            imageCurrentTournament.transform.SetParent(transform);
            imageCurrentTournament.SetActive(false);
            imageCurrentTournament.transform.localPosition = new Vector3(3, 0, 0);
            imageCurrentTournament.transform.localScale = Vector3.zero;
            imageCurrentTournament.transform.DOLocalMove(new Vector3(3, 9, 0), 0.9f);
            imageCurrentTournament.transform.DOScale(Vector3.one, 0.9f);
            imageCurrentTournament.SetActive(true);
            imageCurrentTournament.transform.GetChild(0).GetComponent<DataShowText>().CalculateAndShowData();
            TracksShow.Instance.ShowTracks(dataMana.allCups.listCups[numberCup]);
        }
    }

    public void ActiveCurrenCupIsThis()
    {
        dataMana.SetSpecificKeyInt(KeyStorage.CURRENTCUP_I, numberCup);
        if (dataMana.GetSpecificKeyInt(KeyStorage.CURRENTCUP_I) == numberCup)
        {
            dataMana.SetSpecificKeyInt(KeyStorage.CURRENTCUP_I, numberCup);
            dataMana.EraseLeague();
            Camera.main.GetComponent<SwipeMovement>().FollowTrophyPosition(transform.position);
            imageCurrentTournament.transform.SetParent(transform);
            imageCurrentTournament.SetActive(false);
            imageCurrentTournament.transform.localPosition = new Vector3(3, 0, 0);
            imageCurrentTournament.transform.localScale = Vector3.zero;
            imageCurrentTournament.transform.DOLocalMove(new Vector3(3, 9, 0), 0.9f);
            imageCurrentTournament.transform.DOScale(Vector3.one, 0.9f);
            imageCurrentTournament.SetActive(true);
            imageCurrentTournament.transform.GetChild(0).GetComponent<DataShowText>().CalculateAndShowData();
            TracksShow.Instance.ShowTracks(dataMana.allCups.listCups[numberCup]);
        }
    }

    private void ActiveDebtCollector()
    {
        TracksShow.Instance.HidePanel();
        collectorDebt.debt = dataMana.allCups.listCups[numberCup].requerimentsLeague.moneyRequeriments;
        collectorDebt.trophiesNecesity = dataMana.allCups.listCups[numberCup].requerimentsLeague.trophiesRequeriments;
        collectorDebt.previousCupPasses = dataMana.allCups.listCups[numberCup].requerimentsLeague.nameCupPreviousRequeriments;
        collectorDebt.curretnCupName = dataMana.allCups.listCups[numberCup].nameLeague;
        collectorDebt.secondPilot = dataMana.allCups.listCups[numberCup].requerimentsLeague.needSecondPilot;
        collectorDebt.gameObject.SetActive(false);
        collectorDebt.gameObject.SetActive(true);
    }

    public void LastUnlocked()
    {
        cupsUnlocked = dataMana.GetSpecificKeyInt(KeyStorage.CUPSUNLOCKED_I);
        if (cupsUnlocked == numberCup)
        {
            brilliantParticles.SetActive(true);
            padLock.SetActive(false);
            ActiveCurrenCupIsThis();
        }
    }

    private void CheckAutomaticTrophy()
    {
        if (numberCup >= dataMana.allCups.listCups.Count) return;

        if (collectorDebt.CheckCanIPay(dataMana.allCups.listCups[numberCup].requerimentsLeague.moneyRequeriments,
            dataMana.allCups.listCups[numberCup].requerimentsLeague.trophiesRequeriments,
            dataMana.allCups.listCups[numberCup].requerimentsLeague.nameCupPreviousRequeriments,
            dataMana.allCups.listCups[numberCup].requerimentsLeague.needSecondPilot) && 
            numberCup > dataMana.GetSpecificKeyInt(KeyStorage.CUPSUNLOCKED_I))
            ActiveDebtCollector();
    }
}
