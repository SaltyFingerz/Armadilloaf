using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolMovement : MonoBehaviour
{
    public Transform M_goal;
    NavMeshAgent m_agent;
    // Start is called before the first frame update
    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

        m_agent.destination = M_goal.position;
    }
}
