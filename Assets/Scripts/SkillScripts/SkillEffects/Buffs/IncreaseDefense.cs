using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseDefense : EffectBase
{
    private float defDif;
    private void Awake()
    {
        effectName = "IncDef";
        effectDescription = "Increases characters defense by 60% of their combat defense";
        if (IconList.instance.FindIcon(effectName) != null)
        {
            effectIcon = IconList.instance.FindIcon(effectName);
        }
        effectStackable = false;

        effectRole = effectRole.BUFF;
        effectType = effectType.ONPLACED;
    }
    // Start is called before the first frame update
    void Start()
    {
        character = this.GetComponentInParent<CharacterControl>();
    }

    public override void Effect()
    {
        defDif = character.character.GetCombatDefense() * 0.6f;
        character.character.AdjustDefense(defDif);
    }

    public override void Destroy()
    {
        character.character.AdjustDefense(-defDif);
        RemoveValue();
        character.RemoveEffect(this);
    }
}
