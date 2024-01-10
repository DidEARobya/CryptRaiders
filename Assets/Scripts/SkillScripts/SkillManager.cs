using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

public class SkillManager : MonoBehaviour
{
    private SkillObject activeSkill;
    private SkillObject tempSkill;

    private TeamScript targetTeam;

    private List<CharacterControl> controlList;
    private CharacterControl targetControl;
    private CharacterObject targetObject;

    private CharacterControl characterControl;
    private CharacterObject characterObject;
    private TeamScript characterTeam;

    public new Camera camera;

    private float wait;

    private void Start()
    {
        wait = 2f;
        camera = Camera.main;

        characterControl = this.GetComponentInParent<CharacterControl>();
        characterObject = characterControl.character;
        characterTeam = characterControl.GetComponentInParent<TeamScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (characterControl.GetState() == characterStates.ACTIVE)
        {
            PlayerTurn();
        }
        else if (characterControl.GetState() == characterStates.AUTO)
        {
            if (activeSkill != null)
            {
                if (targetControl != null)
                {
                    wait -= Time.deltaTime;

                    targetObject = targetControl.character;
                    targetTeam = targetControl.GetComponentInParent<TeamScript>();

                    controlList = targetTeam.GetTeam();

                    if (wait <= 0)
                    {
                        UseSkill();
                        wait = 2f;
                    }
                }
            }
        }
    }

    public CharacterControl SetTarget()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 rayStart = camera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitData = Physics2D.Raycast(new Vector2(rayStart.x, rayStart.y), Vector2.zero, 0);

            if (hitData.collider && hitData.collider.GetComponentInParent<CharacterControl>())
            {
                targetControl = hitData.transform.gameObject.GetComponentInParent<CharacterControl>();
                return targetControl;
            }
        }

        return null;
    }
    public SkillObject SetActiveSkill()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 rayStart = camera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitData = Physics2D.Raycast(new Vector2(rayStart.x, rayStart.y), Vector2.zero, 0);

            if (hitData.collider)
            {
                if (hitData.collider.GetComponent<SkillReference>())
                {
                    SkillObject temp = hitData.collider.GetComponent<SkillReference>().skill;

                    if (temp.activeSkill.skillName == characterObject.skillOne.activeSkill.skillName)
                    {
                        tempSkill = characterObject.skillOne;
                    }
                    else if (temp.activeSkill.skillName == characterObject.skillTwo.activeSkill.skillName)
                    {
                        tempSkill = characterObject.skillTwo;
                    }
                    else if (temp.activeSkill.skillName == characterObject.skillThree.activeSkill.skillName)
                    {
                        tempSkill = characterObject.skillThree;
                    }

                    if (tempSkill.GetCooldown() == 0)
                    {
                        return tempSkill;
                    }
                }
            }
        }

        return null;
    }
    private void UseSkill()
    {
        activeSkill.Activate(characterObject, targetControl, controlList, activeSkill.activeSkill.amountOfTargets);
        activeSkill.SetCooldown();

        targetObject = null;
        activeSkill = null;
    }
    public SkillObject GetActiveSkill()
    {
        return activeSkill;
    }
    private void PlayerTurn()
    {
        if (SetActiveSkill() != null)
        {
            if (activeSkill != null)
            {
                if (activeSkill != tempSkill)
                {
                    targetObject = null;
                    targetTeam = null;
                    controlList = null;

                    activeSkill = tempSkill;
                }
            }
            else
            {
                activeSkill = tempSkill;
            }
        }

        if (activeSkill != null)
        {
            if (SetTarget() != null)
            {
                targetObject = targetControl.character;
                targetTeam = targetControl.GetComponentInParent<TeamScript>();

                controlList = targetTeam.GetTeam();
            }

            if (targetObject != null)
            {
                SkillData temp = activeSkill.activeSkill;

                if (temp.skillRole == skillRoles.ATTACK && characterTeam != targetTeam)
                {
                    if (temp.skillType == skillType.AOE || temp.skillType == skillType.MULTITARGET)
                    {
                        UseSkill();
                    }
                    else if (temp.skillType == skillType.SINGLETARGET && targetControl.GetState() != characterStates.DEAD)
                    {
                        UseSkill();
                    }
                }
                else if (temp.skillRole == skillRoles.SUPPORT && characterTeam == targetTeam)
                {
                    if (temp.skillType == skillType.SELF)
                    {
                        if (targetObject == characterObject)
                        {
                            UseSkill();
                        }
                    }
                    else
                    {
                        UseSkill();
                    }
                }
                else if (temp.skillRole == skillRoles.HEAL && characterTeam == targetTeam)
                {
                    if(temp.skillType == skillType.SELF)
                    {
                        if(targetObject == characterObject && targetObject.GetHealth() != targetObject.GetCombatHealth())
                        {
                            UseSkill();
                        }
                    }
                    else if (temp.skillType == skillType.SINGLETARGET)
                    {
                        if (targetObject.GetHealth() != targetObject.GetCombatHealth())
                        {
                            UseSkill();
                        }
                    }
                    else if (temp.skillType == skillType.AOE)
                    {
                        List<CharacterControl> team;
                        team = targetTeam.GetTeam();

                        for (int i = 0; i < team.Count; i++)
                        {
                            if (team[i].character.GetHealth() != team[i].character.GetCombatHealth())
                            {
                                UseSkill();
                            }
                        }
                    }
                }
                else if (temp.skillRole == skillRoles.REVIVE && characterTeam == targetTeam)
                {
                    if (temp.skillType == skillType.AOE)
                    {
                        List<CharacterControl> team;
                        team = targetTeam.GetTeam();

                        for (int i = 0; i < team.Count; i++)
                        {
                            if (team[i].GetState() == characterStates.DEAD)
                            {
                                UseSkill();
                            }
                        }
                    }
                    else if (temp.skillType == skillType.SINGLETARGET && targetControl.GetState() == characterStates.DEAD)
                    {
                        UseSkill();
                    }
                }
            }
        }
    }
    public void PassInTarget(CharacterControl temp)
    {
        targetControl = temp;
    }

    public void PassInSkill(SkillObject temp)
    {
        activeSkill = temp;
    }
}
