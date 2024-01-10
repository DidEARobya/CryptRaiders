using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skills", menuName = "Skills/Vuduin/VuduinSkillThree")]
public class VuduinSkillThree : SkillData
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
        float temp = skillParent.GetAttack() * 1.9f;

        for(int u = 0; u < 2; u++)
        {
            for (int i = 0; i < controlList.Count; i++)
            {
                if (controlList[i].GetState() != characterStates.DEAD)
                {
                    controlList[i].character.TakeDamage(temp);
                }
            }
        }
    }
}
