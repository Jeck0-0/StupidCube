using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public string levelToPlay = "NormalGame";

    public GameObject menuUI;
    public GameObject optionsUI;
    public GameObject infoUI;

    public Text bestScore;
    public GameObject demotivationCheckmark;
    public GameObject graphicsCheckmarkAndImage;

    private void Start()
    {
        bestScore.text = PlayerPrefs.GetInt("BestScore", 0).ToString();

        if(PlayerPrefs.GetInt("Demotivate", 0) == 0)
            demotivationCheckmark.SetActive(false);
        else
            demotivationCheckmark.SetActive(true);

        if (PlayerPrefs.GetInt("Graphics", 0) == 0)
            graphicsCheckmarkAndImage.SetActive(false);
        else
            graphicsCheckmarkAndImage.SetActive(true);
    }

    public void Play()
    {
        SceneManager.LoadScene(levelToPlay);
    }

    public void Options()
    {
        menuUI.SetActive(!menuUI.active);
        optionsUI.SetActive(!optionsUI.active);
    }

    public void Info()
    {
        menuUI.SetActive(!menuUI.active);
        infoUI.SetActive(!infoUI.active);
    }
    
    public void ToggleGraphics()
    {
        if (PlayerPrefs.GetInt("Graphics", 0) == 0)
        {
            PlayerPrefs.SetInt("Graphics", 1);
            graphicsCheckmarkAndImage.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("Graphics", 0);
            graphicsCheckmarkAndImage.SetActive(false);
        }
        PlayerPrefs.Save();
    }

    public void ToggleDemotivationMode()
    {

        if(PlayerPrefs.GetInt("Demotivate", 0) == 0)
        {
            PlayerPrefs.SetInt("Demotivate", 1);
            demotivationCheckmark.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("Demotivate", 0);
            demotivationCheckmark.SetActive(false);

        }
        PlayerPrefs.Save();
    }

}
