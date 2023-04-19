using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManagerScript : UIManagerScript
{
    public Canvas M_canvas;
    public Image M_pausePanel;
    public GameObject M_playerManager;
    public GameObject M_buttonsAndText;
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

        if (Input.GetButtonUp("Cancel") || Input.GetKeyDown(KeyCode.P))
        {
            Resume();
        }
    }

    public void Resume()
    {
        M_buttonsAndText.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(FadeAway(M_pausePanel));
        M_playerManager.GetComponent<PlayerManagerScript>().Resume();
    }

    public void Paused()
    {
        M_canvas.enabled = true;
        M_buttonsAndText.SetActive(true);
        StartCoroutine(FadeIn(M_pausePanel));
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
