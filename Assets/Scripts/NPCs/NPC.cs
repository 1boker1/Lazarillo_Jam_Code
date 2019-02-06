using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public static PlayerUI _UI { get; set; }
    NavMeshAgent m_NavMeshAgent;
    public enum TState
    {
        IDLE,
        PATROL,
        RECOLLECT,
        ATTACK,
        CHASE,
        STEAL
    }
    public List<Transform> m_PatrolPositions;
    public List<Sprite> FrontAnim;
    public List<Sprite> BackAnim;
    int numSpriteAnim;
    float countAnim;
    float timerAnim = 0.3f;

    public Transform npcQuad;
    public TState State;
    Vector3 lookPos;
    public Sprite front;
    public Sprite back;
    Material m_Material;
    float m_CurrentTime;
    [Space(10)]

    //[Tooltip ()]
    //Timers
    public float timeIdle = 2;
    public float timeChase;
    public float timeAttack;
    public float timeEnterChase;

    [Space(10)]
    //Bools
    public bool canSteal;
    public bool easySteal;
    public bool stealing;
    [Space(10)]

    int Alert;

    public Vector3 playerNPCVector;
    public float m_MinDistanceToAttack;
    int m_CurrentPatrolPositionId = -1;
    public GameObject bag;

    int itemsAttack;
    int numberItems;
    public float AlertperSecond;
    public float speedPickUp;

    public PickUpObject food5;
    public PickUpObject food1;
    public PickUpObject money5;
    public PickUpObject money1;

    PickUpObject recollectingObject;

    public float NPC_DMG;
    public int RandomBag;

    [Space(10)]
    public float pushingForce;
    public ParticleSystem particles;

    public AudioSource NPCAudioSource;
    public AudioClip angryClip;
    public AudioClip hitClip;
    public AudioClip talkClip;

    public Transform positionToDeath;


    void Start()
    {
        m_PatrolPositions = PatrolPositions._Instance.m_PatrolPositions;
        m_Material = npcQuad.GetComponent<Renderer>().material;
        m_NavMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
        canSteal = true;
        easySteal = false;
        State = TState.PATROL;
        numSpriteAnim = 0;
        recollectingObject = null;
        RandomBag = Random.Range(0, AllItems.instance.allBags.Count - 1);
        bag = Instantiate(AllItems.instance.allBags[RandomBag], bag.transform.position, bag.transform.rotation, bag.transform.parent);
        NPCAudioSource.loop = false;
        particles.Stop();

        NPCAudioSource.loop = true;
        NPCAudioSource.clip = talkClip;
        NPCAudioSource.Play();


    }

    void Update()
    {
        m_CurrentTime += Time.deltaTime;
        LookPlayer();

        StateMachine();
    }

    void LookPlayer()
    {

        npcQuad.transform.position = transform.position;
        npcQuad.LookAt(PlayerManager._Instance.transform.position);
        npcQuad.eulerAngles = new Vector3(0, npcQuad.eulerAngles.y + 180, 0);


        playerNPCVector = Camera.main.transform.position - transform.position;

        countAnim += Time.deltaTime;
        if (m_NavMeshAgent.isOnNavMesh && m_NavMeshAgent.isStopped == false)
        {
            if (Vector3.Angle(playerNPCVector, transform.forward) <= 90)
            {
                if (countAnim >= timerAnim)
                {
                    numSpriteAnim++;
                    if (numSpriteAnim >= FrontAnim.Count)
                    {
                        numSpriteAnim = 0;
                    }
                    m_Material.mainTexture = FrontAnim[numSpriteAnim].texture;
                    countAnim = 0;
                }
                easySteal = false;
            }
            else
            {
                if (countAnim >= timerAnim)
                {
                    numSpriteAnim++;
                    if (numSpriteAnim >= BackAnim.Count)
                    {
                        numSpriteAnim = 0;
                    }
                    m_Material.mainTexture = BackAnim[numSpriteAnim].texture;
                    countAnim = 0;
                }
                easySteal = true;
            }
        }
        else
        {
            if (Vector3.Angle(playerNPCVector, transform.forward) <= 90)
            {

                m_Material.mainTexture = front.texture;
                countAnim = 0;

                easySteal = false;
            }
            else
            {
                m_Material.mainTexture = back.texture;
                countAnim = 0;
                easySteal = false;
            }
        }
    }

    void StateMachine()
    {
        switch (State)
        {
            case TState.IDLE:
                UpdateIdle();
                break;

            case TState.PATROL:
                UpdatePatrol();
                break;

            case TState.STEAL:
                UpdateSteal();
                break;

            case TState.RECOLLECT:
                UpdateRecollect();
                break;

            case TState.ATTACK:
                UpdateAttack();
                break;

            case TState.CHASE:
                UpdateChase();
                break;
        }
    }


    //SETS

    void SetIdle()
    {
        State = TState.IDLE;
        m_CurrentTime = 0;
        m_NavMeshAgent.isStopped = true;
        easySteal = true;
    }

    void SetPatrol()
    {
        State = TState.PATROL;
        easySteal = false;

        m_NavMeshAgent.isStopped = false;

        NPCAudioSource.loop = true;
        NPCAudioSource.clip = talkClip;
        NPCAudioSource.Play();


        if (canSteal)
        {
            m_CurrentPatrolPositionId = Random.Range(0, m_PatrolPositions.Count - 1);
            m_NavMeshAgent.SetDestination(m_PatrolPositions[m_CurrentPatrolPositionId].position);
        }
        else if (!canSteal)
        {
            m_CurrentPatrolPositionId = Random.Range(0, SpawnerPositions._Instance.m_SpawnerPositions.Count - 1);

            m_NavMeshAgent.SetDestination(SpawnerPositions._Instance.m_SpawnerPositions[m_CurrentPatrolPositionId].position);

            positionToDeath = SpawnerPositions._Instance.m_SpawnerPositions[m_CurrentPatrolPositionId];

        }

    }

    void SetSteal()
    {
        State = TState.STEAL;
        m_NavMeshAgent.isStopped = true;
        canSteal = false;
    }

    void SetRecollect()
    {
        State = TState.RECOLLECT;
        m_NavMeshAgent.isStopped = true;

        NPCAudioSource.loop = true;
        NPCAudioSource.clip = angryClip;
        NPCAudioSource.Play();
    }

    void SetAttack()
    {
        State = TState.ATTACK;
        m_CurrentTime = 0;
        m_NavMeshAgent.isStopped = true;

        PlayerManager._Instance.Damage(NPC_DMG);

        NPCAudioSource.loop = false;
        NPCAudioSource.clip = hitClip;
        NPCAudioSource.Play();


        Vector3 jumpBack = (PlayerManager._Instance.transform.position - transform.position).normalized * pushingForce;
        jumpBack.y = 3;
        PlayerManager._Movement._Velocity = jumpBack;

        if (PlayerManager._Instance.Food >= 20)
        {
            PlayerManager._Instance.Food -= 20;
            PlayerManager._Instance.RefreshUI();
            for (int i = 0; i < 3; i++)
            {
                PickUpObject newObject = Instantiate(food5);
                PlayerManager._Instance.DropItems.Add(newObject);
                newObject.transform.position = new Vector3(PlayerManager._Instance.transform.position.x, PlayerManager._Instance.transform.position.y + 2, PlayerManager._Instance.transform.position.z);
                newObject.transform.position += Random.insideUnitSphere;
            }
            for (int i = 0; i < 5; i++)
            {
                PickUpObject newObject = Instantiate(food1);
                PlayerManager._Instance.DropItems.Add(newObject);
                newObject.transform.position = new Vector3(PlayerManager._Instance.transform.position.x, PlayerManager._Instance.transform.position.y + 2, PlayerManager._Instance.transform.position.z);
                newObject.transform.position += Random.insideUnitSphere;
            }
        }
        else
        {
           
            if (PlayerManager._Instance.Food > 5)
            {
                for (int i = 0; PlayerManager._Instance.Food > 5; i++)
                {
                    PickUpObject newObject = Instantiate(food5);
                    PlayerManager._Instance.DropItems.Add(newObject);
                    newObject.transform.position = new Vector3(PlayerManager._Instance.transform.position.x, PlayerManager._Instance.transform.position.y + 2, PlayerManager._Instance.transform.position.z);
                    newObject.transform.position += Random.insideUnitSphere;
                    PlayerManager._Instance.Food -= 5;
                }
            }
            for (int i = 0; PlayerManager._Instance.Food > 0; i++)
            {
                PickUpObject newObject = Instantiate(food1);
                PlayerManager._Instance.DropItems.Add(newObject);
                newObject.transform.position = new Vector3(PlayerManager._Instance.transform.position.x, PlayerManager._Instance.transform.position.y + 2, PlayerManager._Instance.transform.position.z);
                newObject.transform.position += Random.insideUnitSphere;
                PlayerManager._Instance.Food -= 1;
            }
            PlayerManager._Instance.RefreshUI();

        }

        if (PlayerManager._Instance.Money >= 95)
        {
            PlayerManager._Instance.Money -= 95;
            Debug.Log(PlayerManager._Instance.Money);
            PlayerManager._Instance.RefreshUI();


            for (int i = 0; i < 2; i++)
            {
                PickUpObject newObject = Instantiate(money5);
                PlayerManager._Instance.DropItems.Add(newObject);
                newObject.transform.position = new Vector3(PlayerManager._Instance.transform.position.x, PlayerManager._Instance.transform.position.y + 2, PlayerManager._Instance.transform.position.z);
                newObject.transform.position += Random.insideUnitSphere;
            }
            for (int i = 0; i < 3; i++)
            {
                PickUpObject newObject = Instantiate(money1);
                PlayerManager._Instance.DropItems.Add(newObject);
                newObject.transform.position = new Vector3(PlayerManager._Instance.transform.position.x, PlayerManager._Instance.transform.position.y + 2, PlayerManager._Instance.transform.position.z);
                newObject.transform.position += Random.insideUnitSphere;
            }
        }
        else
        {
            if(PlayerManager._Instance.Money > 50)
            {
                for (int i = 0; PlayerManager._Instance.Money > 50; i++)
                {
                    PickUpObject newObject = Instantiate(money5);
                    PlayerManager._Instance.DropItems.Add(newObject);
                    newObject.transform.position = new Vector3(PlayerManager._Instance.transform.position.x, PlayerManager._Instance.transform.position.y + 2, PlayerManager._Instance.transform.position.z);
                    newObject.transform.position += Random.insideUnitSphere;
                    PlayerManager._Instance.Money -= 25;
                }
            }
            
            for (int i = 0; PlayerManager._Instance.Money >= 15; i++)
            {
                PickUpObject newObject = Instantiate(money1);
                PlayerManager._Instance.DropItems.Add(newObject);
                newObject.transform.position = new Vector3(PlayerManager._Instance.transform.position.x, PlayerManager._Instance.transform.position.y + 2, PlayerManager._Instance.transform.position.z);
                newObject.transform.position += Random.insideUnitSphere;
                PlayerManager._Instance.Money -= 15;
            }
            PlayerManager._Instance.RefreshUI();

        }
    }

    void SetChase()
    {
        State = TState.CHASE;
        m_CurrentTime = 0;
        m_NavMeshAgent.isStopped = false;
        particles.Play();

        NPCAudioSource.loop = true;
        NPCAudioSource.clip = angryClip;
        NPCAudioSource.Play();

        Vector3 jumpBack = (PlayerManager._Instance.transform.position - transform.position).normalized * pushingForce;
        jumpBack.y = 3;
        PlayerManager._Movement._Velocity = jumpBack;

    }

    //UPDATES

    void UpdateIdle()
    {

        if (m_CurrentTime >= timeIdle)
        {
            SetPatrol();
        }

    }
    void UpdatePatrol()
    {
        if (!m_NavMeshAgent.hasPath && m_NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            if (canSteal)
                MoveToNextPatrolPosition();
            else if (!canSteal)
            {
                for (int i = 0; i < SpawnerPositions._Instance.m_SpawnerPositions.Count - 1; i++)
                {
                    if ((SpawnerPositions._Instance.m_SpawnerPositions[i].position - m_NavMeshAgent.transform.position).magnitude < 5)
                    {
                        AllItems.instance.allActiveNPC.Remove(this.transform.parent.gameObject);
                        Destroy(this.transform.parent.gameObject);
                    }
                    else
                    {
                        m_CurrentPatrolPositionId = Random.Range(0, SpawnerPositions._Instance.m_SpawnerPositions.Count - 1);

                        m_NavMeshAgent.SetDestination(SpawnerPositions._Instance.m_SpawnerPositions[m_CurrentPatrolPositionId].position);
                    }
                }
            }
        }





        if (stealing)
        {
            SetSteal();
        }

    }

    void UpdateSteal()
    {
        if (!stealing)
        {
            if (Alert >= 90)
            {
                SetAttack();
            }
            else if (Alert < 90 && Alert >= 50)
            {
                SetChase();
            }
            else if (Alert < 50)
            {
                SetPatrol();
            }
        }
    }

    void UpdateRecollect()
    {
        //Absorber cosas
        if (PlayerManager._Instance.DropItems.Count > 0)
        {
            if (recollectingObject == null)
            {
                int RandomItem = Random.Range(0, PlayerManager._Instance.DropItems.Count - 1);
                recollectingObject = PlayerManager._Instance.DropItems[RandomItem];
                recollectingObject.GetComponent<Rigidbody>().isKinematic = true;

            }
            else
            {
                float step = speedPickUp * Time.deltaTime;
                recollectingObject.transform.position = Vector3.MoveTowards(recollectingObject.transform.position, transform.position, step);
                if (recollectingObject.transform.position == transform.position)
                {
                    PlayerManager._Instance.DropItems.Remove(recollectingObject);
                    recollectingObject.gameObject.SetActive(false);
                    recollectingObject = null;
                }
            }
        }
        else
        {
            SetPatrol();
        }
    }

    void UpdateAttack()
    {
        if (m_CurrentTime >= timeAttack)
        {
            SetRecollect();
        }
    }

    void UpdateChase()
    {
        if (m_CurrentTime >= timeEnterChase)
        {
            if (m_CurrentTime >= timeChase)
            {
                SetPatrol();
            }

            else if (GetSqrDistanceXZToPosition(PlayerManager._Instance.transform.position) <= m_MinDistanceToAttack)
            {

                SetAttack();
            }
            else
            {
                SetNextChasePosition();

            }
        }

    }



    //FUNCIONES NAVMESH

    float GetSqrDistanceXZToPosition(Vector3 PlayerPosition)
    {
        Vector2 PlayerVector2 = new Vector2(PlayerPosition.x, PlayerPosition.z);
        Vector2 EnemyVector2 = new Vector2(m_NavMeshAgent.transform.position.x, m_NavMeshAgent.transform.position.z);
        return Vector2.Distance(PlayerVector2, EnemyVector2);
    }

    int GetClosestPatrolPositionId()
    {
        float minDist = Mathf.Infinity;
        Vector3 currentPos = m_NavMeshAgent.transform.position;
        int currentId = -1;
        for (int i = 0; i < m_PatrolPositions.Count; i++)
        {
            float dist = Vector3.Distance(m_PatrolPositions[i].position, currentPos);
            if (dist < minDist)
            {
                minDist = dist;
                currentId = i;

            }
        }
        return currentId;
    }


    void SetNextChasePosition()
    {
        m_NavMeshAgent.isStopped = false;
        Vector3 l_Destination = PlayerManager._Instance.transform.position - transform.position;
        float l_Distance = l_Destination.magnitude;
        l_Destination /= l_Distance;
        l_Destination = transform.position + l_Destination * (l_Distance - m_MinDistanceToAttack);
        m_NavMeshAgent.SetDestination(l_Destination);
    }
    void MoveToNextPatrolPosition()
    {

        m_CurrentPatrolPositionId = Random.Range(1, m_PatrolPositions.Count - 1);
        if (m_CurrentPatrolPositionId >= m_PatrolPositions.Count)
            m_CurrentPatrolPositionId = 0;
        m_NavMeshAgent.SetDestination(m_PatrolPositions[m_CurrentPatrolPositionId].position);


    }

    //OTHER

    #region other
    public void EndMinigame(int danger)
    {
        Alert = danger;
        stealing = false;
    }
    void OnTriggerEnter(Collider _collider)
    {
        if (_collider.tag == "Player")
        {
            Vector3 jumpBack = (PlayerManager._Instance.transform.position - transform.position).normalized * pushingForce;
            jumpBack.y = 3;
            PlayerManager._Movement._Velocity = jumpBack;
        }
    }

    #endregion
}
