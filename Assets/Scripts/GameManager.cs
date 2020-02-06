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

    public Camera camera;
    public Animator cameraAnimations;
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

    private float timer = 0f;
    public bool isUnpausing = false;
    public bool pauseDelay = false;
    private float timeWhenPaused = 1;

    private void Awake()
    {
        Camera.main.orthographicSize = 1.408f * 16f / 9f / camera.aspect;
        ser.Deserialize();

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
        if (gameLost) return;

        if (paused)
        {
            if (Input.GetKey(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject() && !playerMovement.touchInput)
                Unpause();

            return;
        }

        ser.data.TimePlayed += Time.deltaTime;

        if (!isUnpausing)
        {
            timer += Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Menu) && pauseButton)
        {
            Pause();
        }

    }

    private void OnApplicationPause(bool isQuitting)
    {
        if (!gameLost && isQuitting && !paused)
            Pause();
    }

    public void Unpause()
    {
        paused = false;

        StartCoroutine(SlowStart(10, true));
        
        pauseMenu.SetActive(false);
    }

    public void Pause()
    {
        if (paused || gameLost || !pauseButton.activeInHierarchy || Time.timeScale == 0f) return;
        paused = true;
        if (!isUnpausing || gameLost)
            timeWhenPaused = Time.timeScale;
        Time.timeScale = 0f;

        ser.Serialize();
        
        pauseButton.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void LoseGame()
    {
        gameLost = true;
        timeWhenPaused = Time.timeScale;
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
        score++;
        ser.data.totalScore++;

        if(score > ser.data.BestScore)
        {
            ser.data.BestScore = score;
        }
        ser.Serialize();

        Time.timeScale = 1 + (score / 120f - .2f);
    }

    public IEnumerator ContinuePlaying()
    {
        LoseScreenUI.SetActive(false);

        cameraAnimations.updateMode = AnimatorUpdateMode.UnscaledTime;
        cameraAnimations.SetTrigger("ContinuePlaying");
        
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy e in enemies)
        {
            e.OnContinuePlaying();
        }

        Time.timeScale = 1;
        player.SetActive(true);

        yield return new WaitForSecondsRealtime(1);
        
        player.transform.rotation = Quaternion.identity;
        player.GetComponent<Player>().SetInvincible(0f);
        player.GetComponent<Player>().DestroyEffects();

        countDown.SetActive(true);
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(3);
        countDown.SetActive(false);
        StartCoroutine(SlowStart(30, true, 4.5f));
        gameLost = false;
    }

    

    IEnumerator SlowStart(int speed, bool activatePauseButton, float afterSeconds = 7.5f)
    {
        isUnpausing = true;
        for (int i = 0; i < speed && !paused; i++)
        {
            Time.timeScale = timeWhenPaused / speed * i;
            yield return new WaitForSecondsRealtime(.1f);
        }
        isUnpausing = false;

        if(activatePauseButton && !paused)
        {
            yield return new WaitForSeconds(afterSeconds);

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
