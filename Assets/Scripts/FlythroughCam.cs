using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlythroughCam : MonoBehaviour
{
    public GameObject M_WalkCam;
    public GameObject M_HUD;
    public GameObject M_cutsceneSkip;
    public GameObject M_text1;
    public GameObject M_text2;
    bool[] flyOut = new bool[2];
    bool[] hasFlown = new bool[2];
    Vector3 M_text1start;
    Vector3 M_text2start;

    // Start is called before the first frame update
    void Start()
    {
        M_text1start = M_text1.transform.localPosition;
        M_text2start = M_text2.transform.localPosition;
        M_cutsceneSkip.SetActive(true);
        M_WalkCam.SetActive(false);
        M_HUD.SetActive(false);
        StartCoroutine(TextFlyIn(M_text1));
    }

    IEnumerator TextFlyIn(GameObject a_flyObject)
    {
        Vector3 l_currentVector = a_flyObject.transform.localPosition;
        l_currentVector = new Vector3(l_currentVector.x + 750.0f * Time.deltaTime, l_currentVector.y, l_currentVector.z);
        a_flyObject.transform.localPosition = l_currentVector;
        yield return null;
    }

    IEnumerator WaitACoupleSecs(GameObject a_flyObject)
    {
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(TextFlyOut(a_flyObject));
    }

    IEnumerator TextFlyOut(GameObject a_flyObject)
    {
        Vector3 l_currentVector = a_flyObject.transform.localPosition;
        l_currentVector = new Vector3(l_currentVector.x - 750.0f * Time.deltaTime, l_currentVector.y, l_currentVector.z);
        a_flyObject.transform.localPosition = l_currentVector;
        yield return null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SwitchCams();
        }

        if (M_text1.transform.localPosition.x < -176 && !flyOut[0])
        {
            StartCoroutine(TextFlyIn(M_text1));
        }
        else if (!hasFlown[0] && M_text1.transform.localPosition.x > M_text1start.x)
        {
            flyOut[0] = true;
            StartCoroutine(WaitACoupleSecs(M_text1));
        }
        else
        {
            hasFlown[0] = true;
        }

        if (hasFlown[0])
        { 
        if (M_text2.transform.localPosition.x < -176 && !flyOut[1])
        {
            StartCoroutine(TextFlyIn(M_text2));
        }

        else if (!hasFlown[1] == true && M_text2.transform.localPosition.x > M_text2start.x)
        {
            flyOut[1] = true;
            StartCoroutine(WaitACoupleSecs(M_text2));
        }
        else
        {
            hasFlown[1] = true;
        }
        }
    }
    public void SwitchCams()
    {
        M_WalkCam.SetActive(true);
        M_cutsceneSkip.SetActive(false);
        gameObject.SetActive(false);
        M_HUD.SetActive(true);
    }

}
