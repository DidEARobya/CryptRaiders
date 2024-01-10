using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerBackUp : ScriptableObject
{
    [SerializeField]
    public List<string> dataKey;

    [SerializeField]
    public List<PlayerCharacterData> dataValue;
}
