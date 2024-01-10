using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Skills", menuName = "Skills/Alvi/AlviSkillTwo")]

public class AlviSkillTwo : SkillData
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
        float temp = skillParent.GetAttack() * 1.1f;

        for (int i = 0; i < controlList.Count; i++)
        {
            if (controlList[i].GetState() != characterStates.DEAD)
            {
                controlList[i].character.TakeDamage(temp);
                controlList[i].ClearEffects(effectRole.BUFF);

                Sleep sleep = controlList[i].AddComponent<Sleep>();
                controlList[i].AddEffect(sleep, 1);
            }
        }
    }
}
