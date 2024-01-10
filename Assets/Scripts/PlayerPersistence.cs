using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class PlayerPersistence : SerializableDictionary<string, PlayerCharacterData>
{
    [SerializeField]
    public List<PlayerCharacterData> dataList;

    public PlayerBackUp playerBackUp;

    public void Init()
    {
        playerBackUp = ScriptableObject.CreateInstance<PlayerBackUp>();
        
        for (int i = 0; i < dataList.Count; i++)
        {
            this.Add(dataList[i].GetData().characterName, dataList[i]);
        }
        

        if (File.Exists(Application.dataPath + "/PlayerCharacters.txt") != true)
        {
            SaveCharacters();
            Debug.Log("Saved");
        }
        else
        {
            LoadCharacters();
        }
    }

    public void AddCharacter(PlayerCharacterData data)
    {
        if(keys.Contains(data.GetData().characterName) != true)
        {
            keys.Add(data.GetData().characterName);
            values.Add(data);
        }
    }
    public void SaveCharacters()
    {
        this.OnBeforeSerialize();

        playerBackUp.dataKey = keys;
        playerBackUp.dataValue = values;

        if (playerBackUp != null)
        {
            string json = JsonConvert.SerializeObject(playerBackUp, Formatting.Indented, new JsonSerializerSettings{ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
            File.WriteAllText(Application.dataPath + "/PlayerCharacters.txt", json);
        }

        keys.Clear();
        values.Clear();
    }

    public void LoadCharacters()
    {
        if (File.Exists(Application.dataPath + "/PlayerCharacters.txt"))
        {   
            string json = File.ReadAllText(Application.dataPath + "/PlayerCharacters.txt");
            playerBackUp = JsonConvert.DeserializeObject<PlayerBackUp>(json);
        }

        if(playerBackUp != null)
        {
            keys = playerBackUp.dataKey;
            values = playerBackUp.dataValue;
        }

        this.OnAfterDeserialize();
    }

    public List<PlayerCharacterData> GetCharacters()
    {
        return playerBackUp.dataValue;
    }
     
}
