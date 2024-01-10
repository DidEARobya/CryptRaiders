using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Skills", menuName = "Skills/Vuduin/VuduinSkillOne")]
public class VuduinSkillOne : SkillData
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
        float temp = skillParent.GetAttack() * 2.1f;

        if (target.GetState() != characterStates.DEAD)
        {
            target.character.TakeDamage(temp);
        }
    }
}
