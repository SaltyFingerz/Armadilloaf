using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolMovement : MonoBehaviour
{
    public Transform M_goal;
    // Start is called before the first frame update
    void Start()
    {
        NavMeshAgent l_agent = GetComponent<NavMeshAgent>();
        l_agent.destination = M_goal.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
