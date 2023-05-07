using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageScreenScript : MonoBehaviour
{
    Canvas m_canvas;
    public TextMeshProUGUI m_checkpointText;

    // Start is called before the first frame update
    void Start()
    {
        m_canvas = GetComponent<Canvas>();
        m_canvas.enabled = false;
        m_checkpointText.enabled = false;
    }

    public IEnumerator CheckpointTextOnScreen(CheckpointScript A_checkPoint)
    {
        m_canvas.enabled = true;
        m_checkpointText.enabled = true;

        yield return new WaitForSeconds(2);

        m_canvas.enabled = false;
        m_checkpointText.enabled = false;

        yield return new WaitForSeconds(1);

        Destroy(A_checkPoint);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
