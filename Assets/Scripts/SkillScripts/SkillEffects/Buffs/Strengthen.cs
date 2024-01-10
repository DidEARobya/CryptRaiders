using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Strengthen : EffectBase
{
    private void Awake()
    {
        effectName = "Strengthen";
        effectDescription = "Decreases physical damage this champion recieves by 25%";
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
        character.character.AdjustDmgMulti(-0.25f);
    }

    public override void Destroy()
    {
        character.character.AdjustDmgMulti(0.25f);
        RemoveValue();
        character.RemoveEffect(this);
    }
}
