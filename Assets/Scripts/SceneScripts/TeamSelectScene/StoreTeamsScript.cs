using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StoreTeamsScript : MonoBehaviour
{
    public static StoreTeamsScript instance;

    private List<PlayerCharacterData> playerTeam;
    private List<CharacterData> enemyTeam;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void SetPlayerTeam(List<PlayerCharacterData> team)
    {
        playerTeam = team;
    }
    public void SetEnemyTeam(List<CharacterData> team)
    {
        enemyTeam = team;
    }
    public List<PlayerCharacterData> GetPlayerTeam()
    {
        return playerTeam;
    }
    public List<CharacterData> GetEnemyTeam()
    {
        return enemyTeam;
    }
}
