using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

[System.Serializable]
public class PlayerCharacterData : ScriptableObject
{ 
    public CharacterData characterData;

    public int level;
    public int skillTwoPrio, skillThreePrio;

    public CharacterData GetData()
    {
        return characterData;
    }

    public void SaveData(CharacterObject obj)
    {
        characterData = obj.characterData;

        level = obj.GetLevel();
        skillTwoPrio = obj.skillTwoPrio;
        skillThreePrio = obj.skillThreePrio;
    }
    public CharacterObject LoadObject(CharacterObject temp)
    {
        temp.characterData = characterData;
        temp.SetLevel(level);
        temp.skillTwoPrio = skillTwoPrio;
        temp.skillThreePrio = skillThreePrio;

        return temp;
    }
}
