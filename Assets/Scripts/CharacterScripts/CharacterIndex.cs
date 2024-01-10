using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CharacterIndex : MonoBehaviour
{
    public static CharacterIndex instance;

    public List<CharacterObject> objectList;

    private Dictionary<string, CharacterObject> characterDictionary;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            characterDictionary = new Dictionary<string,CharacterObject>();

            for (int i = 0; i < objectList.Count; i++)
            {
                characterDictionary.Add(objectList[i].GetCharacterName(), objectList[i]);
            }
        }
    }
    public Dictionary<string, CharacterObject> GetDictionary()
    {
        return characterDictionary;
    }

    public CharacterObject GetCharacter(string name)
    {
        return characterDictionary[name];
    }
}
