using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsListener
{
    public AdReward adReward;
    public bool testMode = false;

    static int nscene = 0;

#if UNITY_IOS
    private string adID = "3447012";
#elif UNITY_ANDROID
    private string adID = "3447013";
#endif

    private string placementId = "rewardedVideo";

    void Start()
    {
        adReward = FindObjectOfType<AdReward>();

        if (nscene == 0)
        {
            Advertisement.AddListener(this);
            Advertisement.Initialize(adID, testMode);
        }
        nscene++;

        Debug.Log(nscene);
    }


    public void OnUnityAdsDidError(string message)
    {
        adReward.AdError();
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        adReward.AdStarted();
    }

    public void OnUnityAdsReady(string pId)
    {
        //keep this, ik it looks useless but keep this
    }

    public void OnUnityAdsDidFinish(string id, ShowResult result)
    {
        adReward = FindObjectOfType<AdReward>();

        if (result == ShowResult.Finished)
        {
            adReward.Reward(2);
        }
        else if (result == ShowResult.Skipped)
        {
            adReward.Reward(1);
        }
        else if (result == ShowResult.Failed)
        {
            adReward.Reward(0);
        }
    } 

    public void WatchAd(int rew = 0)
    {
        Advertisement.Show(placementId);
        adReward.reward = rew;
    }
    
}
