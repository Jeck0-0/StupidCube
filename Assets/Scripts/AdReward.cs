using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdReward : MonoBehaviour
{

    public GameManager gameManager;
    public Button adButton;
    public GameObject errorText;

    public bool canRetry = true;
    public int reward = 0;
    
    private string placementId = "rewardedVideo";
    

    private void Update()
    {
        if (canRetry)
            adButton.interactable = Advertisement.IsReady(placementId);
        else
            adButton.interactable = false;
    }

    public void AdError()
    {
        errorText.SetActive(true);
    }
    
    public void AdStarted()
    {
        Debug.Log("Starting an ad");
        if (gameManager != null) gameManager.Pause();
    }
    
    public void Reward(int result)
    {
        if (reward == 1)
        {
            if (result == 2)
            {
                Debug.Log("Ad Watched");
                canRetry = false;
                gameManager.StartContinuePlaying();
            }
            else if (result == 1)
            {
                Debug.Log("Ad Skipped");
                canRetry = false;
                gameManager.StartContinuePlaying();

            }
            else if (result == 0)
            {
                Debug.LogWarning("Ad Failed");
                AdError();
            }
        }
        else
        {
            Debug.Log("No reward");
        }
    }

    public void WatchAd(int rew = 0)
    {
        Advertisement.Show(placementId);
        reward = rew;
    }

}
