using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour {


    NavMeshAgent m_NavMeshAgent;
    int m_CurrentPatrolPositionId = -1;
    public List<Transform> m_PatrolPositions;
    public Transform quad;

    public GameObject patrolPositionsGameObject;
    public List<Sprite> Anim;
    int numSpriteAnim;
    float countAnim;
    float timerAnim = 0.3f;
    Material m_Material;
    int children;

    private void Awake()
    {

    }
    void Start()
    {
        m_Material = quad.GetComponent<Renderer>().material;

        children = patrolPositionsGameObject.transform.childCount;
        for (int i = 0; i < children; i++)
            m_PatrolPositions.Add(patrolPositionsGameObject.transform.GetChild(i));
        m_NavMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();

        m_CurrentPatrolPositionId = Random.Range(0, m_PatrolPositions.Count - 1);
        m_NavMeshAgent.isStopped = false;
        m_NavMeshAgent.SetDestination(m_PatrolPositions[m_CurrentPatrolPositionId].position);
    }


    void Update()
    {
        quad.LookAt(PlayerManager._Instance.transform.position);
        quad.eulerAngles = new Vector3(0, quad.transform.eulerAngles.y + 180, 0);

        if (!m_NavMeshAgent.hasPath && m_NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
            MoveToNextPatrolPosition();

        if (Anim.Count != 0)
        {
            countAnim += Time.deltaTime;
            if (countAnim >= timerAnim)
            {
                numSpriteAnim++;
                if (numSpriteAnim >= Anim.Count)
                {
                    numSpriteAnim = 0;
                }
                m_Material.mainTexture = Anim[numSpriteAnim].texture;
                countAnim = 0;
            }
        }

    }

    void MoveToNextPatrolPosition()
    {
        m_CurrentPatrolPositionId = Random.Range(1, m_PatrolPositions.Count - 1);
        if (m_CurrentPatrolPositionId >= m_PatrolPositions.Count)
            m_CurrentPatrolPositionId = 0;
        m_NavMeshAgent.SetDestination(m_PatrolPositions[m_CurrentPatrolPositionId].position);
    }
}
