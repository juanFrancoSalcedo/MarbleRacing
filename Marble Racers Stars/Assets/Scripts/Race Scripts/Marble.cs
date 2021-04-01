using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using DG.Tweening;
using System.Threading.Tasks;
using System.Threading;

public class Marble : MonoBehaviour, IMainExpected
{
    public Sector beforeSector { get; private set; }
    public Sector currentSector;
    public int currentMarbleLap = 0;
    public bool fell { get; set; }
    public BoardUIController boardController;
    public float distanceBetweenSector { get; set; }
    private float trailDistanceAmount;
    public int sectorsPassed { get; private set; }
    public MarbleData marbleInfo { get; private set; }
    public bool isPlayer;
    public bool justVisualAward;
    public bool isZombieQualy;

    #region Variables Race End
    public int finalPosition { get; set; }
    public bool raceEnded { get; set; }
    public int scorePartial { get; set; }
    #endregion
    public Rigidbody rb { get; private set; }
    public Renderer renderCompo { get; private set; }
    private GameObject objInside;
    [SerializeField] DataManager dataAllMarbles;
    private float handicap;
    public float rightEnergy { get; private set; } = Constants.timeDriving;
    public float leftEnergy { get; private set; } = Constants.timeDriving;
    public float frontEnergy { get; private set; } = Constants.timeAceleration;
    public MarbleData bufferPlayer { get; set; }
    private bool outOfTrack;

    public System.Action<float> OnTrackSpeed;
    public System.Action<bool> OnTheTrack;
    public System.Action OnRespawn;
    //No Quite System

    [Header("~~~~~~~ Powers ~~~~~~~")]
    [SerializeField] private GameObject freezeModel;
    [SerializeField] private GameObject explotionParticles;
    public System.Action<PowerUpType> OnPowerUpObtained;
    public System.Action<PowerUpType> OnPowerUpDelivered;
    private CollisionDetector colliDetector;
    private PowerUpType powerObtained = PowerUpType.None;

    private void Awake()
    {
        renderCompo = GetComponent<Renderer>();
        colliDetector = GetComponent<CollisionDetector>();
        colliDetector.OnCollisionEntered += CollisionWithOtherCompetitor;
        SubscribeToTheMainMenu();
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
            leftEnergy = ChargeEnergyDriving(leftEnergy);
            rightEnergy = ChargeEnergyDriving(rightEnergy);
            frontEnergy = ChargeEnergyFront(frontEnergy);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isPlayer)
        {
            ApplyForce();
        }
    }

    public void SubscribeToTheMainMenu()
    {
        if (justVisualAward)
        {
            return;
        }
        MainMenuController.GetInstance().OnRaceReady += ReadyToPlay;
    }
    public void ReadyToPlay()
    {
        if (!isPlayer) renderCompo.enabled = true;
        if (objInside != null) objInside.SetActive(true);
    }
    public void FirstImpulse()
    {
        SetRigidbody();
        rb.isKinematic = false;
        StartCoroutine(ContinuesImpulse());
        float hancdicap = (isPlayer) ? 1 : 1 + +RaceController.Instance.GetHandicapByLeagueSaved(this);
        rb.AddForce(Vector3.forward * Random.Range(21f, 24f) * hancdicap, ForceMode.Impulse);
        if (!isPlayer)
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
            //Debug.LogError("MNueren");
        }
        else if (!ReferenceEquals(newSector, currentSector) && !ReferenceEquals(newSector, beforeSector))
        {
            print("Penalizacion por pasrce el sector" + currentSector.name + " -al- " + newSector.name + "@" + name);
            RespawnMarble();
        }
    }
    #region Force 
    private float ChargeEnergyFront(float energyDesire)
    {
        if (energyDesire < Constants.timeAceleration)
            energyDesire += Time.deltaTime;

        return energyDesire;
    }

    private float ChargeEnergyDriving(float energyDesire)
    {
        if (energyDesire < Constants.timeDriving)
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

    private void ApplyForce() => rb.AddForce(currentSector.transform.forward * GetMultiplicatorByPosition(), ForceMode.Impulse);

    private void ApplyForce(bool directionRight)
    {
        if (directionRight)
            rb.AddForce((currentSector.transform.forward / 4 + currentSector.transform.right) * GetMultiplicatorByPosition(), ForceMode.Impulse);
        else
            rb.AddForce((currentSector.transform.forward / 4 - currentSector.transform.right) * GetMultiplicatorByPosition(), ForceMode.Impulse);
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
            OnTrackSpeed?.Invoke(rb.velocity.magnitude);
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

    #region SoundsMarble
    private void SoundHitTrack(bool touching)
    {
        if (PoolAmbientSounds.GetInstance())
        {
            PoolAmbientSounds.GetInstance().PushShoot(SoundType.CollisionMarbleTrack, transform.position, renderCompo.isVisible);
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
                    if (PoolAmbientSounds.GetInstance())
                    {
                        canHitSound = false;
                        PoolAmbientSounds.GetInstance().PushShoot(SoundType.CollisionMarbleTrack, transform.position, renderCompo.isVisible);
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
        if (outOfTrack)
            RespawnMarble();
    }

    public void RespawnMarble()
    {
        fell = true;
        rb.velocity = Vector3.zero;
        //bug fixing: fall before pass through the goal
        if (beforeSector == null)
            beforeSector = RaceController.Instance.sectorInFront;
        transform.position = beforeSector.transform.position;
        Invoke("PushByFell", 1f);
        print(name + " fell");
        PoolAmbientSounds.GetInstance().PushShoot(SoundType.Respawn, (isPlayer) ? Vector3.zero : transform.position, renderCompo.isVisible);
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
        while (rb.velocity.magnitude < 10)
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
            bufferPlayer = dataAllMarbles.allMarbles.GetSpecificMarble(dataAllMarbles.GetCurrentMarble());
            marbleInfo = dataAllMarbles.allMarbles.GetSpecificMarble(0);
            if (renderCompo == null) { renderCompo = GetComponent<Renderer>(); }
            renderCompo.material = bufferPlayer.mat;

            if (bufferPlayer.objectInside != null)
            {
                if (objInside != null)
                    Destroy(objInside);

                objInside = Instantiate(bufferPlayer.objectInside, transform.position, Quaternion.identity, transform);
                objInside.SetActive(true);
            }
            marbleInfo.nameMarble = Constants.NORMI;
        }
        else
        {
            marbleInfo = mar;

            if (renderCompo == null) { renderCompo = GetComponent<Renderer>(); }
            if (renderCompo == null) print("commpo nullo");

            renderCompo.material = marbleInfo.mat;

            if (marbleInfo.objectInside != null)
            {
                if (objInside != null)
                {
                    Destroy(objInside);
                }
                objInside = Instantiate(marbleInfo.objectInside, transform.position, Quaternion.identity, transform);
                objInside.SetActive((justVisualAward == true) ? true : false);
            }
        }
        if (justVisualAward) return;
        CalculateHandicapLeague();
    }

    public void SetMarbleSettings(int indexInAllMarbles)
    {
        if (!isPlayer)
        {
            Debug.LogError("Tenemos Problemas La marble no es player");
        }

        bufferPlayer = dataAllMarbles.allMarbles.GetSpecificMarble(indexInAllMarbles);
        dataAllMarbles.SetCurrentMarble(indexInAllMarbles);
        if (renderCompo == null) { renderCompo = GetComponent<Renderer>(); }

        renderCompo.material = bufferPlayer.mat;

        if (objInside != null) Destroy(objInside);

        if (bufferPlayer.objectInside != null) objInside = Instantiate(bufferPlayer.objectInside, transform.position, Quaternion.identity, transform);
        marbleInfo = bufferPlayer;
    }

    #region Dificulty

    private float GetMultiplicatorByPosition()
    {
        if (isZombieQualy)
            return 5;
        float multiplicator = (boardController.transform.GetSiblingIndex() > 6) ? 4f : 3f;
        multiplicator += handicap;
        return multiplicator;
    }
    void CalculateHandicapLeague()
    {
        if (!isPlayer && RaceController.Instance != null)
        {
            handicap += RaceController.Instance.GetHandicapByLeagueSaved(this);
        }
    }
    #endregion


    #region PowerUpEnchants

    public void CollisionWithOtherCompetitor(Collision collis)
    {
        if (!collis.gameObject.GetComponent<Marble>()) return;
        Marble otherMarble = collis.gameObject.GetComponent<Marble>();

        BecameZombieQualifying(otherMarble);

        if (powerObtained != PowerUpType.None)
        {
            otherMarble.Enchant(powerObtained);
            powerObtained = PowerUpType.None;
            OnPowerUpDelivered?.Invoke(powerObtained);
        }

        PoolAmbientSounds.GetInstance().PushShoot(SoundType.CollisionMarbleMarble, transform.position, renderCompo.isVisible);
    }

    public void SetPowerUp(PowerUpType _typerPowe)
    {
        powerObtained = _typerPowe;
        OnPowerUpObtained?.Invoke(_typerPowe);
        if (_typerPowe == PowerUpType.Shrink
            || _typerPowe == PowerUpType.Enlarge
            || _typerPowe == PowerUpType.Wall)
        {
            Enchant(_typerPowe);
            powerObtained = PowerUpType.None;
        }

        PoolAmbientSounds.GetInstance().PushShoot(SoundType.BoxPowerUp, (isPlayer) ? Vector3.zero : transform.position, renderCompo.isVisible);
    }

    public void Enchant(PowerUpType _typerPower)
    {
        switch (_typerPower)
        {
            case PowerUpType.Freeze:
                freezeModel.SetActive(true);
                rb.velocity = Vector3.zero;
                Invoke("RestoreMarble", 3);
                PoolAmbientSounds.GetInstance().PushShoot(SoundType.FreezePow, (isPlayer) ? Vector3.zero : transform.position, renderCompo.isVisible);
                break;

            case PowerUpType.Shrink:
                transform.DOScale(Vector3.one * 0.4f, 0.8f).SetEase(Ease.InElastic);
                rb.mass = 0.4f;
                Invoke("RestoreMarbleScale", Constants.timeBigSize);
                break;

            case PowerUpType.Enlarge:
                transform.DOScale(Vector3.one * 2f, 0.8f).SetEase(Ease.InElastic);
                ApplyForce();
                ApplyForce();
                ApplyForce();
                rb.mass = 2f;
                Invoke("RestoreMarbleScale", Constants.timeBigSize);
                PoolAmbientSounds.GetInstance().PushShoot(SoundType.EnlargePow, (isPlayer) ? Vector3.zero : transform.position, renderCompo.isVisible);
                break;

            case PowerUpType.Explo:
                explotionParticles.SetActive(false);
                explotionParticles.SetActive(true);
                rb.AddForce(currentSector.transform.up * 9, ForceMode.Impulse);
                rb.velocity = rb.velocity * 0.8f;
                PoolAmbientSounds.GetInstance().PushShoot(SoundType.ExploPow, (isPlayer) ? Vector3.zero : transform.position, renderCompo.isVisible);
                break;

            case PowerUpType.Wall:
                Marble marbleFirst = RaceController.Instance.GetPositionMarble(0);
                if (marbleFirst == null)
                { //en caso que ninguna marbles haya pasado la linea de meta
                    Vector3 posFirstSector = currentSector.nextSector.nextSector.nextSector.transform.position;
                    Vector3 posFrontSector = posFirstSector + currentSector.nextSector.nextSector.nextSector.transform.forward * 6;
                    PoolPowerUps.GetInstance().CreatePow(posFrontSector, currentSector.nextSector.nextSector.nextSector.transform.rotation, _typerPower);
                }
                else
                {
                    Vector3 posFirstsector = marbleFirst.currentSector.nextSector.transform.position;
                    Vector3 posFrontSector = posFirstsector + marbleFirst.currentSector.nextSector.transform.forward * 6;
                    PoolPowerUps.GetInstance().CreatePow(posFrontSector, marbleFirst.currentSector.nextSector.transform.rotation, _typerPower);
                }
                break;

            case PowerUpType.Bump:
                Vector3 dirVelo = transform.position - rb.velocity.normalized * 3;
                PoolPowerUps.GetInstance().CreatePow(dirVelo, transform.rotation, _typerPower);
                break;
        }
    }
    public bool CheckHasPower() => powerObtained != PowerUpType.None ? true : false;
    public void RestoreMarble()
    {
        if (freezeModel.activeInHierarchy)
        {
            freezeModel.SetActive(false);
            ApplyForce();
        }
    }

    public void RestoreMarbleScale()
    {
        if (transform.localScale.z < 0.5f)
        {
            transform.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutElastic);
            rb.mass = 1f;
        }

        if (transform.localScale.z > 1)
        {
            transform.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutElastic);
            rb.mass = 1f;
        }
        PoolAmbientSounds.GetInstance().PushShoot(SoundType.RestoreSize, (isPlayer) ? Vector3.zero : transform.position, renderCompo.isVisible);
    }
    #endregion

#region Qualifying 
    private void BecameZombieQualifying(Marble marble) 
    {
        if(!RacersSettings.GetInstance().legaueManager.Liga.GetIsQualifying()) return;
        if (isZombieQualy && !marble.isZombieQualy)
        {
            marble.isZombieQualy = RaceController.Instance.AddMarbleZombie(marble.gameObject);
            if (marble.isZombieQualy)
            { 
                marble.renderCompo.material = PoolPowerUps.GetInstance().materialZombie;
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
        //do
        //{
        //    rb.velocity = Vector3.zero;
        //    yield return new WaitForSeconds(0.1f);
        //}
        //while (gameObject.activeInHierarchy); 
    }

    private IEnumerator ContinuesImpulse()
    {
        while (isZombieQualy)
        {
            if (rb.velocity.magnitude < 10)
                rb.AddForce(currentSector.transform.forward, ForceMode.Impulse);
            yield return new WaitForEndOfFrame();
            rb.isKinematic = false;
        }
    }
#endregion
}
