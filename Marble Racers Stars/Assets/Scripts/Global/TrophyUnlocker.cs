using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class TrophyUnlocker : MonoBehaviour
{
    [SerializeField] private GameObject brilliantParticles = null;
    [SerializeField] private GameObject imageCurrentTournament = null;
    [SerializeField] private GameObject padLock = null;
    [SerializeField] private int numberCup =0;
    [SerializeField] private DebtCollector collectorDebt = null;
    [SerializeField] private Material matLocked = null;
    int? cupsWon = null;
    int? cupsUnlocked = null;

    List<Renderer> renderComps = new List<Renderer>();
    Dictionary<int, Material> materialsById = new Dictionary<int, Material>();


    void Start()
    {
        collectorDebt.OnTrhopyWasUnlocked += LastUnlocked;
        cupsWon = DataController.Instance.GetCupsWon();
        cupsUnlocked = DataController.Instance.GetSpecificKeyInt(KeyStorage.CUPSUNLOCKED_I);
        FillRenderComponents();

        if (cupsWon < numberCup)
            ActiveMaterials(false);

        CheckAutomaticTrophy();

        if (cupsUnlocked >= numberCup)
            padLock.SetActive(false);

        ActiveCurrenCupIsThis(numberCup);
    }

    private void FillRenderComponents()
    {
        if (GetComponent<Renderer>())
        {
            renderComps.Add(GetComponent<Renderer>());
            materialsById.Add(gameObject.GetInstanceID(),GetComponent<Renderer>().material);
        }
        print(GetComponentsInChildren<Renderer>(true).Length);
        System.Array.ForEach(GetComponentsInChildren<Renderer>(true), r => {
            if(!ReferenceEquals(brilliantParticles,r.gameObject) && !ReferenceEquals(padLock, r.gameObject) && !ReferenceEquals(imageCurrentTournament, r.gameObject))
            {
                if (!materialsById.ContainsKey(r.gameObject.GetInstanceID()))
                {
                    materialsById.Add(r.gameObject.GetInstanceID(), r.material);
                    renderComps.Add(r);
                }
            }
        });
    }

    private void ActiveMaterials(bool unlocked) 
    {
        renderComps.ForEach(r => {
            Material materialReturned;
            if (materialsById.TryGetValue(r.gameObject.GetInstanceID(), out materialReturned))
                r.material = (unlocked)?materialReturned:matLocked;
            });
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
        if (DataController.Instance.GetSpecificKeyInt(KeyStorage.CURRENTCUP_I) == numberCup)
        {
            Camera.main.GetComponent<SwipeMovement>().FollowTrophyPosition(transform.position);
            imageCurrentTournament.transform.SetParent(transform);
            imageCurrentTournament.SetActive(false);
            imageCurrentTournament.transform.localPosition = new Vector3(3, 0, 0);
            imageCurrentTournament.transform.localScale = Vector3.zero;
            imageCurrentTournament.transform.DOLocalMove(new Vector3(4, 4, -2.5f), 0.9f);
            imageCurrentTournament.transform.DOScale(Vector3.one/transform.localScale.x, 0.9f);
            imageCurrentTournament.SetActive(true);
            imageCurrentTournament.transform.GetChild(0).GetComponent<DataShowText>().CalculateAndShowData();
            TracksShow.Instance.ShowTracks(DataController.Instance.allCups.listCups[numberCup]);
        }
    }

    public void ActiveCurrenCupIsThis()
    {
        DataController.Instance.SetSpecificKeyInt(KeyStorage.CURRENTCUP_I, numberCup);
        if (DataController.Instance.GetSpecificKeyInt(KeyStorage.CURRENTCUP_I) == numberCup)
        {
            DataController.Instance.SetSpecificKeyInt(KeyStorage.CURRENTCUP_I, numberCup);
            DataController.Instance.EraseLeague();
            Camera.main.GetComponent<SwipeMovement>().FollowTrophyPosition(transform.position);
            imageCurrentTournament.transform.SetParent(transform);
            imageCurrentTournament.SetActive(false);
            imageCurrentTournament.transform.localPosition = new Vector3(3, 0, 0);
            imageCurrentTournament.transform.localScale = Vector3.zero;
            imageCurrentTournament.transform.DOLocalMove(new Vector3(4, 4, -2.5f), 0.9f);
            imageCurrentTournament.transform.DOScale(Vector3.one / transform.localScale.x, 0.9f);
            imageCurrentTournament.SetActive(true);
            imageCurrentTournament.transform.GetChild(0).GetComponent<DataShowText>().CalculateAndShowData();
            TracksShow.Instance.ShowTracks(DataController.Instance.allCups.listCups[numberCup]);
        }
    }

    private void ActiveDebtCollector()
    {
        TracksShow.Instance.HidePanel();
        collectorDebt.debt = DataController.Instance.allCups.listCups[numberCup].requerimentsLeague.moneyRequeriments;
        collectorDebt.trophiesNecesity = DataController.Instance.allCups.listCups[numberCup].requerimentsLeague.trophiesRequeriments;
        collectorDebt.previousCupPasses = DataController.Instance.allCups.listCups[numberCup].requerimentsLeague.nameCupPreviousRequeriments;
        collectorDebt.curretnCupName = DataController.Instance.allCups.listCups[numberCup].nameLeague;
        collectorDebt.secondPilot = DataController.Instance.allCups.listCups[numberCup].requerimentsLeague.needSecondPilot;
        collectorDebt.gameObject.SetActive(false);
        collectorDebt.gameObject.SetActive(true);
    }

    public void LastUnlocked()
    {
        cupsUnlocked = DataController.Instance.GetSpecificKeyInt(KeyStorage.CUPSUNLOCKED_I);
        if (cupsUnlocked == numberCup)
        {
            brilliantParticles.SetActive(true);
            padLock.SetActive(false);
            ActiveCurrenCupIsThis();
        }
    }

    private void CheckAutomaticTrophy()
    {
        if (numberCup >= DataController.Instance.allCups.listCups.Count) return;

        if (collectorDebt.CheckCanIPay(DataController.Instance.allCups.listCups[numberCup].requerimentsLeague.moneyRequeriments,
            DataController.Instance.allCups.listCups[numberCup].requerimentsLeague.trophiesRequeriments,
            DataController.Instance.allCups.listCups[numberCup].requerimentsLeague.nameCupPreviousRequeriments,
            DataController.Instance.allCups.listCups[numberCup].requerimentsLeague.needSecondPilot) && 
            numberCup > DataController.Instance.GetSpecificKeyInt(KeyStorage.CUPSUNLOCKED_I))
            ActiveDebtCollector();
    }
}
