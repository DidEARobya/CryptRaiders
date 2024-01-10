using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Skills", menuName = "Skills/Kilnuak/KilnuakSkillThree")]
public class KilnuakSkillThree : SkillData
{
    public override void Activate(CharacterObject character, CharacterControl target, List<CharacterControl> controlList, int amount)
    {
        extraTurn = true;
        SetSkillParent(character);
        SelfTarget(target);

        character.GetControl().SetState(characterStates.INACTIVE);

        if (extraTurn == true)
        {
            character.AdjustTM(100);
        }
    }

    public override void SelfTarget(CharacterControl target)
    {
        if (target.GetState() != characterStates.DEAD)
        {
            IncreaseAttack incAtk = target.AddComponent<IncreaseAttack>();
            target.AddEffect(incAtk, 3);
        }
    }
}
