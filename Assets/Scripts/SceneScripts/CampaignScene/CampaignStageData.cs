using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class CampaignStageData : ScriptableObject
{
    public bool isComplete, isActive;
    public string stageName;

    public void SetIsComplete(bool data)
    {
        isComplete = data;
    }
    public bool GetIsComplete()
    {
        return isComplete;
    }

    public void SetIsActive(bool data)
    {
        isActive = data;
    }
    public bool GetIsActive()
    {
        return isActive;
    }
    public void SetStageName(string name)
    {
        stageName = name;
    }
    public string GetStageName()
    {
        return stageName;
    }
}
