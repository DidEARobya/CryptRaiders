using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreStageScript : MonoBehaviour
{
    public static StoreStageScript instance;

    private CampaignStageData stage;
    private List<CharacterObject> enemyTeam;
    private int enemyLevel;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
    public void StoreStage(CampaignStageData data, List<CharacterObject> team, int level)
    {
        stage = data;
        enemyTeam = team;
        enemyLevel = level;
    }

    public List<CharacterObject> GetEnemyTeam()
    {
        return enemyTeam;
    }
    public CampaignStageData GetStageData()
    {
        return stage;
    }
    public int GetEnemyLevel()
    {
        return enemyLevel;
    }
}
