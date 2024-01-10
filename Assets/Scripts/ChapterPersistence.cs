using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class ChapterPersistence : SerializableDictionary<string, CampaignStageData>
{
    [SerializeField]
    public List<CampaignStageData> dataList;

    public ChapterBackUp chapterBackUp;

    private string chapterName;

    public void Init(string name)
    {
        if(chapterBackUp == null)
        {
            chapterBackUp = ScriptableObject.CreateInstance<ChapterBackUp>();
        }

        if(chapterName == null)
        {
            chapterName = name;
        }

        if(chapterName != null)
        {
            if (File.Exists(Application.dataPath + "/" + chapterName + ".txt") != true)
            {
                SaveChapter();
                Debug.Log("Saved");
            }
            else
            {
                Debug.Log("Nope");
            }
        }
    }

    public void AddStage(CampaignStageData data)
    {
        if (keys.Contains(data.GetStageName()) != true)
        {
            this.Add(data.GetStageName(), data);
        }
    }
    public void SaveChapter()
    {
        this.OnBeforeSerialize();

        chapterBackUp.dataKey = keys;
        chapterBackUp.dataValue = values;

        if (chapterBackUp != null)
        {
            string json = JsonConvert.SerializeObject(chapterBackUp, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            File.WriteAllText(Application.dataPath + "/" + chapterName + ".txt", json);
        }

        keys.Clear();
        values.Clear();
    }

    public void LoadChapter()
    {
        if (File.Exists(Application.dataPath + "/" + chapterName + ".txt"))
        {
            string json = File.ReadAllText(Application.dataPath + "/" + chapterName + ".txt");
            chapterBackUp = JsonConvert.DeserializeObject<ChapterBackUp>(json);
        }

        if (chapterBackUp != null)
        {
            keys = chapterBackUp.dataKey;
            values = chapterBackUp.dataValue;
        }

        this.OnAfterDeserialize();
    }
    public List<CampaignStageData> LoadStages(string name)
    {
        if (chapterBackUp == null)
        {
            chapterBackUp = ScriptableObject.CreateInstance<ChapterBackUp>();
        }

        chapterName = name;

        if (File.Exists(Application.dataPath + "/" + chapterName + ".txt") == true)
        {
            LoadChapter();

            if (chapterBackUp.dataValue != null)
            {
                return chapterBackUp.dataValue;
            }
        }

        return null;
    }
    public List<CampaignStageData> GetStages()
    {
        if(chapterBackUp.dataValue != null)
        {
            return null;
        }
        return chapterBackUp.dataValue;
    }

}
