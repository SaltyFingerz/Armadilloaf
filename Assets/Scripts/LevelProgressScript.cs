using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressScript : MonoBehaviour
{
    public GameObject M_FinishUI;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Finish"))
        {
            M_FinishUI.SetActive(true);
            M_FinishUI.GetComponent<LevelFinishScript>().UpdateScores();
            M_FinishUI.GetComponent<LevelFinishScript>().UpdateStars();
        }
    }
}
