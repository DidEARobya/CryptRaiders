using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerIndex : MonoBehaviour
{
    public PlayerPersistence playerPersistence;

    public static PlayerIndex instance;

    public List<PlayerCharacterData> playerData;
    public Dictionary<string, PlayerCharacterData> playerCharacters;

    void Awake()
    {
        if (instance == null)
        { 
            instance = this;
            playerPersistence.Init();

            playerData = playerPersistence.GetCharacters();

            if(playerData != null )
            {
                playerCharacters = new Dictionary<string, PlayerCharacterData>();

                for (int i = 0; i < playerData.Count; i++)
                {
                    playerCharacters.Add(playerData[i].GetData().characterName, playerData[i]);

                    string temp = playerData[i].GetData().characterName;
                }
            }
        }
    }

    public PlayerCharacterData LoadCharacterData(string name)
    {
        if (playerCharacters.ContainsKey(name))
        {
            return playerCharacters[name];
        }
        else
        {
            return null;
        }
    }
    public List<PlayerCharacterData> GetPlayerCharacters()
    {
        return new List<PlayerCharacterData>(playerCharacters.Values);
    }
    public void SaveCharacter(PlayerCharacterData character)
    {
        playerPersistence.AddCharacter(character);
    }
    private void OnApplicationQuit()
    {
        playerPersistence.SaveCharacters();
    }
}
