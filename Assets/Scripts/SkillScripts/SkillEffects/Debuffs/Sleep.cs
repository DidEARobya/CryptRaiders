using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Sleep : EffectBase
{
    private float initialHealth;

    private void Awake()
    {
        effectName = "Sleep";
        effectDescription = "Prevents the character from taking a turn. This is removed if the character takes damage.";
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
        initialHealth = character.character.GetHealth();
    }
    new void Update()
    {
        base.Update();

        if(character.character.GetHealth() < initialHealth)
        {
            this.Destroy();
        }
    }
    public override void Effect()
    {
        character.SetState(characterStates.INACTIVE);
    }

    public override void Destroy()
    {
        RemoveValue();
        character.RemoveEffect(this);
    }
}
