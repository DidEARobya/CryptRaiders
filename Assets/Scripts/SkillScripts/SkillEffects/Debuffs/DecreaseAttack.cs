using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DecreaseAttack : EffectBase
{
    private float atkDif;

    private void Awake()
    {
        effectName = "DecAtk";
        effectDescription = "Decreases characters attack by 50% of their combat attack";
        if(IconList.instance.FindIcon(effectName) != null)
        {
            effectIcon = IconList.instance.FindIcon(effectName);
        }
        effectStackable = false;

        effectRole = effectRole.DEBUFF;
        effectType = effectType.ONPLACED;
    }
    // Start is called before the first frame update
    void Start()
    {
        character = this.GetComponentInParent<CharacterControl>();
    }

    public override void Effect()
    {
        atkDif = character.character.GetCombatAttack() * 0.5f;
        character.character.AdjustAttack(-atkDif);
    }

    public override void Destroy()
    {
        character.character.AdjustAttack(atkDif);
        RemoveValue();
        character.RemoveEffect(this);
    }
}

