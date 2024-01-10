using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DecreaseSpeed : EffectBase
{
    private float spdDif;
    private void Awake()
    {
        effectName = "DecSpd";
        effectDescription = "Decreases characters speed by 30% of their combat speed";
        if (IconList.instance.FindIcon(effectName) != null)
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
        spdDif = character.character.GetCombatSpeed() * 0.3f;
        character.character.AdjustSpeed(-spdDif);
    }

    public override void Destroy()
    {
        character.character.AdjustSpeed(+spdDif);
        RemoveValue();
        character.RemoveEffect(this);
    }
}
