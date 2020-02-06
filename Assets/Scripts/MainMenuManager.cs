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
    public GameObject statsUI;

    public GameObject demotivationCheckmark;
    public GameObject graphicsCheckmarkAndImage;
    
    public Animator startGameAnim;

    public Text bestScoreText;
    public Text statsBestScore;
    public Text statsTotalScore;
    /*public Text statsNormalEnemiesSurvived;
    public Text statsFastEnemiesSurvived;
    public Text statsSlowEnemiesSurvived;
    public Text statsLaserEnemiesSurvived;*/
    public Text statsAverageScore;
    public Text statsTotalTime;
    public Text statsGamesPlayed;
    public Text statsDistanceTravelled;

    public Serializer ser;

    static private bool playCreditsAnimation = true;

    private void Start()
    {
        bestScoreText.text = ser.data.BestScore.ToString();
        UpdateStats();

        Time.timeScale = 1f;

        if (!playCreditsAnimation)
            startGameAnim.SetTrigger("SkipCredits");
        playCreditsAnimation = false;
        
        demotivationCheckmark.SetActive(ser.data.Demotivate);
        graphicsCheckmarkAndImage.SetActive(ser.data.Graphics);

        UpdateStats();
    }



    public void Play()
    {
        startGameAnim.SetTrigger("StartGame");
        StartCoroutine(StartAfterSeconds(levelToPlay, 1f));
    }

    public void Options()
    {
        menuUI.SetActive(!menuUI.activeInHierarchy);
        optionsUI.SetActive(!optionsUI.activeInHierarchy);
    }

    public void Info()
    {
        menuUI.SetActive(!menuUI.activeInHierarchy);
        infoUI.SetActive(!infoUI.activeInHierarchy);
    }

    public void Stats()
    {
        menuUI.SetActive(!menuUI.activeInHierarchy);
        statsUI.SetActive(!statsUI.activeInHierarchy);
        UpdateStats();
    }



    public void ToggleGraphics()
    {
        ser.data.Graphics = !ser.data.Graphics;
        graphicsCheckmarkAndImage.SetActive(ser.data.Graphics);
        ser.Serialize();
    }

    public void ToggleDemotivationMode()
    {
        ser.data.Demotivate = !ser.data.Demotivate;
        demotivationCheckmark.SetActive(ser.data.Demotivate);
        ser.Serialize();
    }
    
    public void ResetStats()
    {
        Debug.Log("skdoakdos");
        ser.data.BestScore = 0;
        ser.data.DistanceTravelled = 0;
        ser.data.GamesPlayed = 0;
        ser.data.TimePlayed = 0;
        ser.data.totalScore = 0;
        Debug.Log(ser.data.BestScore);
        Debug.Log(ser.data.totalScore);
        /*ser.data.FastEnemiesSurvived = 0;
        ser.data.LasersSurvived = 0;
        ser.data.NormalEnemiesSurvived = 0;
        ser.data.SlowEnemiesSurvived = 0;*/

        UpdateStats();

        ser.Serialize();
    }


    private void UpdateStats()
    {
        int average = 0;
        if(ser.data.GamesPlayed != 0)
            average = ser.data.totalScore / ser.data.GamesPlayed;
        
        statsAverageScore.text =            "Average score: " + average.ToString("0");
        statsTotalScore.text =              "Total score: " + ser.data.totalScore;
        statsGamesPlayed.text =             "Games played: " + ser.data.GamesPlayed;
        statsBestScore.text =               "Best score: " + ser.data.BestScore;
        statsDistanceTravelled.text =       "Distance travelled: " + ser.data.DistanceTravelled.ToString("0") + "m";
        statsTotalTime.text =               "Time played: " + ser.data.TimePlayed.ToString("0") + "s";

        /*statsNormalEnemiesSurvived.text =   ser.data.NormalEnemiesSurvived.ToString();
        statsFastEnemiesSurvived.text =     ser.data.FastEnemiesSurvived.ToString();
        statsSlowEnemiesSurvived.text =     ser.data.SlowEnemiesSurvived.ToString();
        statsLaserEnemiesSurvived.text =    ser.data.LasersSurvived.ToString();*/

        bestScoreText.text = ser.data.BestScore.ToString();
    }

    


    IEnumerator StartAfterSeconds(string scene,  float seconds = 1f)
    {
        yield return new WaitForSecondsRealtime(seconds);

        SceneManager.LoadScene(scene);
    }
}
