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
       

        //transform.position.y rather than target.transform.position.y is used to prevent the enemy from tilting off the vertical y axis. 

        {
            if (M_Walker.activeSelf)
            {
                Vector3 targetPosition = new Vector3(M_targetWalk.transform.position.x, transform.position.y, M_targetWalk.transform.position.z);
                transform.LookAt(targetPosition); }
            else if (M_Ball.activeSelf)
            {
                Vector3 targetPosition = new Vector3(M_targetBall.transform.position.x, transform.position.y, M_targetBall.transform.position.z);
                transform.LookAt(M_targetBall.transform); }
           //makes the enemy look at the player. This line of code was learnt from the tutorial available at: www.youtube.com/watch?v=rP_bEq248e4

        }
    }
}
