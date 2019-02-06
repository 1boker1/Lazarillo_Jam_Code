using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager _Instance;
    public static PlayerMovement _Movement { get; set; }
    public static PlayerUI _UI { get; set; }
    public static PlayerInteract _Interact { get; set; }

    public List<Item> PlayerItems = new List<Item>();
    public List<PickUpObject> DropItems = new List<PickUpObject>();

    public bool destroyOnLoad = false;

    public Animation dayAndNightCycle;
    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
            SetUp();
            if (!destroyOnLoad) DontDestroyOnLoad(this.gameObject);
        }
    }

    void SetUp()
    {
        _Movement = FindObjectOfType<PlayerMovement>();
        _UI = FindObjectOfType<PlayerUI>();
        _Interact = FindObjectOfType<PlayerInteract>();

        HP = MaxHP;
        Energy = MaxEnergy;
        MaxStamina = Energy;
        Stamina = MaxStamina;
        RunHunger = RunHungerCount;
        sleep = TimeToSleep;

        WakeUp();
    }

    [HideInInspector] public bool RunAway;
    [HideInInspector] public bool Stealing;
    public void OnStealing(NPC newNPC)
    {
        CurrentNPC = newNPC;

        Stealing = true;
        _Movement.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        _UI.DesableWalkingUI();
    }

    public void OnWalking(int score)
    {
        Stealing = false;
        _Movement.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _UI.EnableWalkingUI();


        // CHANGE
        if (CurrentNPC)
            CurrentNPC.EndMinigame(score);

        CurrentNPC = null;
    }

    public void OnSleeping(float food, float energy, string message)
    {
        Stamina = MaxStamina;

        Stealing = true;
        _Movement.enabled = false;

        Energy += energy;
        Food -= food;

        if (food == Mathf.Infinity)
        {
            Money = 0;
        }

        if (Food < 0)
            Food = 0;

        if (Energy > MaxEnergy)
            Energy = MaxEnergy;

        _UI.MakeFadeIn(message);
        sleep = TimeToSleep;
        if (dayAndNightCycle != null) dayAndNightCycle.Rewind();
    }

    public void WakeUp()
    {
        Stealing = false;
        _Movement.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _UI.EnableWalkingUI();


        CurrentNPC = null;
    }


    public float Food;
    public float Money;

    [Space(10)]
    public float MaxHP;
    [HideInInspector] public float HP;

    public float MaxEnergy;
    [HideInInspector] public float Energy;
    public float EnergyOnRunning;

    [Space(10)]
    [HideInInspector] public float MaxStamina;
    [HideInInspector] public float Stamina;
    public float StaminaMod;

    [HideInInspector] public bool Running;
    [HideInInspector] public bool canRun = true;
    public float RunHungerCount;
    public float EatingTime;
    float eatTime;
    float RunHunger;

    [Space(10)]
    public float TimeToSleep;
    [HideInInspector] public float sleep;

    [Space(10)]
    public float PigEnergy;
    public float FaintEnergy;
    public float SquireEnergy;

    public NPC CurrentNPC;

    public int day;

    public ParticleSystem EatPaticles;
    public ParticleSystem HitParticles;

    public AudioSource audioSource;

    public AudioClip eat;
    public AudioClip damage;
    public AudioClip exhaust;

    bool onEating;
    public void StartEating()
    {
        if (Food > 0 && MaxEnergy > Energy)
        {

            onEating = true;
            eatTime = EatingTime;
            EatPaticles.Play();
            audioSource.clip = eat;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void Eating()
    {
        if (onEating)
        {
            eatTime -= Time.deltaTime;
            if (eatTime < 0)
            {
                StopEating();
                Food -= 1;
                Energy += 10;
                if (Energy > MaxEnergy)
                    Energy = MaxEnergy;
                _UI.RefreshUI();
            }
            _UI.RefreshEatImage(eatTime / RunHungerCount);
        }

    }

    public void StopEating()
    {
        onEating = false;
        _UI.HideEatImage();
        EatPaticles.Stop();
        audioSource.Stop();

    }

    #region FOOD/MONEY
    public void AddFood(float num)
    {
        Food += num;
        _UI.RefreshUI();
    }

    public void AddMoney(float num)
    {
        Money += num;
        _UI.RefreshUI();
    }
    #endregion

    #region Energy / Damage / Stamina
    public void Damage(float Dmg)
    {
        HP -= Dmg;
        _UI.RefreshUI();
        HitParticles.gameObject.SetActive(true);
        HitParticles.Play();
        audioSource.clip = damage;
        audioSource.loop = false;
        audioSource.Play();
    }
    public void RefreshUI()
    {
        _UI.RefreshUI();
    }

    public void ConsumeHunger(float num)
    {
        Energy -= num;
        MaxStamina = Energy;

        _UI.RefreshUI();
    }

    public void UseStamina()
    {
        Stamina -= Time.deltaTime * StaminaMod;
        RunHunger -= Time.deltaTime;

        _UI.RefreshStamina();

        if (RunHunger < 0)
        {
            RunHunger = RunHungerCount;
            ConsumeHunger(EnergyOnRunning);
        }

        if (Stamina < 0)
        {
            audioSource.clip = exhaust;
            audioSource.loop = false;
            audioSource.Play();
            canRun = false;
            _Movement.Walk();
        }

    }

    public void RecoverStamina()
    {
        if (!Running)
        {
            RunHunger = RunHungerCount;
            if (Stamina > MaxStamina)
            {
                _UI.HideStamina();
            }
            else
            {
                Stamina += Time.deltaTime * StaminaMod;
                _UI.RefreshStamina();
            }

            if (!canRun && Stamina > MaxStamina / 4)
            {
                canRun = true;
            }
        }

    }
    #endregion

    void Sleep()
    {
        if (sleep < 0)
        {
            OnSleeping(Mathf.Infinity, FaintEnergy, "Las calles no son un muy buen sitio para dormir, perdere todas mis pertenecias \n pero ahora mismo no puedo levantarme");
            Damage(10);
        }
        else
        {
            sleep -= Time.deltaTime;
        }
        _UI.RefreshSleep();
    }

    void RecoverHP()
    {
        if (HP != MaxHP)
        {
            HP += Time.deltaTime;
            Energy -= Time.deltaTime;

            _UI.RefreshUI();
        }

    }

    private void Update()
    {
        if (!Stealing)
        {
            RecoverStamina();
            Eating();
            Sleep();
        }
    }

}
