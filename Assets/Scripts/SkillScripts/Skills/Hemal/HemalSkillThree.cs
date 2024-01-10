using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Skills", menuName = "Skills/Hemal/HemalSkillThree")]
public class HemalSkillThree : SkillData
{
    public override void Activate(CharacterObject character, CharacterControl target, List<CharacterControl> controlList, int amount)
    {
        extraTurn = false;
        SetSkillParent(character);
        AllTargets(controlList);

        character.GetControl().SetState(characterStates.INACTIVE);

        if (extraTurn == true)
        {
            character.AdjustTM(100);
        }
    }

    public override void AllTargets(List<CharacterControl> controlList)
    {
        for (int i = 0; i < controlList.Count; i++)
        {
            if (controlList[i].GetState() == characterStates.DEAD)
            {
                controlList[i].SetState(characterStates.INACTIVE);

                float temp = controlList[i].character.GetCombatHealth() * 0.3f;
                controlList[i].character.Heal(temp);

                extraTurn = true;
            }

            controlList[i].character.AdjustTM(0.1f);
        }
    }
}
