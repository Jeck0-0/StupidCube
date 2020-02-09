using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public bool gameLost = false;
    public bool paused = false;

    public int score = 0;
    public int lanes = 5;

    public Camera camera;
    public Animator cameraAnimations;
    public Animator scoreEffects;
    public GameObject LoseScreenUI;
    public GameObject pauseButton;
    public GuiManager guiManager;
    public GameObject pauseMenu;
    public GameObject player;
    public GameObject countDown;
    public GameObject playerTrailLine;
    public GameObject playerTrailParticles;

    public PlayerMovement playerMovement;
    public Serializer ser;
    public PostProcessVolume postProcess;

    private float cameraWidth;
    public bool isUnpausing = false;
    private bool canAddScore = true;

    private float getTime() { return 1 + (score / 150f); }

    private void Awake()
    {
        ser.Deserialize();

        cameraWidth = lanes;
        Time.timeScale = 1f;

        postProcess.enabled = ser.data.Graphics;
        if (!ser.data.Graphics)
        {
            Destroy(playerTrailLine);
            Destroy(playerTrailParticles);
        }

        ser.data.GamesPlayed++;
        ser.Serialize();
    }

    private void Update()
    {
        camera.orthographicSize = (cameraWidth / 2) / camera.aspect; 

        if (gameLost) return;

        if (paused)
        {
            if (Input.GetKey(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject() && !playerMovement.touchInput)
                Unpause();

            return;
        }

        ser.data.TimePlayed += Time.deltaTime;
        

        if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Menu) )
        {
            if (!paused)
            {
                if (pauseButton)
                    Pause();
            }
            else
            {
                Application.Quit();
            }
        }

    }
    
    private void OnApplicationPause(bool isQuitting)
    {
        if (!gameLost && isQuitting && !paused)
            Pause(true);
    }

    public void Unpause()
    {
        paused = false;

        StartCoroutine(SlowStart(10, true));
        
        pauseMenu.SetActive(false);
    }

    public void Pause(bool force = false)
    {
        if (paused || gameLost || Time.timeScale == 0f) return;

        if (!force && !pauseButton.activeInHierarchy) return; // if forced continue even if button is hidden

        paused = true;
        Time.timeScale = 0f;

        ser.Serialize();
        
        pauseButton.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void LoseGame()
    {
        gameLost = true;
        StartCoroutine(SlowStop());
        cameraAnimations.updateMode = AnimatorUpdateMode.Normal;
        cameraAnimations.SetTrigger("GameLost");
        pauseMenu.SetActive(false);
        player.SetActive(false);
        LoseScreenUI.SetActive(true);
        pauseButton.SetActive(false);
        ser.Serialize();
    }

    public void StartContinuePlaying()
    {
        this.StartCoroutine(ContinuePlaying());
    }

    public void AddScore(int qty = 1)
    {
        if (!canAddScore) return;
        score++;
        ser.data.totalScore++;

        if(score > ser.data.BestScore)
        {
            ser.data.BestScore = score;
        }
        ser.Serialize();

        if(score%100 == 0)
        {
            scoreEffects.SetTrigger("100Multiple");
            //lanes += 2;
            //StartCoroutine(CameraFixView());
        }
        else if(score%25 == 0)
        {
            scoreEffects.SetTrigger("25Multiple");

        }
        else if(score == 666)
        {
            StartCoroutine(score666());
        }

        Time.timeScale = getTime();
    }

    public IEnumerator score666()
    {
        canAddScore = false;
        scoreEffects.SetTrigger("666");
        player.GetComponent<Player>().SetInvincible(1000f);

        yield return new WaitForSecondsRealtime(4.5f);
        Application.Quit();
        player.GetComponent<Player>().SetInvincible(3f);
        score = 69;
        StartContinuePlaying();
        canAddScore = true;
    }

    public IEnumerator CameraFixView()
    {
        for(int i = 0; i < 40; i++)
        {
            cameraWidth = Mathf.Lerp(cameraWidth, lanes, .1f);
            yield return new WaitForSecondsRealtime(.04f);
        }
        cameraWidth = lanes;
    }


    public IEnumerator ContinuePlaying()
    {
        LoseScreenUI.SetActive(false);

        cameraAnimations.updateMode = AnimatorUpdateMode.UnscaledTime;
        cameraAnimations.SetTrigger("ContinuePlaying");
        canAddScore = false;
        Time.timeScale = 1;

        yield return new WaitForSecondsRealtime(1);

        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy e in enemies)
            e.Destroy(0f);
        
        player.SetActive(true);
        player.transform.rotation = Quaternion.identity;
        player.GetComponent<Player>().SetInvincible(1f);
        player.GetComponent<Player>().DestroyEffects();

        countDown.SetActive(true);
        Time.timeScale = 0f;
        canAddScore = true;

        yield return new WaitForSecondsRealtime(3);
        countDown.SetActive(false);
        StartCoroutine(SlowStart(30, true, 3.5f));
        gameLost = false;
    }

    

    IEnumerator SlowStart(int speed, bool activatePauseButton, float afterSeconds = 3.5f)
    {
        isUnpausing = true;
        float timeToReach = getTime();
        for (int i = 0; i < speed && !paused; i++)
        {
            Time.timeScale = timeToReach / speed * i;
            yield return new WaitForSecondsRealtime(.1f);
        }
        isUnpausing = false;

        if(activatePauseButton && !paused)
        {
            yield return new WaitForSecondsRealtime(afterSeconds);

            if(!gameLost && !paused)
                pauseButton.SetActive(true);
        }
    }
    


    IEnumerator SlowStop()
    {
        while(Time.timeScale > 0.06f)
        {
            Time.timeScale -= (Time.timeScale / 10f) + .05f;
            yield return new WaitForSecondsRealtime(0.2f);
        }
        Time.timeScale = 0;
    }
    
}
