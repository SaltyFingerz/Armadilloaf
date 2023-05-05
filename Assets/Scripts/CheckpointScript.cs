using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    public PlayerManagerScript m_player;
    public MessageScreenScript m_messageScreen;
    public bool m_hasSetCheckpoint;

    // Start is called before the first frame update
    void Start()
    {
        m_player = FindObjectOfType<PlayerManagerScript>();
        m_messageScreen = FindObjectOfType<MessageScreenScript>();
        m_hasSetCheckpoint = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!m_hasSetCheckpoint)
        { 
        SetCheckpoint();
        }
        m_player.Regenerate();
    }

    public void SetCheckpoint()
    {
        if (m_player.currentCheckpoint != transform.position)
        { 
        m_player.currentCheckpoint = new Vector3 (transform.position.x, transform.position.y + 2.0f, transform.position.z);   
        StartCoroutine(m_messageScreen.CheckpointTextOnScreen(this));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
