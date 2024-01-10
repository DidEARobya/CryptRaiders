using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

public enum characterStates
{
    INACTIVE,
    ACTIVE,
    DEAD,
    AUTO
}

public class CharacterControl : MonoBehaviour
{
    public CharacterObject character;
    public CharacterGUI characterGUI;
    public SkillManager skillManager;
    characterStates currentstate;

    private bool PlayerCharacter = false;

    private List<EffectBase> effects, debuffs, buffs;
    private int effectLimit;

    private bool InCombat = false;
    private bool active = false;

    public void Init()
    {
        if (character != null)
        {
            skillManager = this.AddComponent<SkillManager>();

            character.Init(this);
            LoadCharacter(character);
            SetState(characterStates.INACTIVE);

            effects = new List<EffectBase>();
            debuffs = new List<EffectBase>();
            buffs = new List<EffectBase>();
            effectLimit = 10;

            characterGUI = this.AddComponent<CharacterGUI>();
        }
    }

    public void IndexInit()
    {
        if(character != null)
        {
            LoadCharacter(character);
            SetState(characterStates.INACTIVE);
        }
    }

    public void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(InCombat == true)
        {
            character.Update();

            UpdateEffects();

            if (character.GetHealth() <= 0)
            {
                SetState(characterStates.DEAD);
            }

            if (this.GetState() == characterStates.DEAD)
            {
                character.ResetTM();
            }

            if (this.GetState() == characterStates.ACTIVE && active == false)
            {
                if (PlayerCharacter == true)
                {
                    gameObject.transform.position = this.transform.position + new Vector3(2, 0, 0);
                }
                else
                {
                    gameObject.transform.position = this.transform.position + new Vector3(-2, 0, 0);
                }

                active = true;
            }
            else if (this.GetState() == characterStates.AUTO && active == false)
            {
                if (PlayerCharacter == true)
                {
                    gameObject.transform.position = this.transform.position + new Vector3(2, 0, 0);
                }
                else
                {
                    gameObject.transform.position = this.transform.position + new Vector3(-2, 0, 0);
                }

                active = true;
            }
            else if (this.GetState() != characterStates.ACTIVE && this.GetState() != characterStates.AUTO && active == true)
            {
                if (PlayerCharacter == true)
                {
                    gameObject.transform.position = this.transform.position + new Vector3(-2, 0, 0);
                }
                else
                {
                    gameObject.transform.position = this.transform.position + new Vector3(2, 0, 0);
                }
                active = false;
            }
        }
    }

    private void LoadCharacter(CharacterObject obj)
    {
        foreach (Transform child in this.transform)
        {
            if (Application.isEditor)
            {
                DestroyImmediate(child.gameObject);
            }
            else
            {
                Destroy(child.gameObject);
            }
        }

        GameObject visuals = Instantiate(obj.GetCharacterModel());
        visuals.transform.SetParent(this.transform);
        visuals.transform.localPosition = Vector3.zero + Vector3.back;
        visuals.transform.rotation = Quaternion.identity;
    }

    public void DestroyCharacter()
    {
        foreach (Transform child in this.transform)
        {
            if (Application.isEditor)
            {
                DestroyImmediate(child.gameObject);
            }
            else
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void UpdateEffects()
    {
        if(effects.Count != debuffs.Count + buffs.Count)
        {
            effects.Clear();

            for(int i = 0; i < debuffs.Count; i++)
            {
                effects.Add(debuffs[i]);
            }

            for (int i = 0; i < buffs.Count; i++)
            {
                effects.Add(buffs[i]);
            }
        }

        characterGUI.UpdateEffects(effects);
    }
    public void AddEffect(EffectBase effect, int duration)
    {
        if(effect.effectRole == effectRole.BUFF)
        {
            if (effect.effectStackable == true)
            {
                if (buffs.Count < effectLimit)
                {
                    effect.StartDebuff(duration);
                    buffs.Add(effect);
                }
                else
                {
                    for (int i = 0; i < buffs.Count; i++)
                    {
                        if (buffs[i].GetType().ToString() == effect.GetType().ToString())
                        {
                            if (buffs[i].duration < duration)
                            {
                                buffs[i].StartDebuff(duration);
                                break;
                            }
                        }
                    }
                }
            }
            else if (effect.effectStackable == false)
            {
                if (buffs.Count == 0)
                {
                    effect.StartDebuff(duration);
                    buffs.Add(effect);
                }
                else
                {
                    bool temp = false;

                    for (int i = 0; i < buffs.Count; i++)
                    {
                        if (buffs[i].GetType().ToString() == effect.GetType().ToString())
                        {
                            temp = true;

                            if (buffs[i].duration < duration)
                            {
                                buffs[i].StartDebuff(duration);
                                break;
                            }
                        }
                    }

                    if (temp == false)
                    {
                        if (buffs.Count < effectLimit)
                        {
                            effect.StartDebuff(duration);
                            buffs.Add(effect);
                        }
                    }

                }
            }
        }
        else if (effect.effectRole == effectRole.DEBUFF)
        {
            if (effect.effectStackable == true)
            {
                if (debuffs.Count < effectLimit)
                {
                    effect.StartDebuff(duration);
                    debuffs.Add(effect);
                }
                else
                {
                    for (int i = 0; i < debuffs.Count; i++)
                    {
                        if (debuffs[i].GetType().ToString() == effect.GetType().ToString())
                        {
                            if (debuffs[i].duration < duration)
                            {
                                debuffs[i].StartDebuff(duration);
                                break;
                            }
                        }
                    }
                }
            }
            else if (effect.effectStackable == false)
            {
                if (debuffs.Count == 0)
                {
                    effect.StartDebuff(duration);
                    debuffs.Add(effect);
                }
                else
                {
                    bool temp = false;

                    for (int i = 0; i < debuffs.Count; i++)
                    {
                        if (debuffs[i].GetType().ToString() == effect.GetType().ToString())
                        {
                            temp = true;

                            if (debuffs[i].duration < duration)
                            {
                                debuffs[i].StartDebuff(duration);
                                break;
                            }
                        }
                    }

                    if (temp == false)
                    {
                        if (debuffs.Count < effectLimit)
                        {
                            effect.StartDebuff(duration);
                            debuffs.Add(effect);
                        }
                    }

                }
            }
        }
    }

    public void RemoveEffect(EffectBase effect)
    {
        if(effect.effectRole == effectRole.BUFF)
        {
            for (int i = 0; i < buffs.Count; i++)
            {
                if (buffs[i] == effect)
                {
                    buffs.Remove(buffs[i]);
                    Destroy(effect);
                    break;
                }
            }
        }
        else if (effect.effectRole == effectRole.DEBUFF)
        {
            for (int i = 0; i < debuffs.Count; i++)
            {
                if (debuffs[i] == effect)
                {
                    debuffs.Remove(debuffs[i]);
                    Destroy(effect);
                    break;
                }
            }
        }
    }

    public void ClearEffects(effectRole role)
    {
        if(role == effectRole.BUFF)
        {
            for (int i = 0; i < buffs.Count; i++)
            {
                buffs[i].duration = 0;
            }
        }
        if (role == effectRole.DEBUFF)
        {
            for (int i = 0; i < debuffs.Count; i++)
            {
                debuffs[i].duration = 0;
            }
        }
    }
    public EffectBase GetRandomEffect(effectRole role)
    {
        int temp;

        if (role == effectRole.BUFF)
        {
            temp = Random.Range(0, buffs.Count);
            return buffs[temp];
        }
        else if (role == effectRole.DEBUFF)
        {
            temp = Random.Range(0, debuffs.Count);
            return debuffs[temp];
        }
        else
        {
            return null;
        }
    }
    public void SetState(characterStates state)
    {
        currentstate = state;
    }
    public characterStates GetState()
    {
        return currentstate;
    }
    public void TogglePlayable()
    {
        PlayerCharacter = !PlayerCharacter;
    }
    public void ToggleCombat()
    {
        InCombat = true;
    }
    public bool CombatCheck()
    {
        return InCombat;
    }
    public bool PlayerCheck()
    {
        return PlayerCharacter;
    }
}
