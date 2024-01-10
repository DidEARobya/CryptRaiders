using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAI : MonoBehaviour
{
    private CharacterObject character;
    private CharacterControl control, target;
    private SkillManager skillManager;
    private CombatScript combatScript;

    private bool started = false;

    // Start is called before the first frame update
    public void Init(CharacterObject data, CharacterControl ctrl, SkillManager sklManager)
    {
        control = ctrl;
        character = data;
        skillManager = sklManager;

        target = null;
        started = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (started == true)
        {
            if (control.GetState() == characterStates.AUTO)
            {
                if (character.prio1 != null)
                {
                    if (character.prio1.GetCooldown() == 0)
                    {
                        Targetting(character.prio1);

                        if (target != null)
                        {
                            skillManager.PassInSkill(character.prio1);
                            skillManager.PassInTarget(target);
                            return;
                        }
                    }
                }
                if (character.prio2 != null)
                {
                    if (character.prio2.GetCooldown() == 0)
                    {
                        Targetting(character.prio2);

                        if (target != null)
                        {
                            skillManager.PassInSkill(character.prio2);
                            skillManager.PassInTarget(target);
                            return;
                        }
                    }
                }
                if (character.prio3 != null)
                {
                    if (character.prio3.GetCooldown() == 0)
                    {
                        Targetting(character.prio3);

                        if (target != null)
                        {
                            skillManager.PassInSkill(character.prio3);
                            skillManager.PassInTarget(target);
                            return;
                        }
                    }
                }
            }
        }
    }

    private CharacterControl Targetting(SkillObject skill)
    {
        List<CharacterControl> temp;

        if (skill.activeSkill.skillRole == skillRoles.ATTACK)
        {
            temp = combatScript.GetPlayerTeam();
            temp.Sort(SortByHealth);

            if (skill.activeSkill.skillType == skillType.AOE || skill.activeSkill.skillType == skillType.MULTITARGET)
            {
                for (int i = 0; i < temp.Count; i++)
                {
                    if (temp[i].GetState() != characterStates.DEAD)
                    {
                        target = temp[i];

                        return target;
                    }
                }
            }

            if (skill.activeSkill.skillType == skillType.SINGLETARGET)
            {
                target = temp[temp.Count - 1];

                return target;
            }
        }

        if (skill.activeSkill.skillRole == skillRoles.SUPPORT)
        {
            temp = combatScript.GetEnemyTeam();
            temp.Sort(SortByHealth);

            if (skill.activeSkill.skillType == skillType.SELF)
            {
                target = control;
                return target;
            }
            if (skill.activeSkill.skillType == skillType.AOE || skill.activeSkill.skillType == skillType.MULTITARGET)
            {
                for (int i = 0; i < temp.Count; i++)
                {
                    if (temp[i].GetState() != characterStates.DEAD)
                    {
                        target = temp[i];

                        return target;
                    }
                }
            }

            if (skill.activeSkill.skillType == skillType.SINGLETARGET)
            {
                for (int i = 0; i < temp.Count; i++)
                {
                    if (temp[i].GetState() != characterStates.DEAD)
                    {
                        target = temp[i];

                        return target;
                    }
                }
            }
        }

        if (skill.activeSkill.skillRole == skillRoles.REVIVE)
        {
            temp = combatScript.GetEnemyTeam();
            temp.Sort(SortByMaxHealth);

            if (skill.activeSkill.skillType == skillType.AOE || skill.activeSkill.skillType == skillType.MULTITARGET)
            {
                for (int i = 0; i < temp.Count; i++)
                {
                    if (temp[i].GetState() == characterStates.DEAD)
                    {
                        target = temp[i];

                        return target;
                    }
                }
            }

            if (skill.activeSkill.skillType == skillType.SINGLETARGET)
            {
                for (int i = temp.Count - 1; i > 0; i--)
                {
                    if (temp[i].GetState() == characterStates.DEAD)
                    {
                        target = temp[i];

                        return target;
                    }
                }
            }
        }

        if (skill.activeSkill.skillRole == skillRoles.HEAL)
        {
            temp = combatScript.GetEnemyTeam();
            temp.Sort(SortByHealth);

            if (skill.activeSkill.skillType == skillType.SELF)
            {
                if(control.character.GetHealth() < control.character.GetCombatHealth())
                {
                    target = control;
                    return target;
                }
            }

            if (skill.activeSkill.skillType == skillType.AOE || skill.activeSkill.skillType == skillType.MULTITARGET)
            {
                for (int i = 0; i < temp.Count; i++)
                {
                    if (temp[i].GetState() != characterStates.DEAD)
                    {
                        if (temp[i].character.GetHealth() < temp[i].character.GetCombatHealth())
                        {
                            target = temp[i];

                            return target;
                        }
                    }
                }
            }

            if (skill.activeSkill.skillType == skillType.SINGLETARGET)
            {
                for (int i = 0; i < temp.Count; i++)
                {
                    if (temp[i].GetState() != characterStates.DEAD)
                    {
                        if (temp[i].character.GetHealth() < temp[i].character.GetCombatHealth())
                        {
                            target = temp[i];

                            return target;
                        }
                    }
                }
            }
        }

        target = null;

        return target;
    }
    static int SortByHealth(CharacterControl p1, CharacterControl p2)
    {
        return p1.character.GetHealth().CompareTo(p2.character.GetHealth());
    }

    static int SortByMaxHealth(CharacterControl p1, CharacterControl p2)
    {
        return p1.character.GetCombatHealth().CompareTo(p2.character.GetCombatHealth());
    }

    public void SetCombatScript(CombatScript script)
    {
        combatScript = script;
    }
}


