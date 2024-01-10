using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChapterBackUp : ScriptableObject
{
    [SerializeField]
    public List<string> dataKey;

    [SerializeField]
    public List<CampaignStageData> dataValue;
}
