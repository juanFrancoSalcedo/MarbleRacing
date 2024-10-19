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
    DataController _dataController = null;


    void Start()
    {
        _dataController = DataController.Instance;
        collectorDebt.OnTrhopyWasUnlocked += LastUnlocked;
        cupsWon = _dataController.GetCupsWon();
        cupsUnlocked = _dataController.GetSpecificKeyInt(KeyStorage.CUPSUNLOCKED_I);
        FillRenderComponents();
        if (cupsWon < numberCup)
            ActiveMaterials(false);
        if (cupsUnlocked >= numberCup)
            padLock.SetActive(false);
        if (_dataController.GetSpecificKeyInt(KeyStorage.CURRENTCUP_I) == numberCup)
            FocusCamera();
        //if (LeagueManager.LeagueRunning.date >= LeagueManager.LeagueRunning.listPrix.Count)
        //    ActiveCurrenCupIsThis();
    }

    private void FillRenderComponents()
    {
        if (GetComponent<Renderer>())
        {
            renderComps.Add(GetComponent<Renderer>());
            materialsById.Add(gameObject.GetInstanceID(),GetComponent<Renderer>().material);
        }
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
        if (EventSystem.current.IsPointerOverGameObject()) 
            return;
        if (cupsUnlocked >= numberCup)
        {
            if (LeagueManager.LeagueRunning != null && LeagueManager.LeagueRunning.date>0)
            {
                WarningTrophyChange.Instance.gameObject.SetActive(true);
                WarningTrophyChange.Instance.ChooseCup(this);
            }
            else
                ActiveCurrenCupIsThis();
        }
        else
            ActiveDebtCollector();
        print(cupsUnlocked + " CUPS" + _dataController.allCups.listCups[numberCup].nameLeague);
    }

    public void ActiveCurrenCupIsThis(int cup)
    {
        if (_dataController.GetSpecificKeyInt(KeyStorage.CURRENTCUP_I) == numberCup)
        {
            LeagueManager.ClearLeague();
            FocusCamera();
            TracksShow.Instance.ShowTracks(DataController.Instance.allCups.listCups[numberCup]);
        }
    }


    public void ActiveCurrenCupIsThis()
    {
        _dataController.EraseLeague();
        _dataController.SetSpecificKeyInt(KeyStorage.CURRENTCUP_I, numberCup);
        if (_dataController.GetSpecificKeyInt(KeyStorage.CURRENTCUP_I) == numberCup)
        {
            LeagueManager.ClearLeague();
            _dataController.EraseLeague();
            FocusCamera();
            TracksShow.Instance.ShowTracks(DataController.Instance.allCups.listCups[numberCup]);
        }
    }
    public void FocusCamera()
    {
        Camera.main.GetComponent<SwipeMovement>().FollowTrophyPosition(transform.position);
        imageCurrentTournament.transform.SetParent(transform);
        imageCurrentTournament.SetActive(false);
        imageCurrentTournament.transform.localPosition = new Vector3(3, 0, 0);
        imageCurrentTournament.transform.localScale = Vector3.zero;
        imageCurrentTournament.transform.DOLocalMove(new Vector3(4, 4, -2.5f), 0.9f);
        imageCurrentTournament.transform.DOScale(Vector3.one / transform.localScale.x, 0.9f);
        imageCurrentTournament.SetActive(true);
        imageCurrentTournament.transform.GetChild(0).GetComponent<DataShowText>().CalculateAndShowData();
    }

    private void ActiveDebtCollector()
    {
        var req = _dataController.allCups.listCups[numberCup].requerimentsLeague;
        TracksShow.Instance.HidePanel();
        collectorDebt.debt = req.moneyRequeriments;
        collectorDebt.trophiesNecesity = req.trophiesRequeriments;
        collectorDebt.previousCupPasses = req.nameCupPreviousRequeriments;
        collectorDebt.curretnCupName = _dataController.allCups.listCups[numberCup].nameLeague;
        collectorDebt.secondPilot = req.needSecondPilot;
        collectorDebt.ShowDebtRequeriments();
    }

    public void LastUnlocked()
    {
        cupsUnlocked = _dataController.GetSpecificKeyInt(KeyStorage.CUPSUNLOCKED_I);
        print(cupsUnlocked+" CUPS");
        if (cupsUnlocked == numberCup)
        {
            brilliantParticles.SetActive(true);
            padLock.SetActive(false);
            ActiveCurrenCupIsThis();
        }
    }

}
