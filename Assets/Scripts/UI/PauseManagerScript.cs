using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class PauseManagerScript : UIManagerScript
{
    public Canvas M_canvas;
    public Image M_pausePanel;
    public GameObject M_playerManager;
    public GameObject M_buttonsAndText;
    public TextMeshProUGUI fruitText;
    public TextMeshProUGUI respawnText;
    public TextMeshProUGUI shotsText;
    public TextMeshProUGUI timeText;
    public GameObject M_healthBiscuit;
    public GameObject M_collectedFruits;
    public AudioMixer M_audioMixer;
    public RenderingScript M_Rendering;

    const string MIXER_MASTER = "M_musicMuffle";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Check if the game is paused
        if (Time.timeScale > 0.1)
        {
            return;
        }

        M_audioMixer.SetFloat(MIXER_MASTER, 200.0f);

        if (Input.GetButtonUp("Cancel") || Input.GetKeyDown(KeyCode.P))
        {
            Resume();
        }
    }

    public void Resume()
    {
        M_Rendering.UnBlurBackground();
        M_buttonsAndText.SetActive(false);
        M_healthBiscuit.SetActive(true);
        M_collectedFruits.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        M_audioMixer.SetFloat(MIXER_MASTER, 22000.0f);
        M_playerManager.GetComponent<PlayerManagerScript>().Resume();
    }

    public void Paused()
    {
        M_Rendering.BlurBackground();
        M_canvas.enabled = true;
        M_buttonsAndText.SetActive(true);
        M_healthBiscuit.SetActive(false);
        M_collectedFruits.SetActive(false);
        fruitText.text = M_playerManager.GetComponent<PlayerManagerScript>().M_FruitCollected.ToString() + "/11";
        respawnText.text = M_playerManager.GetComponent<PlayerManagerScript>().M_respawns.ToString();
        shotsText.text = M_playerManager.GetComponent<PlayerManagerScript>().M_shots.ToString();
        int l_timeMins = 0;
        int l_timeSecs = (int)M_playerManager.GetComponent<PlayerManagerScript>().M_timeElapsed;
        if (l_timeSecs > 60)
        {
            l_timeMins = l_timeSecs / 60;
            for (int i = 0; i < l_timeMins; i++)
            {
                l_timeSecs -= 60;
            }
        }

        if (l_timeMins > 0)
        {
            if (l_timeSecs < 10)
            {
                timeText.text = l_timeMins.ToString() + ":0" + l_timeSecs.ToString();
            }
            else
            {
                timeText.text = l_timeMins.ToString() + ":" + l_timeSecs.ToString();
            }
        }
        else
        {
            if (l_timeSecs < 10)
            {
                timeText.text = ":0" + l_timeSecs.ToString();
            }
            else
            {
                timeText.text = ":" + l_timeSecs.ToString();
            }
        }
        M_audioMixer.SetFloat(MIXER_MASTER, 200.0f);
        Cursor.lockState = CursorLockMode.None;
    }

    public IEnumerator FadeIn(Image a_image)
    {
        a_image.enabled = true;
        for (float i = 0; i <= 0.75f; i += Time.unscaledDeltaTime * 2)
        {
            // set color with i as alpha
            a_image.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }

    public IEnumerator FadeAway(Image a_image)
    {
        // loop over 1 second backwards
        for (float i = 0.75f; i >= 0; i -= Time.unscaledDeltaTime * 2)
        {
            // set color with i as alpha
            a_image.color = new Color(1, 1, 1, i);
            yield return null;
        }
        M_canvas.enabled = false;
    }
}
