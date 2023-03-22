using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayerScript : MonoBehaviour
{

    public Transform M_targetWalk;
    public Transform M_targetBall;
    public GameObject M_Walker;
    public GameObject M_Ball;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (M_Walker.activeSelf)
        { transform.LookAt(M_targetWalk.transform); }
        else if (M_Ball.activeSelf)
        { transform.LookAt(M_targetBall.transform); }
    }
}
