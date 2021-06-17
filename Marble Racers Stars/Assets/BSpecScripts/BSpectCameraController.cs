using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using MyBox;
public class BSpectCameraController : MonoBehaviour
{
    [SerializeField] BSpecSectorCamera[] cams = null;
    [SerializeField] CinemachineVirtualCamera onBoardCam = null;
    [SerializeField] CinemachineVirtualCamera zenithCam = null;
    [SerializeField] private BSpecZoomCamera zoomCam = null;
    int current = 0;
    public CinemachineBrain brain;
    private Marble marbleTarget = null;
    public Marble MarbleTarget { get { return marbleTarget; } set { marbleTarget = value; onTargetChanged?.Invoke(marbleTarget.transform); } }
    private BSpecMode bMode = BSpecMode.FreeMode;
    public BSpecMode Mode { get { return bMode; } set { bMode = value; SetCameraByMode(); } }
    public CinemachineVirtualCamera currentVirtualCamera { get; private set;}
    public bool isMainController { get; set; }
    public event System.Action<BSpecSectorCamera> onPriorityChanged = null;
    public event System.Action<Transform> onTargetChanged = null;

    private IEnumerator Start()
    {
        if (zoomCam != null)
        { 
            zoomCam.onScrollingZoom += Zooming;
        }
        while (RacersSettings.GetInstance().GetMarbles()[0] == null)
        {
            yield return null;
        }
        MarbleTarget = RacersSettings.GetInstance().GetMarbles()[0];
        SetVCam(cams[current].compVirtual);
    }
    private void Update()
    {
        if (!isMainController) return;
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Mode = BSpecMode.FreeMode;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            Mode = BSpecMode.Manual;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            Mode = BSpecMode.Zenith;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            Mode = BSpecMode.OnBoard;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            PreviousSector();
        if (Input.GetKeyDown(KeyCode.RightArrow))
            NextSector();
    }

    private void SetCameraByMode() 
    {
        switch (bMode)
        {
            case BSpecMode.FreeMode:
                AddSectorListeners();
                SetVCam(cams[current].compVirtual);
                break;
            case BSpecMode.Manual:
                FocusOnSectorAutomatically(cams[current]);
                SetVCam(cams[current].compVirtual);
                RemoveAllSectorListeners();
                break;
            case BSpecMode.OnBoard:
                SetVCam(onBoardCam);
                onBoardCam.Priority = 100;
                zenithCam.Priority = 0;
                onPriorityChanged?.Invoke(null);
                RemoveAllSectorListeners();
                break;
            case BSpecMode.Zenith: 
                SetVCam(zenithCam);
                zenithCam.Priority = 100;
                onBoardCam.Priority = 0;
                onPriorityChanged?.Invoke(null);
                RemoveAllSectorListeners();
                break;
        }
    }
    private void OnDisable()
    {
        RemoveAllSectorListeners();
    }

    private void RemoveAllSectorListeners() => System.Array.ForEach(cams, x => x.onMarbleTargetEntered -= FocusOnSectorAutomatically);
    private void AddSectorListeners() => System.Array.ForEach(cams, x => x.onMarbleTargetEntered += FocusOnSectorAutomatically);

    private int FocusOnSectorAutomatically(BSpecSectorCamera sector) 
    {
        current = System.Array.IndexOf(cams,sector);
        SetVCam(sector.compVirtual);
        onPriorityChanged?.Invoke(sector);
        return 100;
    }

    private void NextSector() 
    {
        if (bMode != BSpecMode.Manual) return;
        current++;
        if (current >= cams.Length)
            current = 0;
        onPriorityChanged?.Invoke(cams[current]);
        cams[current].compVirtual.Priority = 100;
    }

    private void PreviousSector() 
    {
        if (bMode != BSpecMode.Manual) return;
        current--;
        if (current < 0)
            current = cams.Length -1;
        onPriorityChanged?.Invoke(cams[current]);
        cams[current].compVirtual.Priority = 100;
    }

    public void SetVCam(CinemachineVirtualCamera vCam) 
    {
        currentVirtualCamera = vCam;
    }

    public void SwitchTracking()
    {
        System.Array.ForEach(currentVirtualCamera.GetComponentPipeline(), delegate (CinemachineComponentBase x)
        {
            if (x.GetType() == typeof(CinemachineComposer))
                x.enabled = !x.enabled;
        });
    }

    private void Zooming(float amount) 
    {
        float prev = currentVirtualCamera.m_Lens.FieldOfView;
        float sum = prev + amount;
        currentVirtualCamera.m_Lens.FieldOfView = Mathf.Clamp(sum, 10, 50);
    }
    [ButtonMethod]
    private  void KoolCositas() 
    {
        BSpecSectorCamera[] sectors = GameObject.FindObjectsOfType<BSpecSectorCamera>();

        foreach (var item in cams)
        {
            BSpecSectorCamera spec = System.Array.Find(sectors, x=> x.name.Equals(item.name) && !ReferenceEquals(x,item));
            //item.detectorSector = spec.detectorSector;
            item.transform.position = spec.transform.position;
            item.transform.rotation = spec.transform.rotation;
        }
    }
}

public enum BSpecMode 
{
    FreeMode,
    Zenith,
    OnBoard,
    Manual
}
