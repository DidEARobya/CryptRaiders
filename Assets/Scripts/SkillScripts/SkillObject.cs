using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class SkillObject : MonoBehaviour
{
    public SkillData skillData;
    [HideInInspector]
    public SkillData activeSkill;
    private CharacterObject skillParent;
    private CharacterControl characterControl;

    private int skillCooldown;
    private int cooldownTime;
    private bool cooldown;
    private bool ready;

    // Start is called before the first frame update
    public void Init(CharacterObject character)
    {
        ready = false;

        if(skillData != null)
        {
            activeSkill = skillData;
        }

        if(activeSkill != null)
        {
            skillCooldown = activeSkill.skillCooldown;
            cooldownTime = 0;

            skillParent = character;
            characterControl = skillParent.GetControl();

            this.transform.SetParent(skillParent.transform);
            ready = true;
        }
    }
    public void IndexInit()
    {
        ready = false;

        if (skillData != null)
        {
            activeSkill = skillData;
        }

        if (activeSkill != null)
        {
            skillCooldown = activeSkill.skillCooldown;
        }
    }
    private void Update()
    {
        if(ready == true)
        {
            if (characterControl.GetState() == characterStates.ACTIVE && cooldown == false)
            {
                this.ReduceCooldown();
                cooldown = true;
            }

            if (characterControl.GetState() == characterStates.INACTIVE)
            {
                cooldown = false;
            }
        }
    }

    public void Activate(CharacterObject character, CharacterControl target, List<CharacterControl> controlList, int amount)
    {
        activeSkill.Activate(character, target, controlList, amount);
    }
    public void IncreaseCooldown(int amount)
    {
        cooldownTime += amount;
    }
    public void ReduceCooldown()
    {
        if (cooldownTime > 0)
        {
            cooldownTime -= 1;
        }
    }
    public void ReduceCooldown(int amount)
    {
        if (cooldownTime > 0)
        {
            cooldownTime -= amount;

            if(cooldownTime < 0)
            {
                cooldownTime = 0;
            }
        }
    }
    public void SetCooldown()
    {
        cooldownTime = skillCooldown;
    }
    public void ResetCooldown()
    {
        cooldownTime = 0;
    }
    public int GetCooldown()
    {
        return cooldownTime;
    }
}
