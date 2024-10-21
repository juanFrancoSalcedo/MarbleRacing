using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using DG.Tweening;
using LeagueSYS;

public class Marble : MonoBehaviour, IMainExpected
{
    public Sector beforeSector { get; private set; }
    public Sector currentSector = null;
    public int currentMarbleLap = 0;
    public bool fell { get; set; }
    public bool isPlayer = false;
    public bool justVisualAward = false;
    public bool isZombieQualy = false;
    private bool broken = false;
    #region Variables Race End
    public int finalPosition { get; set; } = 32;
    public bool raceEnded { get; set; }
    public int scorePartial { get; set; }
    #endregion
    public BoardUIController boardController = null;
    public float distanceBetweenSector { get; set; }
    private float trailDistanceAmount = 0;
    public int sectorsPassed { get; private set; }
    public MarbleData marbleInfo { get; private set; }
    
    public Rigidbody rb { get; private set; }
    public Renderer renderCompo { get; private set; }
    private GameObject objInside;
    private GameObject trail;
    [SerializeField] DataController dataAllMarbles;
    #region Variables Stats
    private float handicap;
    public float rightEnergy { get; private set; } = Constants.timeDriving;
    public float leftEnergy { get; private set; } = Constants.timeDriving;
    public float frontEnergy { get; private set; } = Constants.timeAceleration;
    private MarbleStats stats;
    private MarbleStats initStats;
    public MarbleStats InitStats => initStats;
    public MarbleStats Stats
    {
        get{return stats;}
        private set{}
    }
    private bool inPitStop;
    public bool InPitStop { get { return inPitStop; } set { inPitStop = value; CheckMarbleAICanStopInPits(); } }
    public Collider m_collider { get; private set; } = null;
    private GameObject brokenMarbleModel = null;
    private GameObject dirtyMat = null;
    private BrokenMarblePart[] brokenPart;
    public event System.Action onForceApplied = null;

    [Header("~~~~~~~ Pits ~~~~~~~")]
    public TypeCovering marbleCovering = TypeCovering.Medium;
    public int pitStopCount { get; private set; } =0;
    #endregion
    private bool outOfTrack;
    public event System.Action<float> OnTrackSpeed;
    public event System.Action<bool> OnTheTrack;
    public event System.Action OnRespawn;
    //No Quite System
    public string namePilot {get;set;}
    public int idPilot {get;set;}
    [Header("~~~~~~~ Powers ~~~~~~~")]
    [SerializeField] private GameObject freezeModel = null;
    [SerializeField] private GameObject explotionParticles = null;
    public event System.Action<PowerUpType> OnPowerUpObtained;
    public event System.Action<PowerUpType> OnPowerUpDelivered;
    private CollisionDetector colliDetector;
    private PowerUpType m_powerObtained = PowerUpType.None;
    public System.Action onLapWasSum = null;
    
    private void Awake()
    {
        renderCompo = GetComponent<Renderer>();
        m_collider = GetComponent<Collider>();
        colliDetector = GetComponent<CollisionDetector>();
        colliDetector.OnCollisionEntered += CollisionWithOtherCompetitor;
        SubscribeToMainMenu();
        beforeSector = currentSector;
    }
    void Start()
    {
        if (justVisualAward) return;
        if (RaceController.Instance != null)
        {
            RaceController.Instance.OnCountTrafficLigthEnded += FirstImpulse;
            SetRigidbody();
            rb.isKinematic = true;
            RaceController.Instance.onQualifiyingCompleted += BreakByEndQualifying;
            dataAllMarbles = RaceController.Instance.dataManager;
        }
    }

    private void Update()
    {
        if (!raceEnded && !justVisualAward)
        {
            if (currentSector != null)
            {
                distanceBetweenSector = currentSector.distanceBetweenNext - Vector3.Distance(transform.position, currentSector.nextSector.transform.position);
            }
            if (boardController != null)
                boardController.BoardParticip.score = trailDistanceAmount + distanceBetweenSector;
            if (RaceController.Instance != null && RaceController.Instance.stateOfRace == RaceState.Racing) 
            {
                leftEnergy = ChargeEnergyDriving(leftEnergy);
                rightEnergy = ChargeEnergyDriving(rightEnergy);
                frontEnergy = ChargeEnergyFront(frontEnergy);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && isPlayer)
            ApplyForce();
    }
    #region IMainSpected Methods
    public void SubscribeToMainMenu()
    {
        if (justVisualAward)
        {
            return;
        }
        MainMenuController.GetInstance().OnRaceReady += ReadyToPlay;
    }
    public void ReadyToPlay()
    {
        renderCompo.enabled = true;
        if (objInside != null) objInside.SetActive(true);
        SetBoardTransforms();
    }

    private void SetBoardTransforms() 
    {
        boardController.bufferMarble = this;
        boardController.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 100);
        boardController.textName.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 60);
        boardController.textName.GetComponent<RectTransform>().localPosition = new Vector2(-20, 0);
        boardController.textScores.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 60);
        boardController.textScores.GetComponent<RectTransform>().localPosition = new Vector2(180,0);
    }
    #endregion

    public void UpdateBoardForBroadcasting() 
    {
        boardController.StartAnimation("<POS>",namePilot,"tiempo o distancia",false,marbleInfo.spriteMarbl);
        boardController.UpdateAnimation = true;
    }

    public void FirstImpulse()
    {
        SetRigidbody();
        rb.isKinematic = false;
        StartCoroutine(ContinuesImpulse());
        float hancdicap = (isPlayer) ? 1 : 1 + +RaceController.Instance.GetHandicapByLeagueSaved(this);
        rb.AddForce(Vector3.forward * Random.Range(21f, 24f) * hancdicap, ForceMode.Impulse);
        renderCompo.enabled = true;
        objInside?.SetActive(true);
        if (!isPlayer || RacersSettings.GetInstance().Broadcasting())
        {
            StartCoroutine(AddForceByTime(frontEnergy));
            StartCoroutine(AddForceByTimeDirection(leftEnergy, false));
            StartCoroutine(AddForceByTimeDirection(rightEnergy, true));
        }
    }

    private void SetRigidbody()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void AddDistance(Sector newSector)
    {
        if (ReferenceEquals(newSector, currentSector.nextSector))
        {
            beforeSector = currentSector;
            trailDistanceAmount += currentSector.distanceBetweenNext;
            currentSector = newSector;
            sectorsPassed++;
        }
        else if (!ReferenceEquals(newSector, currentSector) && !ReferenceEquals(newSector, beforeSector))
        {
            print("Penalizacion por pasarce el sector" + currentSector.name + " -al- " + newSector.name + "@" + name);
            RespawnMarble();
        }
    }
    #region Force 
    private float ChargeEnergyFront(float energyDesire)
    {
        if (isZombieQualy)
            return 0.3f;

        if (energyDesire < Constants.timeAceleration - stats.coldTimeTurbo)
            energyDesire += Time.deltaTime;

        return energyDesire;
    }

    private float ChargeEnergyDriving(float energyDesire)
    {
        if (isZombieQualy)
            return 1;
        if (energyDesire < Constants.timeDriving -stats.coldTimeDirection)
            energyDesire += Time.deltaTime;
        return energyDesire;
    }

    private IEnumerator AddForceByTimeDirection(float energy, bool directionRight)
    {
        while (true)
        {
            yield return new WaitForSeconds(GetTimeBetwent(Constants.timeDriving));
            if (energy >= Constants.timeDriving)
            {
                ApplyForce(directionRight);
                if (directionRight) rightEnergy = 0; else leftEnergy = 0;
            }
        }
    }

    private IEnumerator AddForceByTime(float energy)
    {
        while (true)
        {
            yield return new WaitForSeconds(GetTimeBetwent(Constants.timeAceleration));
            if (energy >= Constants.timeAceleration)
            {
                ApplyForce();
                frontEnergy = 0;
            }
        }
    }

    private void ApplyForce() 
    {
        float statsForce = (stats == null) ? 0.3f : stats.forceDirection;
        rb.AddForce(currentSector.transform.forward *statsForce*GetMultiplicatorByPosition()*coveringSpeedMultiplier, ForceMode.Impulse);
        rb.AddTorque((rb.linearVelocity+currentSector.transform.forward)/4,ForceMode.Impulse);
        onForceApplied?.Invoke();
        IncreaseFriction();
    }
    private void ApplyForce(bool directionRight)
    {
        float statsForce = (stats == null)?1:stats.forceDirection;

        if (directionRight)
            rb.AddForce((currentSector.transform.forward / 4 + currentSector.transform.right)* statsForce * GetMultiplicatorByPosition(), ForceMode.Impulse);
        else
            rb.AddForce((currentSector.transform.forward / 4 - currentSector.transform.right)* statsForce * GetMultiplicatorByPosition(), ForceMode.Impulse);
    }

    public void ApplyForceLimited()
    {
        ApplyForce();
        frontEnergy = 0;
    }
    public void ApplyForceLimited(bool directionRight)
    {
        ApplyForce(directionRight);
        if (directionRight) rightEnergy = 0; else leftEnergy = 0;
    }

    private float GetTimeBetwent(float arg1)
    {
        RangedFloat timeThreshold = new RangedFloat() { Min = arg1, Max = arg1 + (arg1 / 4) };
        return Random.Range(timeThreshold.Min, timeThreshold.Max);
    }
    #endregion
    #region Collisions
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Track"))
        {
            SoundHitTrack(true);
            CancelInvoke("RespawnByTime");
            outOfTrack = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Track") && isPlayer)
        {
            OnTrackSpeed?.Invoke(rb.linearVelocity.magnitude);
            SoundHitTrack(collision);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Track"))
        {
            outOfTrack = true;
            SoundHitTrack(false);
            Invoke("RespawnByTime", 3f);
        }
    }
    #endregion
    #region SoundsMarble
    private void SoundHitTrack(bool touching)
    {
        if (PoolAmbientSounds.Instance)
        {
            PoolAmbientSounds.Instance.PushShoot(SoundType.CollisionMarbleTrack, transform.position, renderCompo.isVisible);
        }
        OnTheTrack?.Invoke(touching);
    }

    private void SoundHitTrack(Collision colli)
    {
        if (colli.contactCount > 1 && isPlayer)
        {
            foreach (var item in colli.contacts)
            {
                if (item.separation > 0.5f && canHitSound)
                {
                    if (PoolAmbientSounds.Instance)
                    {
                        canHitSound = false;
                        PoolAmbientSounds.Instance.PushShoot(SoundType.CollisionMarbleTrack, transform.position, renderCompo.isVisible);
                        Invoke("CanPlayTrackSoundHit", 0.1f);
                    }
                }
            }
        }
    }

    private bool canHitSound;
    void CanPlayTrackSoundHit()
    {
        canHitSound = true;
    }
    #endregion
    #region RespawnMethods
    private void RespawnByTime()
    {
        if (outOfTrack && !broken)
            RespawnMarble();
        else
            RespawnMarble(RaceController.Instance.goalFinal.GetComponent<Sector>().nextSector.transform.position);
    }

    public void RespawnMarble()
    {
        fell = true;
        rb.linearVelocity = Vector3.zero;
        //bug fixing: fall before pass through the goal
        if (beforeSector == null)
            beforeSector = RaceController.Instance.sectorInFront;
        transform.position = beforeSector.transform.position;
        Invoke("PushByFell", 1f);
        PoolAmbientSounds.Instance.PushShoot(SoundType.Respawn, (isPlayer) ? Vector3.zero : transform.position, renderCompo.isVisible);
        OnRespawn?.Invoke();
    }
    public void RespawnMarble(Vector3 pos)
    {
        fell = true;
        rb.linearVelocity = Vector3.zero;
        //bug fixing: fall before pass through the goal
        if (beforeSector == null)
            beforeSector = RaceController.Instance.sectorInFront;
        transform.position = pos;
        Invoke("PushByFell", 1f);
        PoolAmbientSounds.Instance.PushShoot(SoundType.Respawn, (isPlayer) ? Vector3.zero : transform.position, renderCompo.isVisible);
        broken = true;
        RestoreStatsToInit();
        RestoreBrokenMarble();
        OnRespawn?.Invoke();
    }

    private void PushByFell()
    {
        fell = false;
        if(gameObject.activeInHierarchy)
            StartCoroutine(PushPermanent());
    }

    IEnumerator PushPermanent()
    {
        while (rb.linearVelocity.magnitude < 10)
        {
            rb.AddForce(currentSector.transform.forward, ForceMode.Impulse);
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

    public void SetMarbleSettings(MarbleData mar)
    {
        if (isPlayer)
        {
            MarbleData bufferPlayer = ScriptableObject.CreateInstance<MarbleData>();
            if (dataAllMarbles == null)
                dataAllMarbles = GameObject.FindObjectOfType<DataController>();
            bufferPlayer = dataAllMarbles.GetCustom();

            renderCompo = renderCompo ?? GetComponent<Renderer>();
            CustomMat(bufferPlayer);
            CustomObjInside(bufferPlayer);
            CustomTrail(bufferPlayer);
            marbleInfo.nameMarble = Constants.NORMI;
        }
        else
        {
            marbleInfo = mar;
            renderCompo = renderCompo ?? GetComponent<Renderer>();

            if (marbleInfo.mat == null)
                renderCompo.enabled = false;
            renderCompo.material = marbleInfo.mat;

            if (marbleInfo.objectInside != null)
            {
                if (objInside != null)
                    Destroy(objInside);
                objInside = Instantiate(marbleInfo.objectInside, transform.position, Quaternion.identity, transform);
                objInside.SetActive(true);
            }
            if (!justVisualAward)
                Invoke("DisableRenderCompo",0.1f); 
        }
        if (justVisualAward) return;
        CreateBrokenMarble();
        CreateMaterialDirty();
        CalculateHandicapLeague();
    }

    private void DisableRenderCompo() 
    {
        renderCompo.enabled = false;
        objInside?.SetActive(false);
    }

    #region Customizing
    public void SetMarbleSettings(int indexInAllMarbles)
    {
        //bufferPlayer = 
        //TODO Esto no iria
        //dataAllMarbles.SetCurrentMarble(indexInAllMarbles);
        marbleInfo = dataAllMarbles.allMarbles.GetSpecificMarble(indexInAllMarbles);
        if (renderCompo == null) { renderCompo = GetComponent<Renderer>(); }
        CustomMat(marbleInfo,indexInAllMarbles);
        CustomObjInside(marbleInfo, indexInAllMarbles);
        CustomTrail(marbleInfo, indexInAllMarbles);
        marbleInfo.nameMarble = Constants.NORMI;
    }

    private void CustomMat(MarbleData data, int indexInAll) 
    {
        if (data.mat!= null) 
        {
             renderCompo.material = data.mat;
            SetMarbleInfoApart(data);
             PlayerPrefs.SetInt(KeyStorage.CUSTOM_MAT_I, indexInAll);
        }
    }
    private void SetMarbleInfoApart(MarbleData data) 
    {
        marbleInfo.spriteMarbl = data.spriteMarbl;
        marbleInfo.color1 = data.color1;
        marbleInfo.color2 = data.color2;
    }
    private void CustomObjInside(MarbleData data,int indexInAll) 
    {
        if (data.objectInside != null)
        {
            if (objInside != null) 
                Destroy(objInside);
            if (data.objectInside != null) 
               objInside = Instantiate(data.objectInside, transform.position, Quaternion.identity, transform);
            if (objInside.GetComponent<Rigidbody>())
                Destroy(objInside.GetComponent<Rigidbody>());
            dataAllMarbles.SetSpecificKeyInt(KeyStorage.CUSTOM_OBJ_INSIDE_I, indexInAll);
        }
    }
    private void CustomTrail(MarbleData data,int indexInAll) 
    {
        if (data.objectInside != null)
        {
            if (trail != null) 
                Destroy(trail);
            if (data.objectInside != null) 
                trail = Instantiate(data.objectInside, transform.position, Quaternion.identity, transform);
            if (trail.GetComponent<Rigidbody>())
                Destroy(trail.GetComponent<Rigidbody>());
            dataAllMarbles.SetSpecificKeyInt(KeyStorage.CUSTOM_TRAIL_I,indexInAll);
        }
    }
    private void CustomMat(MarbleData data)
    {
        if (data.mat != null)
        {
            renderCompo.material = data.mat;
            marbleInfo = data;
        }
    }
    private void CustomObjInside(MarbleData data)
    {
        if (data.objectInside != null)
        {
            if (objInside != null) Destroy(objInside);
            if (data.objectInside != null) objInside = Instantiate(data.objectInside, transform.position, Quaternion.identity, transform);

        }
    }
    private void CustomTrail(MarbleData data)
    {
        if (data.objectInside != null)
        {
            if (trail != null) Destroy(trail);
            if (data.objectInside != null) trail = Instantiate(data.objectInside, transform.position, Quaternion.identity, transform);
        }
    }
    #endregion
    #region Dificulty

    private float GetMultiplicatorByPosition()
    {
        if (isZombieQualy)
            return 5;
        float multiplicator = boardController.transform.GetSiblingIndex() > 6 ?4f:3f;
        multiplicator += handicap;
        return multiplicator;
    }
    void CalculateHandicapLeague()
    {
        if (RaceController.Instance != null)
            handicap += RaceController.Instance.GetHandicapByLeagueSaved(this);
    }

    #endregion
    #region PowerUpEnchants
    private void MakeDamage() 
    {
        if (CheckResistDamage()) 
            return;
        stats.hp--;
        ActiveBrokenMarblePartByDamage();
        ShowPitsIsNecesary();
        if (stats.hp <= 0)
        {
            BrokeMarble();
            stats.hp = initStats.hp;
        }
    }
    public void CollisionWithOtherCompetitor(Collision collis)
    {
        Marble otherMarble = collis.gameObject.GetComponent<Marble>()?? null;
        if (otherMarble == null) return;
        BecameZombieQualifying(otherMarble);
        MakeDamage();
        SendBigPush(otherMarble);
        if (m_powerObtained != PowerUpType.None)
        {
            otherMarble.Enchant(m_powerObtained);
            m_powerObtained = PowerUpType.None;
            OnPowerUpDelivered?.Invoke(m_powerObtained);
        }
        PoolAmbientSounds.Instance.PushShoot(SoundType.CollisionMarbleMarble, transform.position, renderCompo.isVisible);
    }

    private void SendBigPush(Marble other) 
    {
        if (transform.localScale.x>1 && m_powerObtained == PowerUpType.Enlarge
            && other.boardController.transform.GetSiblingIndex() > boardController.transform.GetSiblingIndex())
        { 
            other.BigPush(transform);
            if (other.isPlayer && !RacersSettings.GetInstance().Broadcasting())
                CameraShake.Instance.Shake();
        }
    }
    public void BigPush(Transform positionRival) 
    {
        rb.AddForce((transform.position-positionRival.position)*2,ForceMode.Impulse);
        PoolParticles.Instance.ActiveSearchParticles(transform.position,TypeParticle.StarExplo);
    }
    public void SetPowerUp(PowerUpType _typerPowe)
    {
        m_powerObtained = _typerPowe;
        OnPowerUpObtained?.Invoke(_typerPowe);
        if (_typerPowe == PowerUpType.Shrink
            || _typerPowe == PowerUpType.Enlarge
            || _typerPowe == PowerUpType.Wall)
        {
            Enchant(_typerPowe);
            m_powerObtained = PowerUpType.None;
        }
        PoolAmbientSounds.Instance.PushShoot(SoundType.BoxPowerUp, (isPlayer) ? Vector3.zero : transform.position, renderCompo.isVisible);
    }
    public void Enchant(PowerUpType _typerPower)
    {
        switch (_typerPower)
        {
            case PowerUpType.Freeze:
                freezeModel.SetActive(true);
                rb.linearVelocity = Vector3.zero;
                Invoke("CleanIce", 3);
                PoolAmbientSounds.Instance.PushShoot(SoundType.FreezePow, (isPlayer) ? Vector3.zero : transform.position, renderCompo.isVisible);
                IncreaseFriction();
                break;

            case PowerUpType.Shrink:
                CancelInvoke("RestoreMarbleScale");
                transform.DOScale(Vector3.one * 0.6f, 0.8f).SetEase(Ease.InElastic);
                rb.mass = 0.6f;
                Invoke("RestoreMarbleScale", Constants.timeBigSize);
                IncreaseFriction();
                break;

            case PowerUpType.Enlarge:
                CancelInvoke("RestoreMarbleScale");
                transform.DOScale(Vector3.one * 2f, 0.8f).SetEase(Ease.InElastic);
                ApplyForce();
                ApplyForce();
                ApplyForce();
                rb.mass = 2f;
                Invoke("RestoreMarbleScale", Constants.timeBigSize);
                PoolAmbientSounds.Instance.PushShoot(SoundType.EnlargePow, (isPlayer) ? Vector3.zero : transform.position, renderCompo.isVisible);
                IncreaseFriction();
                break;

            case PowerUpType.Explo:
                explotionParticles.SetActive(false);
                explotionParticles.SetActive(true);
                rb.AddForce(currentSector.transform.up * 9, ForceMode.Impulse);
                rb.linearVelocity = rb.linearVelocity * 0.8f;
                PoolAmbientSounds.Instance.PushShoot(SoundType.ExploPow, (isPlayer) ? Vector3.zero : transform.position, renderCompo.isVisible);
                IncreaseFriction();
                break;

            case PowerUpType.Wall:
                Marble marbleFirst = RaceController.Instance.GetMarbleByPosition(0);
                if (marbleFirst == null)
                { //en caso que ninguna marbles haya pasado la linea de meta
                    Vector3 posFirstSector = currentSector.nextSector.nextSector.nextSector.transform.position;
                    Vector3 posFrontSector = posFirstSector + currentSector.nextSector.nextSector.nextSector.transform.forward * 6;
                    PoolPowerUps.Instance.CreatePow(posFrontSector, currentSector.nextSector.nextSector.nextSector.transform.rotation, _typerPower);
                }
                else
                {
                    Vector3 posFirstsector = marbleFirst.currentSector.nextSector.transform.position;
                    Vector3 posFrontSector = posFirstsector + marbleFirst.currentSector.nextSector.transform.forward * 6;
                    PoolPowerUps.Instance.CreatePow(posFrontSector, marbleFirst.currentSector.nextSector.transform.rotation, _typerPower);
                }
                break;

            case PowerUpType.Bump:
                Vector3 dirVelo = transform.position - rb.linearVelocity.normalized * 3;
                PoolPowerUps.Instance.CreatePow(dirVelo, transform.rotation, _typerPower);
                break;
        }
    }
    public bool CheckHasPower() => m_powerObtained != PowerUpType.None ? true : false;
    public void CleanIce()
    {
        if (freezeModel.activeInHierarchy)
        {
            freezeModel.SetActive(false);
            ApplyForce();
        }
    }

    public void RestoreMarbleScale()
    {
        if (transform.localScale.z < 0.7f)
        {
            transform.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutElastic);
            rb.mass = 1f;
        }

        if (transform.localScale.z > 1)
        {
            transform.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutElastic);
            rb.mass = 1f;
        }
        PoolAmbientSounds.Instance.PushShoot(SoundType.RestoreSize, (isPlayer) ? Vector3.zero : transform.position, renderCompo.isVisible);
    }
    #endregion
    #region StatsMethods

    public void SetStats(MarbleStats _stats) 
    {
        stats = _stats;
        initStats = new MarbleStats();
        initStats.coldTimeDirection = stats.coldTimeDirection;
        initStats.coldTimeTurbo = stats.coldTimeTurbo;
        initStats.forceTurbo = stats.forceTurbo;
        initStats.forceDirection = stats.forceDirection;
        initStats.hp = stats.hp;
    }

    private void RestoreStatsToInit() 
    {
        stats.coldTimeDirection = initStats.coldTimeDirection;
        stats.coldTimeTurbo = initStats.coldTimeTurbo;
        stats.forceTurbo = initStats.forceTurbo;
        stats.forceDirection = initStats.forceDirection;
        stats.hp = initStats.hp;
    }
    void CheckMarbleAICanStopInPits() 
    {
        if (!isPlayer || RacersSettings.GetInstance().Broadcasting())
        {
            if (inPitStop &&
                RaceController.Instance.stateOfRace == RaceState.Racing &&
                (m_collider.material.dynamicFriction > 0.2f || stats.hp < 60f) &&
                CheckUsedAllItPitStops())
                PitStop((TypeCovering)Random.Range(0,3));
        }
    }
    private void IncreaseFriction() 
    {
        return;
        m_collider.material.dynamicFriction += Random.Range(Constants.frictionBase* coveringDirtMultiplier, (Constants.frictionBase*2)* coveringDirtMultiplier)
            * LeagueManager.LeagueRunning.GetFriction();
        m_collider.material.dynamicFriction = Mathf.Clamp(m_collider.material.dynamicFriction,0, 0.2f);
        dirtyMat.GetComponent<DirtyMaterialHandler>().UpdateFrictionDirty(1-(m_collider.material.dynamicFriction*4f));
        ShowPitsIsNecesary();
    } 
    public void PitStop(TypeCovering covering) 
    {
        if (inPitStop)
        { 
            RestoreStatsToInit();
            m_collider.material.dynamicFriction = 0f;
            dirtyMat.GetComponent<DirtyMaterialHandler>().RestoreShader();
            System.Array.ForEach(brokenPart, part => part.gameObject.SetActive(false));
            Vector3 veloBuffer = rb.linearVelocity;
            rb.linearVelocity = veloBuffer / 2;
            pitStopCount++;
            SettingsTypeCovering(covering);
            if (isPlayer && !RacersSettings.GetInstance().Broadcasting())
                PitWarning.Instance.DisableWarningPits();
        }
    }
    private void SettingsTypeCovering(TypeCovering covering) 
    {
        switch (covering) 
        {
            case TypeCovering.SoftRough:
                coveringSpeedMultiplier = 2;
                coveringDirtMultiplier = 2;
                m_collider.material.bounciness = 0.2f;
                coveringDamageResistance = false;
                break;

            case TypeCovering.Medium:
                coveringSpeedMultiplier = 1;
                coveringDirtMultiplier = 1;
                m_collider.material.bounciness = 0.2f;
                coveringDamageResistance = false;
                break;

            case TypeCovering.HardElastic:
                coveringSpeedMultiplier = 1;
                coveringDirtMultiplier = 1;
                m_collider.material.bounciness = 0.35f;
                coveringDamageResistance = true;
                break;
        }
        marbleCovering = covering;
    }
    private int coveringSpeedMultiplier = 1;
    private int coveringDirtMultiplier = 1;
    private bool coveringDamageResistance = false;
    private bool CheckResistDamage()=>(Random.Range(0f,1f)>=0.5f);

    public void FullStats() 
    {
        stats.forceTurbo = Constants.forceBaseForward*Constants.fractionStats;
        stats.forceDirection = Constants.forceBaseDriving* Constants.fractionStats;
        stats.coldTimeTurbo = Constants.timeReduceAcelerationBase* Constants.fractionStats;
        stats.coldTimeDirection = Constants.timeReduceDrivingBase* Constants.fractionStats;
        stats.hp = Constants.baseHp* Constants.fractionStats;
    }
    private void CreateBrokenMarble() 
    {
        brokenMarbleModel =Instantiate(PoolPowerUps.Instance.brokenMarble,transform);
        brokenMarbleModel.transform.localPosition = Vector3.zero;
        brokenPart = brokenMarbleModel.transform.GetComponentsInChildren<BrokenMarblePart>();
        System.Array.ForEach(brokenPart, part => { part.gameObject.SetActive(false);part.SetMaterial(renderCompo.material); });
        RandomPartsIndexs();
    }
    private void CreateMaterialDirty() 
    {
        dirtyMat =Instantiate(PoolPowerUps.Instance.materialDirty,transform);
        dirtyMat.transform.localPosition = Vector3.zero;
        dirtyMat.GetComponent<DirtyMaterialHandler>().RestoreShader();
    }
    private void ActiveBrokenMarblePartByDamage() 
    {
        if (stats.hp <= 0) return;
        int average = stats.hp*brokenPart.Length;
        int result = average / initStats.hp;
        if (SeekPart(out int san, result)) 
        {
            brokenPart[listRandom[san]].gameObject.SetActive(true);
        }
    }

    private bool SeekPart(out int realResult, int _result) 
    {
        realResult = (brokenPart.Length - _result - 2 <0)? 0 : (brokenPart.Length - _result-2);
        if (realResult >= listRandom.Count || realResult < 0)
            print(brokenPart.Length+"-"+_result+"-2 ="+ realResult);
        return ((brokenPart.Length - _result - 2) > 0);//(brokenPart.Length - _result) - 1;
    }
    private List<int> listRandom = new List<int>();
    private void RandomPartsIndexs()
    {
        listRandom = UniqueList.CreateRandomListWithoutRepeating(0, brokenPart.Length, brokenPart.Length);
    }

    private async void BrokeMarble() 
    {
        System.Array.ForEach(brokenPart,part =>part.BrokePart());
        rb.isKinematic = true;
        renderCompo.enabled = false;
        m_collider.isTrigger = true;
        brokenMarbleModel = null;
        broken = true;
        if (RacersSettings.GetInstance().Broadcasting())
        {
           await BSpecManager.Instance.AMarbleBroke(this);
        }
    }
    private void RestoreBrokenMarble() 
    {
        RestoreStatsToInit();
        rb.isKinematic = false;
        renderCompo.enabled = true;
        m_collider.isTrigger = false;
        CreateBrokenMarble();
        dirtyMat.GetComponent<DirtyMaterialHandler>().RestoreShader();
        broken = false;
    }
    private void ShowPitsIsNecesary() 
    {
        if (!isPlayer || RacersSettings.GetInstance().Broadcasting()) return;
        if (m_collider.material.dynamicFriction > 0.2f || stats.hp < 30)
            PitWarning.Instance.ActiveWarningPits();
    }
    public bool CheckUsedAllItPitStops() => pitStopCount < RaceController.Instance.minPitsStops + (isPlayer ? 0 : 1);
    #endregion
    #region Qualifying 
    private void BecameZombieQualifying(Marble marble) 
    {
        if(!LeagueManager.LeagueRunning.GetIsQualifying()) return;
        if (isZombieQualy && !marble.isZombieQualy)
        {
            marble.isZombieQualy = RaceController.Instance.AddMarbleZombie(marble.gameObject);
            if (marble.isZombieQualy)
            {
                PoolImages.Instance.PushImage(marble.marbleInfo.spriteMarbl);
                marble.renderCompo.material = PoolPowerUps.Instance.materialZombie;
            }
        }
    }
    private void BreakByEndQualifying() 
    {
        if (!isZombieQualy) return;
        StartCoroutine(StopMarbles());
    }

    private IEnumerator StopMarbles() 
    {
        StopCoroutine(AddForceByTime(0));
        StopCoroutine(AddForceByTimeDirection(0, false));
        StopCoroutine(ContinuesImpulse());
        StopCoroutine(PushPermanent());
        gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator ContinuesImpulse()
    {
        while (isZombieQualy)
        {
            if (rb.linearVelocity.magnitude < 10)
                rb.AddForce(currentSector.transform.forward, ForceMode.Impulse);
            yield return new WaitForEndOfFrame();
            rb.isKinematic = false;
        }
    }
#endregion
}
