using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skills", menuName = "Skills/Vividus/VividusSkillOne")]
public class VividusSkillOne : SkillData
{
    List<CharacterControl> team;
    public override void Activate(CharacterObject character, CharacterControl target, List<CharacterControl> controlList, int amount)
    {
        team = character.GetControl().GetComponentInParent<TeamScript>().GetTeam();

        extraTurn = false;
        SetSkillParent(character);
        SingleTarget(target);
        Heal();

        character.GetControl().SetState(characterStates.INACTIVE);

        if (extraTurn == true)
        {
            character.AdjustTM(100);
        }
    }

    public override void SingleTarget(CharacterControl target)
    {
        float temp = skillParent.GetAttack() * 1.8f;

        if (target.GetState() != characterStates.DEAD)
        {
            target.character.TakeDamage(temp);
        }
    }

    private void Heal()
    {
        float temp = skillParent.GetCombatHealth() * 0.05f;

        CharacterObject target = null;

        for(int i = 0;  i < team.Count; i++)
        {
            if(target != null)
            {
                if (team[i].character.GetHealth() < target.GetHealth())
                {
                    target = team[i].character;
                }
            }
            else
            {
                target = team[i].character;
            }
        }

        target.Heal(temp);
    }
}
