using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Skills", menuName = "Skills/Aiduin/AiduinSkillOne")]
public class AiduinSkillOne : SkillData
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
        float temp = skillParent.GetAttack() * 1.8f;

        for(int i = 0; i < 2; i++)
        {
            if(target.GetState() != characterStates.DEAD)
            {
                target.character.TakeDamage(temp);

                int rand = Random.Range(1, 101);

                if (rand <= 5)
                {
                    DecreaseAttack decAtk = target.AddComponent<DecreaseAttack>();
                    target.AddEffect(decAtk, 2);
                }
            }
        }
    }
}
