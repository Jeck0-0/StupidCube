using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool gameLost = false;
    public bool paused = false;
    

    public int score = 0;

    public Animator cameraAnimations;
    public GameObject LoseScreenUI;
    public GameObject pauseButton;
    public GuiManager guiManager;
    public GameObject pauseMenu;
    public GameObject player;
    public GameObject countDown;

    public PostProcessVolume postProcess;

    private float timer = 0f;
    public bool isUnpausing = false;
    private float timeWhenPaused = 1;

    private void Awake()
    {
        Time.timeScale = 1f;
        if (PlayerPrefs.GetInt("Graphics", 0) == 1)
        {
            postProcess.enabled = true;
        }
    }

    private void Update()
    {
        if (gameLost || paused) return;
        
        if (!isUnpausing)
        {
            timer += Time.deltaTime;
            Time.timeScale = 1 + (timer / 120f - .25f);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Pause();
        }
        
    }
    

    public void Continue()
    {
        StartCoroutine(SlowStart());

        paused = false;

        pauseButton.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void Pause()
    {
        if (!isUnpausing || gameLost)
            timeWhenPaused = Time.timeScale;
        Time.timeScale = 0f;

        paused = true;

        pauseButton.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void LoseGame()
    {
        gameLost = true;
        timeWhenPaused = Time.timeScale;
        StartCoroutine(SlowMo());
        cameraAnimations.updateMode = AnimatorUpdateMode.Normal;
        cameraAnimations.SetTrigger("GameLost");
        player.SetActive(false);
        LoseScreenUI.SetActive(true);
        pauseButton.SetActive(false);
    }

    public void StartContinuePlaying()
    {
        Debug.Log("Continue1");
        StartCoroutine(ContinuePlaying());
    }

    public IEnumerator ContinuePlaying()
    {

        Debug.Log("Continue2");

        LoseScreenUI.SetActive(false);
        pauseButton.SetActive(true);
        
        cameraAnimations.updateMode = AnimatorUpdateMode.UnscaledTime;
        cameraAnimations.SetTrigger("ContinuePlaying");
        
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy e in enemies)
        {
            e.OnContinuePlaying();
        }

        Time.timeScale = 1f;
        player.SetActive(true);

        yield return new WaitForSecondsRealtime(1);
        
        player.transform.rotation = Quaternion.identity;
        player.GetComponent<Player>().SetInvincible(0f);
        player.GetComponent<Player>().DestroyEffects();

        countDown.SetActive(true);
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(3);
        countDown.SetActive(false);
        StartCoroutine(SlowStart(30));
        gameLost = false;
    }

    public void AddScore(int qty = 1)
    {
        score++;
        if(score > PlayerPrefs.GetInt("BestScore", 0))
        {
            PlayerPrefs.SetInt("BestScore", score);
            PlayerPrefs.Save();
        }
    }

    

    IEnumerator SlowStart(int speed = 15)
    {
        isUnpausing = true;
        for (int i = 0; i < speed; i++)
        {
            if (paused) break;

            Time.timeScale += timeWhenPaused / speed;
            yield return new WaitForSecondsRealtime(.1f);
        }
        isUnpausing = false;
    }

    IEnumerator SlowMo()
    {
        while(Time.timeScale > 0.06f)
        {
            Time.timeScale -= (Time.timeScale / 10f) + .05f;
            yield return new WaitForSecondsRealtime(0.2f);
        }
        Time.timeScale = 0;
    }
    
}
