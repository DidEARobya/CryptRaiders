using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillReference : MonoBehaviour
{
    public SkillObject skill;
    public void SetSkillReference(SkillObject skl)
    {
        skill = skl;
    }
}
