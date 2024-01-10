using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Skills", menuName = "Skills/Vividus/VividusSkillTwo")]

public class VividusSkillTwo : SkillData
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
        float temp = skillParent.GetCombatHealth() * 0.3f;

        for (int i = 0; i < controlList.Count; i++)
        {
            if (controlList[i].GetState() != characterStates.DEAD)
            {
                controlList[i].character.Heal(temp);

                IncreaseDefense incDef = controlList[i].AddComponent<IncreaseDefense>();
                controlList[i].AddEffect(incDef, 2);
            }
        }
    }
}
