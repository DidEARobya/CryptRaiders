using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public enum skillRoles
{
    SUPPORT,
    ATTACK,
    REVIVE,
    HEAL
}

public enum skillType
{
    SINGLETARGET,
    MULTITARGET,
    AOE,
    SELF
}

public enum skillScaleType
{
    ATK,
    DEF,
    HP
}

public abstract class SkillData : ScriptableObject
{
    public string skillName;
    public string skillDescription;
    public skillRoles skillRole;
    public GameObject skillIcon;
    public int skillCooldown;

    public skillScaleType skillScaleType;
    public skillType skillType;

    public int amountOfTargets;

    protected CharacterObject skillParent;
    protected bool extraTurn;
    public abstract void Activate(CharacterObject character, CharacterControl target, List<CharacterControl> controlList, int amount);
    public virtual void SingleTarget(CharacterControl target) { }
    public virtual void MultiTarget(List<CharacterControl> controlList, int amount) { }
    public virtual void AllTargets(List<CharacterControl> controlList) { }
    public virtual void SelfTarget(CharacterControl target) { }

    public void SetSkillParent(CharacterObject parent)
    {
        skillParent = parent;
    }
}
