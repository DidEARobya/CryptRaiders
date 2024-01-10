using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SkillDisplay : MonoBehaviour
{
    private CombatScript combatScript;
    private new Camera camera;

    private GameObject visuals1, visuals2, visuals3;

    private GameObject descriptionObject;
    private MeshRenderer descriptionRenderer;
    private TextMesh descriptionText;

    private CharacterControl data;
    private CharacterControl currentData;

    private SkillManager skillManager;
    private SkillObject skillOne, skillTwo, skillThree;
    private GameObject skillOneSelect, skillTwoSelect, skillThreeSelect;

    private GameObject cooldownCounter;
    private GameObject cd2, cd3;

    private GameObject target;
    private string targetString;
    public void Init(CombatScript script)
    {
        currentData = null;
        combatScript = script;
        camera = Camera.main;
        this.transform.position = new Vector3(0, -4, 0);

        Object temp;
        temp = Resources.Load("Models/cooldownCounter");
        cooldownCounter = Instantiate(temp) as GameObject;

        temp = Resources.Load("Models/SkillOneSelect");
        skillOneSelect = Instantiate(temp) as GameObject;

        temp = Resources.Load("Models/SkillTwoSelect");
        skillTwoSelect = Instantiate(temp) as GameObject;

        temp = Resources.Load("Models/SkillThreeSelect");
        skillThreeSelect = Instantiate(temp) as GameObject;

        descriptionObject = new GameObject();
        descriptionObject.name = "SkillDesriptionObject";
        descriptionObject.transform.position = this.transform.position + new Vector3(0, 3, 0);

        descriptionText = descriptionObject.AddComponent<TextMesh>();
        descriptionRenderer = descriptionObject.GetComponent<MeshRenderer>();
        descriptionText.alignment = TextAlignment.Center;
        descriptionText.anchor = TextAnchor.LowerCenter;
        descriptionText.characterSize = 0.03f;
        descriptionText.fontSize = 200;
    }

    // Update is called once per frame
    void Update()
    {
        if(combatScript != null)
        {
            if (combatScript.GetActiveCharacter() != null)
            {
                data = combatScript.GetActiveCharacter();
            }

            if (data != null)
            {
                if (data != currentData)
                {
                    currentData = data;
                    CharacterObject temp = currentData.character;

                    if (temp.skillOne != null)
                    {
                        skillOne = temp.skillOne;
                    }
                    else
                    {
                        skillOne = null;
                    }

                    if (temp.skillTwo != null)
                    {
                        skillTwo = temp.skillTwo;
                    }
                    else
                    {
                        skillTwo = null;
                    }

                    if (temp.skillThree != null)
                    {
                        skillThree = temp.skillThree;
                    }
                    else
                    {
                        skillThree = null;
                    }

                    skillManager = data.GetComponentInParent<SkillManager>();

                    LoadSkills();
                }
                else
                {
                    if (skillTwo != null)
                    {
                        if (cd2 == null)
                        {
                            if (skillTwo.GetCooldown() != 0)
                            {
                                cd2 = Instantiate(cooldownCounter);
                                cd2.name = "CooldownCounter";
                                cd2.transform.SetParent(visuals2.transform);
                                cd2.transform.localPosition = Vector3.forward;
                                cd2.GetComponent<TextMesh>().text = skillTwo.GetCooldown().ToString();
                            }
                        }
                        else
                        {
                            UpdateCooldown(cd2, skillTwo);
                        }
                    }

                    if (skillThree != null)
                    {
                        if (cd3 == null)
                        {
                            if (skillThree.GetCooldown() != 0)
                            {
                                cd3 = Instantiate(cooldownCounter);
                                cd3.name = "CooldownCounter";
                                cd3.transform.SetParent(visuals3.transform);
                                cd3.transform.localPosition = Vector3.forward;
                                cd3.GetComponent<TextMesh>().text = skillTwo.GetCooldown().ToString();
                            }
                        }
                        else
                        {
                            UpdateCooldown(cd3, skillThree);
                        }
                    }
                }
            }

            if (skillManager != null && skillManager.GetActiveSkill() != null)
            {
                SkillObject tempSkill = skillManager.GetActiveSkill();

                if (tempSkill.activeSkill.skillName == skillOne.activeSkill.skillName)
                {
                    skillOneSelect.transform.position = visuals1.transform.position + new Vector3(0, 0, 1f);

                    skillOneSelect.SetActive(true);
                    skillTwoSelect.SetActive(false);
                    skillThreeSelect.SetActive(false);
                }
                else if (tempSkill.activeSkill.skillName == skillTwo.activeSkill.skillName)
                {
                    skillTwoSelect.transform.position = visuals2.transform.position + new Vector3(0, 0, 1f);

                    skillOneSelect.SetActive(false);
                    skillTwoSelect.SetActive(true);
                    skillThreeSelect.SetActive(false);
                }
                else if (tempSkill.activeSkill.skillName == skillThree.activeSkill.skillName)
                {
                    skillThreeSelect.transform.position = visuals3.transform.position + new Vector3(0, 0, 1f);

                    skillOneSelect.SetActive(false);
                    skillTwoSelect.SetActive(false);
                    skillThreeSelect.SetActive(true);
                }
            }
            else
            {
                skillOneSelect.SetActive(false);
                skillTwoSelect.SetActive(false);
                skillThreeSelect.SetActive(false);
            }

            if (GetTarget() != null)
            {
                descriptionRenderer = descriptionText.transform.gameObject.GetComponent<MeshRenderer>();
                SkillData temp = target.GetComponent<SkillReference>().skill.skillData;
                targetString = temp.skillName + "\n\n" + temp.skillDescription;
                TextWrapper(targetString);
            }
            else
            {
                descriptionText.text = null;
                targetString = null;
            }
        }
    }
    public void LoadSkills()
    {
        foreach (Transform child in this.transform)
        {
            {
                Destroy(child.gameObject);
            }
        }

        if (skillOne)
        {
            LoadSkillOne(skillOne);
        }
        if (skillTwo)
        {
            LoadSkillTwo(skillTwo);
        }
        if (skillThree)
        {
            LoadSkillThree(skillThree);
        }
    }

    private void LoadSkillOne(SkillObject skill)
    {
        visuals1 = Instantiate(skill.activeSkill.skillIcon);
        visuals1.AddComponent<SkillReference>();
        visuals1.GetComponent<SkillReference>().SetSkillReference(skill);
        visuals1.transform.SetParent(this.transform);
        visuals1.transform.localPosition = this.transform.position + new Vector3(-1.5f, 0f, 0f);
    }

    private void LoadSkillTwo(SkillObject skill)
    {
        visuals2 = Instantiate(skill.activeSkill.skillIcon);
        visuals2.AddComponent<SkillReference>();
        visuals2.GetComponent<SkillReference>().SetSkillReference(skill);
        visuals2.transform.SetParent(this.transform);
        visuals2.transform.localPosition = this.transform.position;

        if (skill.GetCooldown() != 0)
        {
            cd2 = Instantiate(cooldownCounter);
            cd2.name = "CooldownCounter";
            cd2.transform.SetParent(visuals2.transform);
            cd2.transform.localPosition = Vector3.forward;
            cd2.GetComponent<TextMesh>().text = skill.GetCooldown().ToString();
        }
    }

    private void LoadSkillThree(SkillObject skill)
    {
        visuals3 = Instantiate(skill.activeSkill.skillIcon);
        visuals3.AddComponent<SkillReference>();
        visuals3.GetComponent<SkillReference>().SetSkillReference(skill);
        visuals3.transform.SetParent(this.transform);
        visuals3.transform.localPosition = this.transform.position + new Vector3(1.5f, 0f, 0f);

        if (skill.GetCooldown() != 0)
        {
            cd3 = Instantiate(cooldownCounter);
            cd3.name = "CooldownCounter";
            cd3.transform.SetParent(visuals3.transform);
            cd3.transform.localPosition = Vector3.forward;
            cd3.GetComponent<TextMesh>().text = skill.GetCooldown().ToString();
        }
    }

    private void UpdateCooldown(GameObject cd, SkillObject skill)
    {
        if(skill.GetCooldown() != 0)
        {
            cd.GetComponent<TextMesh>().text = skill.GetCooldown().ToString();
        }
        else
        {
            cd.GetComponent<TextMesh>().text = " ";
        }
    }
    public GameObject GetTarget()
    {
        Vector3 rayStart = camera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hitData = Physics2D.Raycast(new Vector2(rayStart.x, rayStart.y), Vector2.zero, 0);

        if (hitData.collider && hitData.collider.GetComponent<SkillReference>())
        {
            target = hitData.transform.gameObject;
            return target;
        }

        return null;
    }

    public void TextWrapper(string text)
    {
        float rowLimit = 5f;

        string[] parts = text.Split(' ');
        string temp = "";
        descriptionText.text = "";

        for (int i = 0; i < parts.Length; i++)
        {
            descriptionText.text += parts[i] + " ";

            if (descriptionRenderer.bounds.extents.x > rowLimit)
            {
                descriptionText.text = temp.TrimEnd() + System.Environment.NewLine + parts[i] + " ";
            }

            temp = descriptionText.text;
        }
    }
}
