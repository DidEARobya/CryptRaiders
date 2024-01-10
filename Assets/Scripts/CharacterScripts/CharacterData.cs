using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum scaleTypes
{
    ATK,
    HP,
    DEF
}
public enum roleTypes
{
    SUPPORT,
    OFFENSE
}

[System.Serializable]
[CreateAssetMenu(fileName = "CharacterData", menuName = "NewCharacter")]
public class CharacterData : ScriptableObject
{
    [SerializeField, JsonProperty]
    public string characterName;

    public int baselevel = 1;

    public int baseHealth = 0;
    public int baseAttack = 0;
    public int baseDefense = 0;
    public int baseSpeed = 0;

    public scaleTypes scaleType;
    public roleTypes roleType;

    public int skillTwoPrio;
    public int skillThreePrio;
    private bool isOwned;

    public virtual void Update()
    {

    }
    public int GetBaseLevel()
    {
        return baselevel;
    }
    public void ToggleIsOwned()
    {
        isOwned = !isOwned;
    }
    public bool IsOwned()
    {
        return isOwned;
    }
    public int GetBaseHealth()
    {
        return baseHealth;
    }
    public int GetBaseDefense()
    {
        return baseDefense;
    }
    public int GetBaseAttack()
    {
        return baseAttack;
    }
    public int GetBaseSpeed()
    {
        return baseSpeed;
    }
}
