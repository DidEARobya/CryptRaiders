using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class CharacterObject : MonoBehaviour
{
    [SerializeField]
    public CharacterData characterData;

    [HideInInspector]
    public CharacterData activeData;
    [HideInInspector]
    public CharacterAI characterAI;
    private CharacterControl characterControl;

    private PlayerCharacterData playerData;

    [HideInInspector]
    public GameObject characterModel;

    [SerializeField]
    protected int level;

    protected int combatHealth;
    protected int combatAttack;
    protected int combatDefense;
    protected int combatSpeed;

    protected int health;
    protected int attack;
    protected int defense;
    protected int speed;

    [HideInInspector]
    public SkillObject skillOne, skillTwo, skillThree;

    [HideInInspector]
    public SkillObject prio1, prio2, prio3;

    [SerializeField, HideInInspector]
    public int skillTwoPrio, skillThreePrio;

    public static int maxTM = 1000;
    private int trueDmg;
    private float dmgMulti;
    private float turnMeter;

    private float delay;
    private bool saved;

    // Start is called before the first frame update
    public void Init(CharacterControl control)
    {
        if (characterData != null)
        {
            activeData = characterData;
            characterControl = control;

            string name = characterData.characterName;

            UnityEngine.Object load = Resources.Load("CharacterModels/" + name);

            if(load != null)
            {
                characterModel = load as GameObject;
            }
            else
            {
                Debug.Log("No Character Model: " + characterData.characterName);
            }

            load = Resources.Load("CharacterSkills/" + name + "/" + name + "SkillOne");

            if(load != null)
            {
                skillOne = this.AddComponent<SkillObject>();
                skillOne.skillData = load as SkillData;
                skillOne.Init(this);
            }
            else
            {
                Debug.Log("No Skill One: " + characterData.characterName);
            }

            load = Resources.Load("CharacterSkills/" + name + "/" + name + "SkillTwo");

            if (load != null)
            {
                skillTwo = this.AddComponent<SkillObject>();
                skillTwo.skillData = load as SkillData;
                skillTwo.Init(this);
            }
            else
            {
                Debug.Log("No Skill Two: " + characterData.characterName);
            }

            load = Resources.Load("CharacterSkills/" + name + "/" + name + "SkillThree");

            if (load != null)
            {
                skillThree = this.AddComponent<SkillObject>();
                skillThree.skillData = load as SkillData;
                skillThree.Init(this);
            }
            else
            {
                Debug.Log("No Skill Two: " + characterData.characterName);
            }

            if(skillTwoPrio == 0)
            {
                skillTwoPrio = activeData.skillTwoPrio;
            }
            if(skillThreePrio == 0)
            {
                skillThreePrio = activeData.skillThreePrio;
            }

            if(skillTwoPrio == 1 && prio1 == null)
            {
                prio1 = skillTwo;
            }
            else if (skillTwoPrio == 2 && prio2 == null)
            {
                prio2 = skillTwo;
            }

            if(skillThreePrio == 1 && prio1 == null)
            {
                prio1 = skillThree;
            }
            else if (skillThreePrio == 2 && prio2 == null)
            {
                prio2 = skillThree;
            }

            prio3 = skillOne;

        }
        if(characterAI == null)
        {
            characterAI = this.AddComponent<CharacterAI>();

            if(characterAI != null)
            {
                characterAI.Init(this, control, control.skillManager);
            }
        }
    }
    public void PlayerIndexInit(PlayerCharacterData data)
    {
        if (playerData == null)
        {
            playerData = data;
            playerData.LoadObject(this);
        }

        if (characterData != null)
        {
            activeData = characterData;

            string name = characterData.characterName;

            UnityEngine.Object load = Resources.Load("CharacterModels/" + name);

            if (load != null)
            {
                characterModel = load as GameObject;
            }
            else
            {
                Debug.Log("No Character Model: " + characterData.characterName);
            }

            load = Resources.Load("CharacterSkills/" + name + "/" + name + "SkillOne");

            if (load != null)
            {
                skillOne = this.AddComponent<SkillObject>();
                skillOne.skillData = load as SkillData;
                skillOne.IndexInit();
            }
            else
            {
                Debug.Log("No Skill One: " + characterData.characterName);
            }

            load = Resources.Load("CharacterSkills/" + name + "/" + name + "SkillTwo");

            if (load != null)
            {
                skillTwo = this.AddComponent<SkillObject>();
                skillTwo.skillData = load as SkillData;
                skillTwo.IndexInit();
            }
            else
            {
                Debug.Log("No Skill Two: " + characterData.characterName);
            }

            load = Resources.Load("CharacterSkills/" + name + "/" + name + "SkillThree");

            if (load != null)
            {
                skillThree = this.AddComponent<SkillObject>();
                skillThree.skillData = load as SkillData;
                skillThree.IndexInit();
            }
            else
            {
                Debug.Log("No Skill Two: " + characterData.characterName);
            }

            SetPlayerIndexStats();
        }
    }
    // Update is called once per frame
    public void Update()
    {
        delay += Time.deltaTime;

        if (delay >= 2.0f && saved == false)
        {
        //    if (characterData.characterName == "Vuduin" || characterData.characterName == "Alvi")
        //    {
        //        level += 5;
        //    }

            //if (characterData.characterName == "Vuduin" || characterData.characterName == "Alvi")
            //{
            //     PlayerCharacterData temp = ScriptableObject.CreateInstance<PlayerCharacterData>();

            //     temp.SaveData(this);

            //     GameObject.Find("PlayerIndex").GetComponent<PlayerIndex>().SaveCharacter(temp);
            //}

            saved = true;
        }
    }

    public void SetActiveStats()
    {
        if (playerData != null)
        {
            playerData.LoadObject(this);
        }

        combatHealth = Mathf.RoundToInt(characterData.GetBaseHealth() * (1 + (level * 0.01f)));
        combatAttack = Mathf.RoundToInt(characterData.GetBaseAttack() * (1 + (level * 0.01f)));
        combatDefense = Mathf.RoundToInt(characterData.GetBaseDefense() * (1 + (level * 0.01f)));
        combatSpeed = Mathf.RoundToInt(characterData.GetBaseSpeed() * (1 + (level * 0.005f)));

        health = combatHealth;
        attack = combatAttack;
        defense = combatDefense;
        speed = combatSpeed;
        turnMeter = 0;
        dmgMulti = 1;

        delay = 0;
        saved = false;
    }
    public void SetActiveStats(int lvl)
    {
        level = lvl;

        combatHealth = Mathf.RoundToInt(characterData.GetBaseHealth() * (1 + (level * 0.01f)));
        combatAttack = Mathf.RoundToInt(characterData.GetBaseAttack() * (1 + (level * 0.01f)));
        combatDefense = Mathf.RoundToInt(characterData.GetBaseDefense() * (1 + (level * 0.01f)));
        combatSpeed = Mathf.RoundToInt(characterData.GetBaseSpeed() * (1 + (level * 0.005f)));

        health = combatHealth;
        attack = combatAttack;
        defense = combatDefense;
        speed = combatSpeed;
        turnMeter = 0;
        dmgMulti = 1;

        delay = 0;
        saved = false;
    }
    private void SetPlayerIndexStats()
    {
        combatHealth = Mathf.RoundToInt(activeData.GetBaseHealth() * (1 + (level * 0.01f)));
        combatAttack = Mathf.RoundToInt(activeData.GetBaseAttack() * (1 + (level * 0.01f)));
        combatDefense = Mathf.RoundToInt(activeData.GetBaseDefense() * (1 + (level * 0.01f)));
        combatSpeed = Mathf.RoundToInt(activeData.GetBaseSpeed() * (1 + (level * 0.005f)));

        health = combatHealth;
        attack = combatAttack;
        defense = combatDefense;
        speed = combatSpeed;
    }

    public void SetPlayerData(PlayerCharacterData data)
    {
        playerData = data;
    }
    public PlayerCharacterData GetPlayerData()
    {
        return playerData;
    }
    public void SetLevel(int lvl)
    {
        level = lvl;
    }
    public void SetAttack(int atk)
    {
        attack = atk;
    }
    public void SetDefense(int def)
    {
        defense = def;
    }
    public void AdjustDefense(float diff)
    {
        defense = Mathf.RoundToInt(defense + diff);
    }
    public void AdjustAttack(float diff)
    {
        attack = Mathf.RoundToInt(attack + diff);
    }
    public void AdjustSpeed(float diff)
    {
        speed = Mathf.RoundToInt(speed + diff);
    }

    public void DealDamage(float dmg)
    {
        trueDmg = Mathf.RoundToInt(dmg);

        if (health - trueDmg > 0)
        {
            health = Mathf.RoundToInt(health - trueDmg);
        }
        else
        {
            health = 0;
        }

        characterControl.characterGUI.StartHealthCoroutine(trueDmg, true);
    }
    public void TakeDamage(float dmg)
    {
        float mitigation;
        mitigation = dmg / (dmg + defense);

        float temp;
        temp = dmg * mitigation;

        trueDmg = Mathf.RoundToInt(temp * dmgMulti);

        if (health - trueDmg > 0)
        {
            health = health - trueDmg;
        }
        else
        {
            health = 0;
        }

        characterControl.characterGUI.StartHealthCoroutine(trueDmg, true);
    }
    public void TakeDamage(float dmg, float ignoreDef)
    {
        float mitigation;
        float iDef = 1 - (ignoreDef / 100);

        mitigation = dmg / (dmg + (defense * iDef));

        float temp;
        temp = dmg * mitigation;

        trueDmg = Mathf.RoundToInt(temp * dmgMulti);

        if (health - trueDmg > 0)
        {
            health = health - trueDmg;
        }
        else
        {
            health = 0;
        }

        characterControl.characterGUI.StartHealthCoroutine(trueDmg, true);
    }

    public void Heal(float heal)
    {
        int trueHeal = Mathf.RoundToInt(heal);

        if (health + trueHeal < this.GetCombatHealth())
        {
            health = health + trueHeal;
        }
        else
        {
            trueHeal = this.GetCombatHealth() - health;
            health = this.GetCombatHealth();
        }

        characterControl.characterGUI.StartHealthCoroutine(trueHeal, false);
    }

    public void TickTM()
    {
        turnMeter += speed;
    }
    public void ResetTM()
    {
        turnMeter = 0;
    }
    public void AdjustTM(float amount)
    {
        float temp = Mathf.RoundToInt(maxTM * amount);

        turnMeter += temp;
    }

    public void AdjustDmgMulti(float amount)
    {
        dmgMulti += amount;
    }

    public void ResetSkillCooldowns()
    {
        if(skillTwo != null)
        {
            skillTwo.ResetCooldown();
        }

        if(skillThree != null)
        {
            skillThree.ResetCooldown();
        }
    }
    public void ReduceSkillCooldowns(int amount)
    {
        if (skillTwo != null)
        {
            skillTwo.ReduceCooldown(amount);
        }

        if (skillThree != null)
        {
            skillThree.ReduceCooldown(amount);
        }
    }
    public float GetTM()
    {
        return turnMeter;
    }
    public string GetCharacterName()
    {
        return characterData.characterName;
    }
    public int GetLevel()
    {
        return level;
    }
    public int GetTrueDmg()
    {
        return trueDmg;
    }
    public int GetHealth()
    {
        return health;
    }
    public int GetCombatHealth()
    {
        return combatHealth;
    }
    public int GetDefense()
    {
        return defense;
    }
    public int GetCombatDefense()
    {
        return combatDefense;
    }
    public int GetAttack()
    {
        return attack;
    }
    public int GetCombatAttack()
    {
        return combatAttack;
    }
    public int GetSpeed()
    {
        return speed;
    }
    public int GetCombatSpeed()
    {
        return combatSpeed;
    }

    public GameObject GetCharacterModel()
    {
        return characterModel;
    }
    public CharacterControl GetControl()
    {
        return characterControl;
    }
}
