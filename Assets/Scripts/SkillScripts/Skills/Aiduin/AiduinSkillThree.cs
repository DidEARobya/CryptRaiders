using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Skills", menuName = "Skills/Aiduin/AiduinSkillThree")]
public class AiduinSkillThree : SkillData
{
    public override void Activate(CharacterObject character, CharacterControl target, List<CharacterControl> controlList, int amount)
    {
        extraTurn = false;
        SetSkillParent(character);
        MultiTarget(controlList, amount);

        character.GetControl().SetState(characterStates.INACTIVE);

        if (extraTurn == true)
        {
            character.AdjustTM(100);
        }
    }

    public override void MultiTarget(List<CharacterControl> controlList, int amount)
    {
        float temp = skillParent.GetAttack() * 2.2f;
        int temptarget;

        for (int i = 0; i < amount; i++)
        {
            bool check = false;

            while (check == false)
            {
                temptarget = Random.Range(0, controlList.Count);

                if (controlList[temptarget].GetState() != characterStates.DEAD)
                {
                    controlList[temptarget].character.TakeDamage(temp);

                    int rand = Random.Range(1, 101);

                    if (rand <= 75)
                    {
                        Poison poison = controlList[temptarget].AddComponent<Poison>();
                        controlList[temptarget].AddEffect(poison, 2);
                    }

                    check = true;
                }
            }
        }
    }
}
