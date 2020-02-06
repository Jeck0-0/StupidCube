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
    public Serializer ser;

    public string[] demotivationQuotes;


    private void Start()
    {
        if (ser.data.Demotivate)
        {
            demotivationQuote.text = demotivationQuotes[Random.Range(0, demotivationQuotes.Length)];
        }
        else
        {
            demotivationQuote.text = "";
        }

        bestScoreText.text = ser.data.BestScore.ToString();
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
