using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuEnterToPlayScript : MonoBehaviour
{
    private UIManagerScript M_UIMan;
    // Start is called before the first frame update
    void Start()
    {
        M_UIMan = GetComponent<UIManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            M_UIMan.Cutscene();
        }
        
    }
}
