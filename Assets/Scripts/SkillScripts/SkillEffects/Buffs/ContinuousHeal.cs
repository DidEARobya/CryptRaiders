using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousHeal : EffectBase
{
    private void Awake()
    {
        effectName = "ContinuousHeal";
        effectDescription = "Heals the champion by 15% of their max HP on each turn";
        if (IconList.instance.FindIcon(effectName) != null)
        {
            effectIcon = IconList.instance.FindIcon(effectName);
        }
        effectStackable = true;

        effectRole = effectRole.BUFF;
        effectType = effectType.ONTURN;
    }
    // Start is called before the first frame update
    void Start()
    {
        character = this.GetComponentInParent<CharacterControl>();
    }

    public override void Effect()
    {
        float temp = character.character.GetCombatHealth() * 0.15f;
        character.character.Heal(temp);
    }

    public override void Destroy()
    {
        RemoveValue();
        character.RemoveEffect(this);
    }
}
