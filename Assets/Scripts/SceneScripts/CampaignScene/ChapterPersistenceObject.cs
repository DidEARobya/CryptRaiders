using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterPersistenceObject : MonoBehaviour
{
    public static ChapterPersistenceObject instance;

    public ChapterPersistence chapterPersistence;
    public List<CampaignStageData> stageData;

    public bool ready;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);

            stageData = chapterPersistence.LoadStages(SceneManager.GetActiveScene().name);
            ready = true;
        }
    }

    public void SaveChapter()
    {
        chapterPersistence.SaveChapter();
    }
    public void AddStage(CampaignStageData data)
    {
        chapterPersistence.AddStage(data);
    }
    public void Init(string name)
    {
        chapterPersistence.Init(name);
    }
    private void OnApplicationQuit()
    {
        chapterPersistence.SaveChapter();
    }
}