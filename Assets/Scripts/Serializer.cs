using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class Serializer : MonoBehaviour
{

    public SerializedData data;

    void Awake()
    {
        Deserialize();
    }
    
    public void Serialize()
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("SerializedData", json);
        PlayerPrefs.Save();
    }

    public void Deserialize()
    {
        string json = PlayerPrefs.GetString("SerializedData", null);
        if (string.IsNullOrEmpty(json) == true) return; 
        data = JsonUtility.FromJson<SerializedData>(json);
    }

}
