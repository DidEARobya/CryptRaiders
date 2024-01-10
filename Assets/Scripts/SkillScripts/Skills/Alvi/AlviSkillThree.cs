using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Skills", menuName = "Skills/Alvi/AlviSkillThree")]
public class AlviSkillThree : SkillData
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
            if (controlList[i] != skillParent.GetControl() && controlList[i].GetState() != characterStates.DEAD)
            {
                controlList[i].character.ResetSkillCooldowns();
                controlList[i].character.AdjustTM(0.3f);
            }
        }
    }
}
