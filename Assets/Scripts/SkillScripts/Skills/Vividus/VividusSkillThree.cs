using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skills", menuName = "Skills/Vividus/VividusSkillThree")]
public class VividusSkillThree : SkillData
{
    public override void Activate(CharacterObject character, CharacterControl target, List<CharacterControl> controlList, int amount)
    {
        extraTurn = false;
        SetSkillParent(character);
        SingleTarget(target);

        character.GetControl().SetState(characterStates.INACTIVE);

        if (extraTurn == true)
        {
            character.AdjustTM(100);
        }
    }

    public override void SingleTarget(CharacterControl target)
    {
        if(target.GetState() == characterStates.DEAD)
        {
            float temp = target.character.GetCombatHealth() * 0.75f;

            target.SetState(characterStates.INACTIVE);
            target.character.AdjustTM(0.5f);
            target.character.Heal(temp);
        }
    }
}
