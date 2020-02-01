using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsListener
{
    public GameManager gameManager = null;
    public Button adToComtinueButton = null;

    private bool canRetry = true;

    private int reward = 0;

    private string adID = "3447013";
    private string placementId = "rewardedVideo";

    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(adID, true);
    }

    private void Update()
    {
        if(canRetry)
            adToComtinueButton.interactable = Advertisement.IsReady(placementId);    
    }

    public void OnUnityAdsReady(string id)
    {
        // If the ready Placement is rewarded, activate the button:
        if (adToComtinueButton != null && id == placementId)
        {
            adToComtinueButton.interactable = true;
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.LogError("Ad error idk wtf");
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("Starting an ad");
    }

    public void OnUnityAdsDidFinish(string id, ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            if (reward == 1)
            {
                if (result == ShowResult.Finished)
                {
                    gameManager.StartContinuePlaying();
                }
                else if (result == ShowResult.Skipped)
                {
                    canRetry = false;
                    adToComtinueButton.interactable = false;
                    gameManager.StartContinuePlaying();

                }
                else if (result == ShowResult.Failed)
                {
                    Debug.LogError("Video failed to show");
                }
            }
        }
    }

    public void AdToContinue(int rew = 0)
    {
        Advertisement.Show(placementId);
        reward = rew;
    }
    
}
