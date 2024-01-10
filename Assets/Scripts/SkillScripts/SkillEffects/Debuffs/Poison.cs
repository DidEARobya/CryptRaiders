using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : EffectBase
{
    private void Awake()
    {
        effectName = "Poison";
        effectDescription = "Damages the champion by 7.5% of their max HP on each turn";
        if (IconList.instance.FindIcon(effectName) != null)
        {
            effectIcon = IconList.instance.FindIcon(effectName);
        }
        effectStackable = true;

        effectRole = effectRole.DEBUFF;
        effectType = effectType.ONTURN;
    }
    // Start is called before the first frame update
    void Start()
    {
        character = this.GetComponentInParent<CharacterControl>();
    }

    public override void Effect()
    {
        float temp = character.character.GetCombatHealth() * 0.075f;
        character.character.DealDamage(temp);
    }

    public override void Destroy()
    {
        RemoveValue();
        character.RemoveEffect(this);
    }
}
