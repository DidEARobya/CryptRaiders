using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skills", menuName = "Skills/Vuduin/VuduinSkillTwo")]
public class VuduinSkillTwo : SkillData
{
    public override void Activate(CharacterObject character, CharacterControl target, List<CharacterControl> controlList, int amount)
    {
        extraTurn = false;
        SetSkillParent(character);
        SingleTarget(target);
        AllTargets(controlList);

        character.GetControl().SetState(characterStates.INACTIVE);

        if (extraTurn == true)
        {
            character.AdjustTM(100);
        }
    }

    public override void SingleTarget(CharacterControl target)
    {
        float temp = skillParent.GetAttack() * 3.8f;

        if (target.GetState() != characterStates.DEAD)
        {
            target.character.TakeDamage(temp);
        }
    }
    public override void AllTargets(List<CharacterControl> controlList)
    {
        float temp = skillParent.GetAttack() * 2.4f;

        for (int i = 0; i < controlList.Count; i++)
        {
            if (controlList[i].GetState() != characterStates.DEAD)
            {
                controlList[i].character.TakeDamage(temp);
            }
        }
    }
}
