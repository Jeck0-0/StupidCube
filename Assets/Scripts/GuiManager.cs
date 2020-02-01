using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GuiManager : MonoBehaviour
{
    public Text scoreText;
    public Text bestScoreText;
    public Text demotivationQuote;

    public GameManager gameManager;

    public string[] demotivationQuotes;


    private void Start()
    {
        if (PlayerPrefs.GetInt("Demotivate") == 0)
        {
            demotivationQuote.text = "";
        }
        else
        {
            demotivationQuote.text = demotivationQuotes[Random.Range(0, demotivationQuotes.Length)];
        }

        bestScoreText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
    }

    private void Update()
    {
        scoreText.text = gameManager.score.ToString();
    }
    

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }


}
