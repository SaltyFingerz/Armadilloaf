using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelFinishScript : MonoBehaviour
{
    public PlayerManagerScript M_playerManager;
    public PauseManagerScript M_pauseManager;
    public AudioSource M_musicPlayer;
    public AudioClip M_levelEndTheme;
    public GameObject M_attachedObject;
    public TextMeshProUGUI M_timeTaken, M_fruitcollected, M_shotsTaken, M_respawns;
    public Image[] starImages = new Image[15];
    public int score;

    // Start is called before the first frame update
    void Start()
    {
      M_playerManager = FindObjectOfType<PlayerManagerScript>();
      M_pauseManager = FindObjectOfType<PauseManagerScript>();
        M_attachedObject.SetActive(false);
    }

    public void UpdateScores()
    {
        M_musicPlayer.Stop();
        M_musicPlayer.PlayOneShot(M_levelEndTheme);
        M_playerManager.levelCompleted = true;
        M_timeTaken.text = M_pauseManager.GetTimeString();
        M_fruitcollected.text = M_playerManager.M_FruitCollected.ToString() + " of 11";
        M_shotsTaken.text = M_playerManager.M_shots.ToString();
        M_respawns.text = M_playerManager.M_respawns.ToString();
    }

    public void UpdateStars()
    {
        int time = M_pauseManager.M_timeMins;
        if (M_pauseManager.M_timeMins > 2)
        {
            time = 2;
            if (M_pauseManager.M_timeMins > 4)
            {
                time = 1;
                if (M_pauseManager.M_timeMins > 6)
                {
                    time = 0;
                }
            }
        }
        else
        {
            time = 3;
        }

        score += time;

        if (time != 0)
        { 
           for (int i = time - 1; i < 2; i++)
           {
            starImages[i].color = new Color(0, 0, 0, 100);
           }

        }
        else
        {
            for (int i = time; i < 2; i++)
            {
                starImages[i].color = new Color32(0, 0, 0, 100);
            }
        }

        int fruits = 0;

        if (M_playerManager.M_FruitCollected < 11)
        {
            fruits = 2;
            if (M_playerManager.M_FruitCollected < 5)
            {
                fruits = 1;
                if (M_playerManager.M_FruitCollected < 1)
                {
                    fruits = 0;
                }
            }
        }
        else
        {
            fruits = 3;
        }

        score += fruits;

        if (fruits != 3)
        { 

           for (int i = fruits + (3); i < 6; i++)
           {
            starImages[i].color = new Color32(0, 0, 0, 100);
           }
         }
        
        int shots = 0;

        if (M_playerManager.M_shots > 10)
        {
            shots = 2;
            if (M_playerManager.M_shots > 20)
            {
                shots = 1;
                if (M_playerManager.M_shots > 30)
                {
                    shots = 0;
                }
            }
        }
        else
        {
            shots = 3;
        }

        score += shots;

        if (shots != 3)
        {
            for (int i = shots + (6); i < 9; i++)
            {
                starImages[i].color = new Color32(0, 0, 0, 100);
            }

        }

        int respawns = 0;

        if (M_playerManager.M_respawns > 1)
        {
            respawns = 2;
            if (M_playerManager.M_respawns > 3)
            {
                respawns = 1;
                if (M_playerManager.M_respawns > 5)
                {
                    respawns = 0;
                }
            }
        }
        else
        {
            respawns = 3;
        }

        score += respawns;

        if (respawns != 3)
        {
            for (int i = respawns - + (6); i < 12; i++)
            {
                starImages[i].color = new Color32(0, 0, 0, 100);
            }

        }

        if (score < 12 && score >= 8)
        {
                starImages[14].color = new Color32(0, 0, 0, 100);
        }
        else if (score < 8 && score > 0)
        {
            for (int i = 13; i < 14; i++)
            {
                starImages[i].color = new Color32(0, 0, 0, 100);
            }
        }
        else
        {
            for (int i = 12; i < 14; i++)
            {
                starImages[i].color = new Color32(0, 0, 0, 100);
            }
        }

    }

    public void ResetStars()
    {
        foreach(Image star in starImages)
        {
            star.color = new Color32(255, 255, 255, 100);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
