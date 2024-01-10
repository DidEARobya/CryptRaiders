using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum effectRole
{
    BUFF,
    DEBUFF
}

public enum effectType
{
    ONPLACED,
    ONTURN,
    ONDESTROY
}

public class EffectBase : MonoBehaviour
{
    protected CharacterControl character;

    public string effectName;
    public string effectDescription;
    public bool effectStackable;
    public GameObject effectIcon;

    public effectRole effectRole;
    public effectType effectType;

    public int duration;

    private GameObject durationCounter;
    private TextMeshPro durText;
    public int objectVal;

    protected bool start;
    private bool active;
    private bool placed;
    private bool durSet;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponentInParent<CharacterControl>() != null)
        {
            character = GetComponentInParent<CharacterControl>();
        }

        start = false;
        active = false;
        durSet = false;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (start == true)
        {
            EffectUpdate();
        }
    }

    public void StartDebuff(int dur)
    {
        if(objectVal == 0)
        {
            objectVal = EffectValueManager.instance.GenerateValue();
        }

        duration = dur;
        placed = false;
        start = true;
    }
    protected void EffectUpdate()
    {
        if (duration <= 0 || character.GetState() == characterStates.DEAD)
        {
            Destroy();
        }

        if (durationCounter == null)
        {
            GameObject temp;

            if (transform.Find(objectVal.ToString()) != null)
            {
                temp  = transform.Find(objectVal.ToString()).gameObject;

                if (temp.transform.Find("Duration").gameObject != null)
                {
                    durationCounter = transform.Find(objectVal.ToString()).Find("Duration").gameObject;
                }
            }
            else
            {
                
            }

            if(durationCounter != null)
            {
                durText = durationCounter.GetComponent<TextMeshPro>();
                durText.text = duration.ToString();
            }
        }
        else
        {
            if(durText == null)
            {
                durText = durationCounter.GetComponent<TextMeshPro>();
            }

            durText.text = duration.ToString();
        }

        if (effectType == effectType.ONPLACED)
        {
            if (placed == false)
            {
                Effect();
                placed = true;
            }

            if (character.GetState() == characterStates.ACTIVE || character.GetState() == characterStates.AUTO)
            {
                if(active == false)
                {
                    active = true;
                }
            }
            else if (character.GetState() != characterStates.ACTIVE || character.GetState() != characterStates.AUTO)
            {
                if (active == true)
                {
                    duration = duration - 1;
                }
                active = false;
            }
        }
        else if (effectType == effectType.ONTURN)
        {
            if (character.GetState() == characterStates.ACTIVE || character.GetState() == characterStates.AUTO)
            {
                if (active == false)
                {
                    Effect();
                    duration = duration - 1;


                    active = true;
                }

            }
            else if (character.GetState() != characterStates.ACTIVE || character.GetState() != characterStates.AUTO)
            {
                active = false;
            }
        }
    }

    protected void RemoveValue()
    {
        EffectValueManager.instance.RemoveValue(objectVal);
    }
    public virtual void Effect()
    {

    }

    public virtual void Destroy()
    {

    }
}
