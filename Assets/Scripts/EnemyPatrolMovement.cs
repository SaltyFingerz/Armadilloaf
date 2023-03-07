using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolMovement : MonoBehaviour
{
    public List<GameObject> M_goals;
    NavMeshAgent m_agent;
    int m_goalIndex;
    // Start is called before the first frame update
    void Start()
    {
        m_goalIndex = 0;
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.destination = M_goals[0].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_agent.remainingDistance < 0.2f)
        {
            m_goalIndex++;
            if(m_goalIndex >= M_goals.Count)
            {
                m_goalIndex = 0;
            }
            m_agent.destination = M_goals[m_goalIndex].transform.position;
        }
    }
}
